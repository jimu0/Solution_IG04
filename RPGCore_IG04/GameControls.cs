using Mycelia;
using Mycelia.Collision.AABB;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private const float MoveInputEpsilonSq = 1e-6f;
    private const float FacingUpdateEpsilonSq = 0.001f;
    private readonly float speed = 5f;

    public void PawnMove(in CtrlInput ctrlInput, ref WorldState state, Role controlledRole, CollisionSystem collisionSystem)
    {
        float deltaTime = (float)WTime.fixedDt;
        Vec2 moveInput = new Vec2(ctrlInput.move.x, ctrlInput.move.z);
        float moveInputLenSq = moveInput.LengthSq();
        if (moveInputLenSq > MoveInputEpsilonSq)
        {
            moveInput = moveInput.Normalized();
        }

        Vec2 step = moveInput * (deltaTime * speed);
        Vec2 currentPos = controlledRole.tsf.postion;

        // Axis-separated movement reduces wall-sticking and allows sliding.
        Vec2 xCandidate = new Vec2(currentPos.x + step.x, currentPos.y);
        if (!collisionSystem.WouldBeBlocked(controlledRole.collider, xCandidate))
        {
            currentPos.x = xCandidate.x;
        }

        Vec2 yCandidate = new Vec2(currentPos.x, currentPos.y + step.y);
        if (!collisionSystem.WouldBeBlocked(controlledRole.collider, yCandidate))
        {
            currentPos.y = yCandidate.y;
        }

        Tsf2 nextTsf = controlledRole.tsf;
        nextTsf.postion = currentPos;

        if (moveInputLenSq > FacingUpdateEpsilonSq)
        {
            nextTsf.direction = moveInput;
        }

        state.roleStates[0].tsf = nextTsf;
        state.roleStates[0].action = ctrlInput.action;
    }
}
