using Mycelia;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private const float MoveInputEpsilonSq = 1e-6f;
    private const float FacingUpdateEpsilonSq = 0.001f;
    private readonly float speed = 5f;
    private BattleGame? battleGame => RPGMode.battleGame;
    public Role[]? roles;
    //public Role? player0;

    public void Behavior(CtrlInput ctrlInput)
    {
        if (battleGame == null || roles == null) return;
        for (int n = 0; n < ctrlInput.NumberOfPlayers; n++)
        {
            //Role t = roles[n];
            PawnMove(ctrlInput.pawnInputs[n], n);
            PawnJump(ctrlInput.pawnInputs[n], n);
        }
    }


    private void PawnMove(PawnInput pawnInput, int n)
    {
        if (roles == null) return;
        Role role = roles[n];
        //PawnInput roleInput = ctrlInput.pawnInputs[n];
        // if (n < ctrlInput.NumberOfPlayers) roleInput = ctrlInput.pawnInputs[n];
        // else return;//roleInput = new PawnInput();//TODO:其他单位待处理
        
        float deltaTime = (float)WTime.fixedDt;
        Vec2 moveInput = new(pawnInput.move.x, pawnInput.move.z);
        float moveInputLenSq = moveInput.LengthSq();
        if (moveInputLenSq > MoveInputEpsilonSq) moveInput = moveInput.Normalized();
        
        Vec2 step = moveInput * (deltaTime * speed);
        if (battleGame != null)
        {
            Vec2 currentPos = battleGame.collisionSystem.PredictMoveAndSlide(role.collider, role.tsf.postion, step);
            role.tsf.postion = currentPos;
        }

        if (moveInputLenSq > FacingUpdateEpsilonSq) role.tsf.direction = moveInput;

        role.collider.SyncBoundsFromOwnerPosition();
    }

    private void PawnJump(PawnInput pawnInput, int n)
    {
        if (roles == null) return;
        Role role = roles[n];
        //PawnInput roleInput = ctrlInput.pawnInputs[n];
        
        float dt = (float)WTime.fixedDt;
        //ref var pawn = ref state.roleStates; // 假设你这么取
        // === 参数（可以调手感） ===
        float jumpStartVelocity = 8f;     // 起跳瞬间的速度
        float jumpHoldForce = 8f;       // 提供力
        float maxHoldTime = 0.25f;        // 最长“蓄力时间”
        float holdDecay = 6f;             // 衰减速度（越大越快变弱）
        
        var rb = role.rigidbody2D;
        if (rb == null) return;
        // === 起跳（只触发一次） ===
        if (pawnInput.jumpPressed && role.isGrounded)
        {
            role.isJumping = true;
            role.jumpHoldTime = 0f;

            role.isGrounded = false;

            // 给一个瞬时向上速度
            rb.Velocity.y = jumpStartVelocity;
        }

        // === 按住跳跃 → 持续施力（逐渐减弱） ===
        if (role.isJumping && pawnInput.jumpHeld)
        {
            if (role.jumpHoldTime < maxHoldTime)
            {
                // 0 → 1
                float t = role.jumpHoldTime / maxHoldTime;

                // 衰减函数（指数 or 平滑都可以）
                float decay = MathF.Exp(-holdDecay * t);

                float force = jumpHoldForce * decay;

                rb.Velocity.y += force * dt;

                role.jumpHoldTime += dt;
            }
        }

        // === 提前松开 → 立刻削减上升速度（关键手感点） ===
        if (role.isJumping && !pawnInput.jumpPressed)
        {
            if (rb.Velocity.y > 0f)
            {
                rb.Velocity.y *= 0.5f; // 或者直接 clamp
            }

            role.isJumping = false;
        }

        // === 落地重置 ===
        if (role.isGrounded)
        {
            role.isJumping = false;
            role.jumpHoldTime = 0f;
        }
        
        role.collider.SyncBoundsFromOwnerPosition();
    }
}
