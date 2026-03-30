namespace Mycelia;

public class cameraStand
{
    public Vec3 follow;
    public Vec3 lookAt;

    public cameraStand()
    {
        follow = Vec3.Back;
        lookAt = Vec3.Zero;
    }
    public cameraStand(Vec3 follow, Vec3 lookAt)
    {
        this.follow = follow;
        this.lookAt = lookAt;
    }

    public void SetValue(Vec3 pos0,Vec3 pos1)
    {
        follow = pos0;
        lookAt = pos1;
    }

    public void SetCameraStandState(cameraStand stand,ref WorldState state)
    {
        state.cameraStand = stand;
    }
}