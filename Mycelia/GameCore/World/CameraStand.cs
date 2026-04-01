namespace Mycelia;

public class CameraStand
{
    public Vec3 follow;
    public Vec3 lookAt;

    public CameraStand()
    {
        follow = Vec3.Back;
        lookAt = Vec3.Zero;
    }
    public CameraStand(Vec3 follow, Vec3 lookAt)
    {
        this.follow = follow;
        this.lookAt = lookAt;
    }

    public void SetValue(Vec3 pos0,Vec3 pos1)
    {
        follow = pos0;
        lookAt = pos1;
    }

    public void SetCameraStandState(CameraStand stand,ref WorldState state)
    {
        state.cameraStand = stand;
    }
}