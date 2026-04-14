using Mycelia;
using Mycelia.Collision.AABB;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private Tsf2 newTsf = new Tsf2(Vec2.One, Vec2.Zero, Vec2.One, 0);
    private float speed = 5f;

    public void PawnMove(in CtrlInput ctrlInput, ref WorldState state, Role controlledRole, CollisionSystem collisionSystem)
    {
        float datatime = (float)WTime.fixedDt;
        Vec2 currentPos = newTsf.postion;
        float deltaX = ctrlInput.move.x * datatime * speed;
        float deltaY = ctrlInput.move.z * datatime * speed;

        // Axis-separated movement reduces wall-sticking and allows sliding.
        Vec2 xCandidate = new Vec2(currentPos.x + deltaX, currentPos.y);
        if (!collisionSystem.WouldBeBlocked(controlledRole.collider, xCandidate))
        {
            currentPos.x = xCandidate.x;
        }

        Vec2 yCandidate = new Vec2(currentPos.x, currentPos.y + deltaY);
        if (!collisionSystem.WouldBeBlocked(controlledRole.collider, yCandidate))
        {
            currentPos.y = yCandidate.y;
        }

        newTsf.postion = currentPos;

        if (ctrlInput.move.LengthSq() > 0.001f)
        {
            newTsf.direction.x = ctrlInput.move.x;
            newTsf.direction.y = ctrlInput.move.z;
        }

        state.roleStates[0].tsf = newTsf;
        state.roleStates[0].action = ctrlInput.action;
    }
}
