
namespace Mycelia;

public partial class Unit : Un
{
    internal Vec3 position = Vec3.Back;
    internal Vec3 orientation = Vec3.Front;
    internal Vec3 scale = Vec3.One;
    //public Tsf2 tsf = Tsf2.Zero;// = Tsf2.Zero
    internal Unit(int id = 0)
    {
        this.id = id;
        position = Vec3.Back;
        orientation = Vec3.Front;
        scale = Vec3.One;
    }


}

