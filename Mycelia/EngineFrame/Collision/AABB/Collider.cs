//碰撞体（核心对象）

namespace Mycelia.Collision.AABB;

public class Collider
{
    public AABB bounds;
    public bool isStatic;     // 是否静态（地形）
    public bool isTrigger;    // 是否只触发不阻挡
    public Rigidbody2D? rigidbody2D;
    
    public object? userData;   // 对象(单位)

    // 碰撞事件
    public Action<Collider> OnEnter;
    public Action<Collider> OnStay;
    public Action<Collider> OnExit;

    public Collider(AABB aabb, ref Rigidbody2D rig, object? userData, Action<Collider> onEnter, Action<Collider> onStay, Action<Collider> onExit, bool s = false, bool t = false)
    {
        bounds = aabb;
        rigidbody2D = rig;
        this.userData = userData;
        OnEnter = onEnter;
        OnStay = onStay;
        OnExit = onExit;
        isStatic = s;
        isTrigger = t;
    }
}