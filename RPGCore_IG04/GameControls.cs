using Mycelia;
using Mycelia.Collision.AABB;

namespace IGC.RPGCore_IG04;

public class GameControls
{
    private const float MoveInputEpsilonSq = 1e-6f;
    private const float FacingUpdateEpsilonSq = 0.001f;
    private readonly float speed = 5f;
    public BattleGame? battleGame;
    public Role? player0;
    
    public void PawnMove(in CtrlInput ctrlInput, ref WorldState state)
    {
        if (battleGame == null || player0 == null) return;
        
        float deltaTime = (float)WTime.fixedDt;
        Vec2 moveInput = new Vec2(ctrlInput.move.x, ctrlInput.move.z);
        float moveInputLenSq = moveInput.LengthSq();
        if (moveInputLenSq > MoveInputEpsilonSq) moveInput = moveInput.Normalized();
        
        Vec2 step = moveInput * (deltaTime * speed);
        Vec2 currentPos = battleGame.collisionSystem.PredictMoveAndSlide(player0.collider, player0.tsf.postion, step);
        player0.tsf.postion = currentPos;
        if (moveInputLenSq > FacingUpdateEpsilonSq) player0.tsf.direction = moveInput;

        player0.collider.SyncBoundsFromOwnerPosition();

        // state.roleStates[0].tsf = player0.tsf;
        // state.roleStates[0].action = ctrlInput.action;


    }
}
