//纰版挒浣擄紙鏍稿績瀵硅薄锛?

namespace Mycelia.Collision.AABB;

public class Collider
{
    public AABB bounds;
    public bool isStatic; // 鏄惁闈欐€侊紙鍦板舰锛?
    public bool isTrigger;    // 鏄惁鍙Е鍙戜笉闃绘尅
    public Rigidbody2D? rigidbody2D;
    
    public object? userData;   // 瀵硅薄(鍗曚綅)

    // 纰版挒浜嬩欢
    public Action<Collider> OnEnter;
    public Action<Collider> OnStay;
    public Action<Collider> OnExit;

    public Collider(AABB aabb,bool s, bool t, ref Rigidbody2D? rig, object? userData, Action<Collider> onEnter, Action<Collider> onStay, Action<Collider> onExit)
    {
        bounds = aabb;
        isStatic = s;
        isTrigger = t;
        rigidbody2D = rig;
        this.userData = userData;
        OnEnter = onEnter;
        OnStay = onStay;
        OnExit = onExit;
        SyncBoundsFromOwnerPosition();
    }

    public void SyncBoundsFromOwnerPosition()
    {
        UObj? owner = rigidbody2D?.Owner ?? userData as UObj;
        if (owner != null)
        {
            bounds.position = owner.tsf.postion;
        }
    }
}
