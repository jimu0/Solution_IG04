using System;
using System.Collections.Generic;

namespace Mycelia;

public class TrajectoryBuffer
{
    private readonly Queue<TrajectorySample> samples = new();
    public IReadOnlyCollection<TrajectorySample> Samples => samples;
    
    public void AddSample(SimTime time, Vec2 position)
    {
        samples.Enqueue(new TrajectorySample(time, position));

        // 后面可以限制长度
    }

    public bool TryGetPositionAt(SimTime time, out Vec2 position)
    {
        position = Vec2.Zero;
        if (samples.Count == 0) return false;

        TrajectorySample? prev = null;
        foreach (var sample in samples)
        {
            if (sample.time.t >= time.t)
            {
                if (prev == null)
                {
                    position = sample.position;
                    return true;
                }

                double t0 = prev.time.t;
                double t1 = sample.time.t;
                if (Math.Abs(t1 - t0) < 1e-12)
                {
                    position = sample.position;
                    return true;
                }

                double alpha = (time.t - t0) / (t1 - t0);
                position = Vec2.Lerp(prev.position, sample.position, (float)alpha);
                return true;
            }

            prev = sample;
        }

        if (prev == null) return false;
        position = prev.position;
        return true;
    }

    // 之后我们会在这里查“光锥位置�?
}
