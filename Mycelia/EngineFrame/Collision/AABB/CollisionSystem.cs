namespace Mycelia.Collision.AABB;

public class CollisionSystem
{
    private readonly List<Collider> _colliders = new();
    private readonly HashSet<(Collider, Collider)> _lastFrame = new();

    // Contact bias: small penetration is tolerated to reduce jitter/sticking.
    private const float PenetrationSlop = 0.005f;
    private const float PositionCorrectionPercent = 0.8f;
    private const float BlockEpsilon = 1e-4f;
    private const float SweepSkin = 1e-4f;
    private const float Restitution = 0.2f;

    public void Add(Collider c) => _colliders.Add(c);
    public void Remove(Collider c) => _colliders.Remove(c);
    public void Clear() => _colliders.Clear();

    public bool WouldBeBlocked(Collider mover, Vec2 targetPosition)
    {
        if (mover == null || mover.isTrigger) return false;
        Vec2 predicted = PredictMoveAndSlide(mover, mover.bounds.position, targetPosition - mover.bounds.position);
        return (predicted - targetPosition).LengthSq() > BlockEpsilon * BlockEpsilon;
    }

    public Vec2 PredictMoveAndSlide(Collider mover, Vec2 startPosition, Vec2 delta)
    {
        if (mover == null || mover.isTrigger)
        {
            return startPosition + delta;
        }

        Vec2 pos = startPosition;
        float allowedX = SweepAxis(mover, pos, delta.x, moveX: true);
        pos.x += allowedX;

        float allowedY = SweepAxis(mover, pos, delta.y, moveX: false);
        pos.y += allowedY;

        return pos;
    }

    private float SweepAxis(Collider mover, in Vec2 currentPos, float delta, bool moveX)
    {
        if (System.MathF.Abs(delta) <= BlockEpsilon)
        {
            return 0f;
        }

        float halfW = mover.bounds.size.x * 0.5f;
        float halfH = mover.bounds.size.y * 0.5f;
        float allowed = delta;

        float curLeft = currentPos.x - halfW;
        float curRight = currentPos.x + halfW;
        float curTop = currentPos.y - halfH;
        float curBottom = currentPos.y + halfH;

        for (int i = 0; i < _colliders.Count; i++)
        {
            Collider other = _colliders[i];
            if (ReferenceEquals(other, mover) || other.isTrigger)
            {
                continue;
            }

            AABB ob = other.bounds;
            bool overlapPerp = moveX
                ? RangesOverlap(curTop, curBottom, ob.Top, ob.Bottom)
                : RangesOverlap(curLeft, curRight, ob.Left, ob.Right);
            if (!overlapPerp)
            {
                continue;
            }

            if (moveX)
            {
                if (delta > 0f)
                {
                    if (curRight <= ob.Left + BlockEpsilon)
                    {
                        float maxDelta = ob.Left - SweepSkin - curRight;
                        if (maxDelta < allowed)
                        {
                            allowed = maxDelta;
                        }
                    }
                }
                else
                {
                    if (curLeft >= ob.Right - BlockEpsilon)
                    {
                        float minDelta = ob.Right + SweepSkin - curLeft;
                        if (minDelta > allowed)
                        {
                            allowed = minDelta;
                        }
                    }
                }
            }
            else
            {
                if (delta > 0f)
                {
                    if (curBottom <= ob.Top + BlockEpsilon)
                    {
                        float maxDelta = ob.Top - SweepSkin - curBottom;
                        if (maxDelta < allowed)
                        {
                            allowed = maxDelta;
                        }
                    }
                }
                else
                {
                    if (curTop >= ob.Bottom - BlockEpsilon)
                    {
                        float minDelta = ob.Bottom + SweepSkin - curTop;
                        if (minDelta > allowed)
                        {
                            allowed = minDelta;
                        }
                    }
                }
            }
        }

        if (delta > 0f)
        {
            return System.MathF.Max(0f, allowed);
        }

        return System.MathF.Min(0f, allowed);
    }

    private static bool RangesOverlap(float minA, float maxA, float minB, float maxB)
    {
        return maxA > minB + BlockEpsilon && minA < maxB - BlockEpsilon;
    }

    public void Step()
    {
        HashSet<(Collider, Collider)> currentFrame = new();

        for (int i = 0; i < _colliders.Count; i++)
        {
            for (int j = i + 1; j < _colliders.Count; j++)
            {
                Collider a = _colliders[i];
                Collider b = _colliders[j];

                if (!TryGetContact(a.bounds, b.bounds, out Vec2 normal, out float penetration))
                {
                    continue;
                }

                (Collider, Collider) pair = (a, b);
                currentFrame.Add(pair);

                bool existed = _lastFrame.Contains(pair);
                if (!existed)
                {
                    a.OnEnter?.Invoke(b);
                    b.OnEnter?.Invoke(a);
                }
                else
                {
                    a.OnStay?.Invoke(b);
                    b.OnStay?.Invoke(a);
                }

                if (!a.isTrigger && !b.isTrigger)
                {
                    ResolveContact(a, b, normal, penetration);
                }
            }
        }

        foreach ((Collider, Collider) pair in _lastFrame)
        {
            if (!currentFrame.Contains(pair))
            {
                pair.Item1.OnExit?.Invoke(pair.Item2);
                pair.Item2.OnExit?.Invoke(pair.Item1);
            }
        }

        _lastFrame.Clear();
        foreach ((Collider, Collider) pair in currentFrame)
        {
            _lastFrame.Add(pair);
        }
    }

    private static void ResolveContact(Collider a, Collider b, in Vec2 normal, float penetration)
    {
        float invMassA = GetSolveInverseMass(a);
        float invMassB = GetSolveInverseMass(b);
        float invMassSum = invMassA + invMassB;
        if (invMassSum <= 0f)
        {
            return;
        }

        // 1) XY 分离式位置校正
        float correctionDepth = System.MathF.Max(penetration - PenetrationSlop, 0f);
        if (correctionDepth > 0f)
        {
            Vec2 correction = normal * (correctionDepth * PositionCorrectionPercent / invMassSum);
            if (invMassA > 0f) a.bounds.position += correction * invMassA;
            if (invMassB > 0f) b.bounds.position -= correction * invMassB;
            SyncOwnerPosition(a);
            SyncOwnerPosition(b);
        }

        // 2)具有恢复力的线速度冲量（反弹力）
        Rigidbody2D? rbA = a.rigidbody2D;
        Rigidbody2D? rbB = b.rigidbody2D;
        Vec2 velA = rbA?.Velocity ?? Vec2.Zero;
        Vec2 velB = rbB?.Velocity ?? Vec2.Zero;
        Vec2 relativeVelocity = velA - velB;

        float velAlongNormal = Vec2.Dot(relativeVelocity, normal);
        if (velAlongNormal >= 0f)
        {
            return;
        }

        float impulseScalar = -(1f + Restitution) * velAlongNormal / invMassSum;
        Vec2 impulse = normal * impulseScalar;

        if (rbA != null && invMassA > 0f) rbA.Velocity += impulse * invMassA;
        if (rbB != null && invMassB > 0f) rbB.Velocity -= impulse * invMassB;
    }

    private static bool TryGetContact(in AABB a, in AABB b, out Vec2 normal, out float penetration)
    {
        normal = Vec2.Zero;
        penetration = 0f;

        if (!TryGetOverlap(a, b, out float overlapX, out float overlapY))
        {
            return false;
        }

        if (overlapX < overlapY)
        {
            float dir = a.Center.x < b.Center.x ? -1f : 1f;
            normal = new Vec2(dir, 0f);
            penetration = overlapX;
        }
        else
        {
            float dir = a.Center.y < b.Center.y ? -1f : 1f;
            normal = new Vec2(0f, dir);
            penetration = overlapY;
        }

        return true;
    }

    private static bool TryGetOverlap(in AABB a, in AABB b, out float overlapX, out float overlapY)
    {
        overlapX = System.MathF.Min(a.Right, b.Right) - System.MathF.Max(a.Left, b.Left);
        overlapY = System.MathF.Min(a.Bottom, b.Bottom) - System.MathF.Max(a.Top, b.Top);
        return overlapX > 0f && overlapY > 0f;
    }

    private static float GetSolveInverseMass(Collider c)
    {
        if (c.isStatic || c.rigidbody2D == null)
        {
            return 0f;
        }

        Rigidbody2D rb = c.rigidbody2D;
        if (rb.IsKinematic || rb.InverseMass <= 0f)
        {
            return 0f;
        }

        return rb.InverseMass;
    }

    private static void SyncOwnerPosition(Collider c)
    {
        if (c.rigidbody2D == null)
        {
            return;
        }

        c.rigidbody2D.Owner.tsf.postion = c.bounds.position;
    }
}
