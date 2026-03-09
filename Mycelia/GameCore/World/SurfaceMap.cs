namespace Mycelia;

internal class SurfaceMap
{
    private readonly SurfaceSegment[] segments;
    private int cachedIndex = 0;

    internal SurfaceMap(SurfaceSegment[] segs)
    {
        segments = segs;
    }

    internal SurfaceSegment Get(double s)
    {
        if (segments.Length == 0) return default;

        if (cachedIndex < segments.Length - 1 && s >= segments[cachedIndex + 1].startS) cachedIndex++;
        else if (cachedIndex > 0 && s < segments[cachedIndex].startS) cachedIndex--;

        return segments[cachedIndex];
    }
}