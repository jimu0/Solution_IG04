namespace IGC.RPGCore_IG04;

public class Controller
{
    public string Name { get; protected set; }
    public bool Enabled { get; private set; } = true;

    protected float MoveSpeed { get; set; } = 5.0f;
    protected float RotationSpeed { get; set; } = 180.0f;

    protected Controller(string name)
    {
        Name = name;
    }

    public virtual void Update(float deltaTime)
    {
        if (!Enabled)
        {
            return;
        }
    }

    public virtual void Move(float horizontal, float vertical, float deltaTime)
    {
        if (!Enabled)
        {
            return;
        }

        // 在具体引擎层里，这里通常会把输入转换为世界/本地方向位移。
        _ = horizontal * MoveSpeed * deltaTime;
        _ = vertical * MoveSpeed * deltaTime;
    }

    public virtual void Rotate(float yawInput, float deltaTime)
    {
        if (!Enabled)
        {
            return;
        }

        _ = yawInput * RotationSpeed * deltaTime;
    }

    public virtual void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}


