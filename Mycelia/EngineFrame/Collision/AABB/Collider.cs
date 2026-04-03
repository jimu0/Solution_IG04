//碰撞体（核心对象）

namespace Mycelia.Collision.AABB;

public class Collider
{
    public AABB bounds;
    public bool isStatic;     // 是否静态（地形）
    public bool isTrigger;    // 是否只触发不阻挡

    public object userData;   // 对象(单位)

    // 碰撞事件
    public Action<Collider> OnEnter;
    public Action<Collider> OnStay;
    public Action<Collider> OnExit;
}