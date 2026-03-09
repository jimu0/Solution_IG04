namespace Mycelia;

internal interface ISplineTrack
{
    double Length { get; }
    double GetSlope(double s);   // 返回 sinθ
}