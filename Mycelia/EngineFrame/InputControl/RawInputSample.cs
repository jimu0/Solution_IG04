
namespace Mycelia;

public struct RawInputSample
{
    public float startTime;          // Time.time or unscaledTime
    public float time;           // 连续轴
    public bool keyDown;     // 这一帧是否按下
    public bool keyHeld;     // 当前是否按住（可选）
}