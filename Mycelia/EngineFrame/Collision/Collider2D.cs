using Mycelia.Physics;

namespace Mycelia;
public enum ColliderShape { Circle, Rectangle, Other}

public class Collider2D
{
    public UObj? owner;
    public ColliderShape shape;
    public Vec2 scale;
    public Vec2 offset;
    public float radius;
    public AABB bounds;
    public bool isCollided;

    public Action<Collider2D> OnEnter;
    public Action<Collider2D> OnStay;
    public Action<Collider2D> OnExit;
    
    public Collider2D(UObj? owner, ColliderShape shape, Vec2 scale, Vec2 offset, Action<Collider2D> onEnter, Action<Collider2D> onStay, Action<Collider2D> onExit)
    {
        this.owner = owner;
        this.shape = shape;
        this.scale = scale;
        this.offset = offset; 
        radius = scale.x > scale.y ? scale.x / 2 : scale.y / 2;
        switch (shape)
        {
            case ColliderShape.Circle:
                bounds = new AABB(offset, radius);
                break;
            case ColliderShape.Rectangle:
                bounds = new AABB(offset, scale);
                break;
            case ColliderShape.Other:
                //TODO:暂不支持其他形状
                break;
        }
    }
    
    // 碰撞事件

}

