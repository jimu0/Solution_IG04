namespace IGC.RPGCore_IG04;

public class PlayerController : Controller
{
    public int PlayerId { get; }
    public bool IsAutoRunning { get; private set; }

    public PlayerController(int playerId, string playerName) : base(playerName)
    {
        PlayerId = playerId;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (!Enabled)
        {
            return;
        }

        if (IsAutoRunning)
        {
            Move(0f, 1f, deltaTime);
        }
    }

    public void ToggleAutoRun()
    {
        IsAutoRunning = !IsAutoRunning;
    }

    public void UseSkill(int skillId)
    {
        if (!Enabled)
        {
            return;
        }

        // 预留：与技能系统对接（冷却、蓝耗、目标检测等）。
        _ = skillId;
    }

    public void Interact(long targetEntityId)
    {
        if (!Enabled)
        {
            return;
        }

        // 预留：与交互系统对接（NPC对话、拾取、开箱等）。
        _ = targetEntityId;
    }
}