
using Mycelia;

namespace IGC.CardCore_IG04;

public class CardGame : ISim
{
    private GameContext Context { get; }
    public GamePhase Phase { get; private set; }

    private readonly Queue<IGameCommand> _commandQueue = new();
    private readonly SimPhase phase = SimPhase.PostStep;

    public CardGame(GameContext context)
    {
        Context = context;
        Phase = GamePhase.GameBegun;
    }

    public void Update()
    {
        switch (Phase)
        {
            case GamePhase.GameBegun:
                // 对弈开启时
                Phase = GamePhase.StartTurn;
                break;

            case GamePhase.StartTurn:
                // 新回合循环起点/新回合开始时
                Phase = GamePhase.PlayerTurn;
                break;

            case GamePhase.PlayerTurn:
                // 等待玩家命令
                HandlePlayerTurn();
                break;

            case GamePhase.TriggerEffect:
                // 效果触发阶段
                Phase = GamePhase.OffensiveSet;
                break;
            
            case GamePhase.OffensiveSet:
                // 进攻结算
                Phase = GamePhase.DefensiveSet;
                break;
            
            case GamePhase.DefensiveSet:
                // 防守结算
                Phase = GamePhase.EndTurn;
                break;
            
            case GamePhase.EndTurn:
                // 当前回合结束终点，判断是否循环下回合
                if(true) Phase = GamePhase.StartTurn;
                //else Phase = GamePhase.StartTurn;
                break;
            
            case GamePhase.GameSet:
                // 对弈结束时
                break;
        }
    }
    
    public void SubmitCommand(IGameCommand command)
    {
        if (Phase != GamePhase.PlayerTurn) 
            throw new InvalidOperationException("不在玩家回合阶段");

        _commandQueue.Enqueue(command);
    }
    
    /// <summary>
    /// 等待命令
    /// </summary>
    private void HandlePlayerTurn()
    {
        var currentPlayerId = Context.currentPlayerIndex;

        // 如果当前玩家无法行动（比如手牌为空）
        if (!CanPlayerAct(currentPlayerId))
        {
            Phase = GamePhase.TriggerEffect;
            return;
        }

        // 等待命令提交
        if (_commandQueue.Count > 0)
        {
            Phase = GamePhase.TriggerEffect;
        }
    }
    
    private bool CanPlayerAct(int currentPlayerId)
    {
        return Context.cardBoard.PlayerHands[currentPlayerId].cardIds.Count > 0;
    }

    private void ResolveCommands()
    {
        while (_commandQueue.Count > 0)
        {
            IGameCommand command = _commandQueue.Dequeue();
            command.Execute(Context);
        }
    }

    SimPhase ISim.Phase => phase;
    
    
    
    
    public void OnSimStart(in CtrlInput ctrlInput, ref WorldState state)
    {
        //throw new NotImplementedException();
    }

    public void OnSimStep(in CtrlInput ctrlInput, ref WorldState state)
    {
        //throw new NotImplementedException();
        Update();
    }
}