namespace Mycelia;

public struct SimTime(double t)
{
    public double t = t;

    public static SimTime operator +(SimTime time, double dt) => new(time.t + dt);
}