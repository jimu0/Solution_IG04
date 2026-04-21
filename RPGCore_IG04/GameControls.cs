using Mycelia;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private const float MoveInputEpsilonSq = 1e-6f;
    private const float FacingUpdateEpsilonSq = 0.001f;
    private readonly float speed = 5f;
    private BattleGame? battleGame => RPGMode.battleGame;
    public Role[]? roles;

    public void Behavior(CtrlInput ctrlInput)
    {
        if (battleGame == null || roles == null) return;
        for (int n = 0; n < ctrlInput.NumberOfPlayers; n++)
        {
            PawnMove(ctrlInput.pawnInputs[n], n);
            PawnJump(ctrlInput.pawnInputs[n], n);
        }
    }


    private void PawnMove(PawnInput input, int n)
    {
        if (roles == null) return;

        Role role = roles[n];
        var rb = role.rigidbody2D;
        if (rb == null) return;

        Vec2 moveInput = new(input.move.x, input.move.z);

        if (moveInput.LengthSq() > MoveInputEpsilonSq)
        {
            moveInput = moveInput.Normalized();

            // 🎯 目标水平速度
            float targetSpeed = speed;
            float accel = 20f; // 控制“加速手感”

            float current = rb.Velocity.x;
            float target = moveInput.x * targetSpeed;

            // 平滑逼近（比直接赋值更自然）
            float newVelX = MoveTowards(current, target, accel * (float)WTime.fixedDt);

            rb.Velocity = new(newVelX, rb.Velocity.y);

            // 朝向更新
            if (moveInput.LengthSq() > FacingUpdateEpsilonSq) role.tsf.direction = moveInput;
        }
        else
        {
            // 无输入 → 逐渐减速（摩擦替代）
            float decel = 25f;
            rb.Velocity = new Vec2(MoveTowards(rb.Velocity.x, 0f, decel * (float)WTime.fixedDt), rb.Velocity.y);
        }
    }

    private void PawnJump(PawnInput input, int n)
    {
        if (roles == null) return;

        Role role = roles[n];
        var rb = role.rigidbody2D;
        if (rb == null) return;

        float dt = (float)WTime.fixedDt;

        float jumpStartVelocity = 16f;
        float jumpHoldForce = 8f;
        float maxHoldTime = 0.25f;
        float holdDecay = 6f;

        // 起跳
        if (input.jumpPressed && role.HasGroundContact)
        {
            role.isJumping = true;
            role.jumpHoldTime = 0f;

            rb.Velocity = new Vec2(rb.Velocity.x,jumpStartVelocity);
        }

        // 持续跳
        if (role.isJumping && input.jumpHeld)
        {
            if (role.jumpHoldTime < maxHoldTime)
            {
                float t = role.jumpHoldTime / maxHoldTime;
                float decay = MathF.Exp(-holdDecay * t);

                rb.Velocity += new Vec2(0, jumpHoldForce * decay * dt);

                role.jumpHoldTime += dt;
            }
        }

        // 松手削顶
        if (role.isJumping && !input.jumpHeld)
        {
            Vec2 v = rb.Velocity;
            if (rb.Velocity.y > 0f)
            {
                v.y *= 0.5f;
                rb.Velocity = v;
            }
            role.isJumping = false;
        }

        // 落地重置
        if (role.HasGroundContact)
        {
            role.isJumping = false;
            role.jumpHoldTime = 0f;
        }
    }
    
    
    public static float MoveTowards(float current, float target, float maxDelta)
    {
        if (maxDelta <= 0f)
            return current;

        float delta = target - current;

        if (MathF.Abs(delta) <= maxDelta)
            return target;

        return current + MathF.Sign(delta) * maxDelta;
    }
}
