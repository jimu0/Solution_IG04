
namespace Mycelia;

public static class WorldDynamics
{
    // /// <summary>
    // /// 在时间 dt 内推进整个世界：计算延迟引力 -> 加速度 -> 相对论速度合成 -> 推进位置/时间 -> 记录历史。
    // /// 这是唯一会“改变状态”的函数。
    // /// </summary>
    // public static void Step(IReadOnlyList<Body>? bodies, ref SimTime time, double dt, int anchorId = 0)
    // {
    //     // 宇宙唯一会“动”的地方：把状态 S 在 dt 内推进到 S'
    //     if (bodies == null || bodies.Count == 0 || dt <= 0.0) return;
    //
    //     // 建立 id -> index 的查找表，后续用于父子关联
    //     int count = bodies.Count;
    //     var idToIndex = new Dictionary<int, int>(count);
    //     for (int i = 0; i < count; i++) idToIndex[bodies[i].id] = i;
    //
    //     // 先把已附着的体同步到宿主的局部坐标系（包含旋转）
    //     for (int i = 0; i < count; i++)
    //     {
    //         Body body = bodies[i];
    //         if (body.parentId < 0) continue;
    //         if (!idToIndex.TryGetValue(body.parentId, out int parentIndex)) continue;
    //
    //         Body parent = bodies[parentIndex];
    //         Vec2Double rotatedLocal = Rotate(body.localPosition, parent.rotation);
    //         body.position = parent.position + rotatedLocal;
    //         body.velocity = parent.velocity + Rotate(body.localVelocity, parent.rotation) + AngularVelocityCross(parent.angularVelocity, rotatedLocal);
    //     }
    //
    //     // 先做快照，确保本步以同一时刻的数据计算（避免“先更新的体影响后更新的体”）
    //     var positions = new Vec2Double[count];
    //     var velocities = new Vec2Double[count];
    //     var masses = new double[count];
    //     for (int i = 0; i < count; i++)
    //     {
    //         // 复制当前状态，后续计算只读这些快照
    //         positions[i] = bodies[i].position;
    //         velocities[i] = bodies[i].velocity;
    //         masses[i] = bodies[i].mass;
    //     }
    //
    //     var forces = new Vec2Double[count];
    //     double c = WorldConstants.C;
    //     double cSq = c * c;
    //     const double minDistSq = 1e-12;
    //
    //     // 1) 延迟引力：对每个目标体 i，累积其它体 j 的影响
    //     for (int i = 0; i < count; i++)
    //     {
    //         // 数值锚点不推进，但仍可作为引力源
    //         if (bodies[i].id == anchorId) continue;
    //         if (bodies[i].parentId >= 0) continue;
    //
    //         double massI = masses[i];
    //         if (massI <= 0.0) continue;
    //
    //         Vec2Double force = Vec2Double.Zero;
    //         Vec2Double targetPos = positions[i];
    //         for (int j = 0; j < count; j++)
    //         {
    //             if (i == j) continue;
    //             if (masses[j] <= 0.0) continue;
    //
    //             // 用当前距离估计信号延迟时间（光速传播）
    //             Vec2Double toSource = positions[j] - targetPos;
    //             double distSq = toSource.LengthSq();
    //             if (distSq < minDistSq) distSq = minDistSq;
    //             double dist = Math.Sqrt(distSq);
    //             double delay = dist / c;
    //
    //             // 线性回推到“过去位置”，得到延迟引力方向
    //             Vec2Double retardedPos;
    //             double retardedTime = time.t - delay;
    //             if (bodies[j].history.TryGetPositionAt(new SimTime(retardedTime), out Vec2 pastPos))
    //             {
    //                 retardedPos = new Vec2Double(pastPos.x, pastPos.y);
    //             }
    //             else
    //             {
    //                 retardedPos = positions[j] - velocities[j] * delay;
    //             }
    //             Vec2Double dir = retardedPos - targetPos;
    //             double dirSq = dir.LengthSq();
    //             if (dirSq < minDistSq) continue;
    //
    //             // 经典引力：G * m / r^3 * r（方向乘以标量）
    //             double dirDist = Math.Sqrt(dirSq);
    //             double invR3 = 1.0 / (dirSq * dirDist);
    //             force += dir * (WorldConstants.G * massI * masses[j] * invR3);
    //         }
    //
    //         // 把本体的力缓存下来，后面统一更新动量
    //         forces[i] = force;
    //     }
    //
    //     // 2) 用相对论动量更新速度
    //     for (int i = 0; i < count; i++)
    //     {
    //         Body body = bodies[i];
    //         if (body.id == anchorId) continue;
    //         if (body.parentId >= 0) continue;
    //         if (masses[i] <= 0.0) continue;
    //
    //         Vec2Double v = velocities[i];
    //         Vec2Double p = MomentumFromVelocity(v, masses[i], cSq);
    //         Vec2Double newP = p + forces[i] * dt;
    //         Vec2Double newV = VelocityFromMomentum(newP, masses[i], cSq);
    //
    //         // 3) 位置推进：x' = x + v' * dt
    //         body.velocity = newV;
    //         body.position = positions[i] + newV * dt;
    //     }
    //
    //     // 3) 捕获/合并：小质量体与宿主半径相交时，按“陷入程度”决定是否合并
    //     for (int i = 0; i < count; i++)
    //     {
    //         Body body = bodies[i];
    //         if (body.parentId >= 0) continue;
    //         if (body.id == anchorId) continue;
    //         if (body.mass <= 0.0) continue;
    //
    //         for (int j = 0; j < count; j++)
    //         {
    //             if (i == j) continue;
    //             Body host = bodies[j];
    //             if (host.parentId >= 0) continue;
    //             if (host.mass <= 0.0) continue;
    //             if (host.mass < body.mass) continue;
    //
    //             double combinedRadius = host.radius + body.radius;
    //             if (combinedRadius <= 0.0) continue;
    //
    //             Vec2Double delta = body.position - host.position;
    //             double distSq = delta.LengthSq();
    //             if (distSq >= combinedRadius * combinedRadius) continue;
    //
    //             double dist = Math.Sqrt(distSq);
    //             double penetration = combinedRadius - dist;
    //             double penetrationRatio = Clamp01(penetration / combinedRadius);
    //             double damping = 1.0 - penetrationRatio;
    //
    //             Vec2Double relV = body.velocity - host.velocity;
    //             Vec2Double dampedRelV = relV * damping;
    //             double escapeSpeed = Math.Sqrt(2.0 * WorldConstants.G * host.mass / Math.Max(host.radius, dist));
    //             if (dampedRelV.LengthSq() > escapeSpeed * escapeSpeed) continue;
    //
    //             double totalMass = host.mass + body.mass;
    //             if (totalMass <= 0.0) continue;
    //
    //             Vec2Double newPos = (host.position * host.mass + body.position * body.mass) / totalMass;
    //             Vec2Double newVel = (host.velocity * host.mass + body.velocity * body.mass) / totalMass;
    //             double newRadius = CombineRadius(host.radius, body.radius);
    //             double mergedInertia = InertiaDisk(totalMass, newRadius);
    //             double angularMomentum = InertiaDisk(host.mass, host.radius) * host.angularVelocity;
    //             angularMomentum += InertiaDisk(body.mass, body.radius) * body.angularVelocity;
    //             angularMomentum += CrossZ(host.position - newPos, host.velocity * host.mass);
    //             angularMomentum += CrossZ(body.position - newPos, body.velocity * body.mass);
    //
    //             host.mass = totalMass;
    //             host.position = newPos;
    //             host.velocity = newVel;
    //             host.radius = newRadius;
    //             host.angularVelocity = mergedInertia > 1e-18 ? angularMomentum / mergedInertia : 0.0;
    //
    //             Vec2Double dir = dist > 1e-12 ? delta / dist : Vec2Double.Right;
    //             double attachRadius = host.radius * (1.0 - penetrationRatio);
    //             Vec2Double localAttach = Rotate(dir * attachRadius, -host.rotation);
    //             body.parentId = host.id;
    //             body.localPosition = localAttach;
    //             body.localVelocity = Vec2Double.Zero;
    //             body.rotation = host.rotation;
    //             body.angularVelocity = host.angularVelocity;
    //             body.mass = 0.0;
    //             body.position = host.position + body.localPosition;
    //             body.velocity = host.velocity;
    //
    //             break;
    //         }
    //     }
    //
    //     // 捕获后再次同步附着体的世界坐标，避免宿主质心变化带来的偏移
    //     for (int i = 0; i < count; i++)
    //     {
    //         Body body = bodies[i];
    //         if (body.parentId < 0) continue;
    //         if (!idToIndex.TryGetValue(body.parentId, out int parentIndex)) continue;
    //
    //         Body parent = bodies[parentIndex];
    //         Vec2Double rotatedLocal = Rotate(body.localPosition, parent.rotation);
    //         body.position = parent.position + rotatedLocal;
    //         body.velocity = parent.velocity + Rotate(body.localVelocity, parent.rotation) + AngularVelocityCross(parent.angularVelocity, rotatedLocal);
    //     }
    //
    //     // 4) 旋转推进
    //     for (int i = 0; i < count; i++)
    //     {
    //         bodies[i].rotation += bodies[i].angularVelocity * dt;
    //     }
    //
    //     // 4) 时间推进
    //     time += dt;
    //
    //     // 5) 记录历史（用于因果一致性的轨迹查询）
    //     for (int i = 0; i < count; i++)
    //     {
    //         Vec2Double pos = bodies[i].position;
    //         // 目前历史使用 Vec2(float)，这里做一次显式降精度
    //         bodies[i].history.AddSample(time, new Vec2((float)pos.x, (float)pos.y));
    //     }
    //
    //     static Vec2Double MomentumFromVelocity(Vec2Double v, double mass, double cSq)
    //     {
    //         double vSq = v.LengthSq();
    //         if (vSq <= 1e-24) return v * mass;
    //         double gamma = 1.0 / Math.Sqrt(1.0 - vSq / cSq);
    //         return v * (gamma * mass);
    //     }
    //
    //     static Vec2Double VelocityFromMomentum(Vec2Double p, double mass, double cSq)
    //     {
    //         double pSq = p.LengthSq();
    //         if (pSq <= 1e-24) return p / mass;
    //         double gamma = Math.Sqrt(1.0 + pSq / (mass * mass * cSq));
    //         return p / (gamma * mass);
    //     }
    //
    //     static double Clamp01(double v)
    //     {
    //         if (v < 0.0) return 0.0;
    //         if (v > 1.0) return 1.0;
    //         return v;
    //     }
    //
    //     static double CombineRadius(double a, double b)
    //     {
    //         double volume = a * a * a + b * b * b;
    //         return volume <= 0.0 ? 0.0 : Math.Pow(volume, 1.0 / 3.0);
    //     }
    //
    //     static double InertiaDisk(double mass, double radius)
    //     {
    //         return 0.5 * mass * radius * radius;
    //     }
    //
    //     static double CrossZ(Vec2Double a, Vec2Double b)
    //     {
    //         return a.x * b.y - a.y * b.x;
    //     }
    //
    //     static Vec2Double AngularVelocityCross(double omega, Vec2Double r)
    //     {
    //         return new Vec2Double(-omega * r.y, omega * r.x);
    //     }
    //
    //     static Vec2Double Rotate(Vec2Double v, double angle)
    //     {
    //         double cos = Math.Cos(angle);
    //         double sin = Math.Sin(angle);
    //         return new Vec2Double(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    //     }
    // }
}
