using System;
using System.Collections.Generic;

namespace Mycelia;

public struct Rdm
{
    private Random rand;

    /// <summary>
    /// 使用指定种子初始化随机数生成器。
    /// 相同 seed 会产生完全一致的随机序列（适用于回放、锁步、测试）。
    /// </summary>
    public Rdm(int seed)
    {
        rand = new Random(seed);
    }

    /// <summary>
    /// 使用系统时间作为种子初始化随机数生成器。
    /// 不保证可复现（不适用于确定性模拟）。
    /// </summary>
    public Rdm()
    {
        rand = new Random();
    }

    /// <summary>
    /// 获取内部 Random 实例。
    /// 如果尚未初始化，则自动创建（使用默认种子）。
    /// </summary>
    private Random Rand => rand ??= new Random();

    /// <summary>
    /// 返回一个非负随机整数。
    /// 范围：[0, Int32.MaxValue)
    /// </summary>
    public int NextInt()
    {
        return Rand.Next();
    }

    /// <summary>
    /// 返回一个指定范围内的随机整数。
    /// 范围：[minInclusive, maxExclusive)
    /// </summary>
    /// <param name="minInclusive">最小值（包含）</param>
    /// <param name="maxExclusive">最大值（不包含）</param>
    /// <exception cref="ArgumentOutOfRangeException">当 min >= max 时抛出</exception>
    public int Range(int minInclusive, int maxExclusive)
    {
        if (minInclusive >= maxExclusive)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxExclusive),
                "maxExclusive 必须大于 minInclusive.");
        }

        return Rand.Next(minInclusive, maxExclusive);
    }

    /// <summary>
    /// 返回一个指定范围内的随机浮点数。
    /// 范围：[minInclusive, maxExclusive)
    /// </summary>
    /// <param name="minInclusive">最小值（包含）</param>
    /// <param name="maxExclusive">最大值（不包含）</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数非法（NaN / Infinity / min >= max）时抛出</exception>
    public float Range(float minInclusive, float maxExclusive)
    {
        if (float.IsNaN(minInclusive) || float.IsNaN(maxExclusive))
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxExclusive),
                "界限必须是有效的数字.");
        }

        if (float.IsInfinity(minInclusive) || float.IsInfinity(maxExclusive))
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxExclusive),
                "界限必须是有效的数字.");
        }

        if (!(maxExclusive > minInclusive))
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxExclusive),
                "maxExclusive 必须大于 minInclusive.");
        }

        var t = (float)Rand.NextDouble();
        return minInclusive + ((maxExclusive - minInclusive) * t);
    }

    /// <summary>
    /// 返回一个 [0,1) 区间的随机浮点数。
    /// 常用于概率计算、插值等。
    /// </summary>
    public float Value()
    {
        return (float)Rand.NextDouble();
    }

    /// <summary>
    /// 按给定概率返回 true 或 false。
    /// </summary>
    /// <param name="probability">概率值，范围建议为 [0,1]</param>
    public bool Chance(float probability)
    {
        return probability switch { <= 0f => false, >= 1f => true, _ => Value() < probability };
    }

    /// <summary>
    /// 随机返回 -1 或 1。
    /// 常用于方向、符号随机化。
    /// </summary>
    public int Sign()
    {
        return Rand.Next(0, 2) == 0 ? -1 : 1;
    }

    /// <summary>
    /// 在单位圆内随机生成一个点（均匀分布）。
    /// </summary>
    /// <returns>返回一个 Vec2，其长度 <= 1</returns>
    /// <remarks>
    /// 使用拒绝采样法（Rejection Sampling）。
    /// 适用于随机方向、扩散、粒子等系统。
    /// </remarks>
    public Vec2 InsideUnitCircle()
    {
        while (true)
        {
            var x = Range(-1f, 1f);
            var y = Range(-1f, 1f);

            if ((x * x) + (y * y) <= 1f)
            {
                return new Mycelia.Vec2(x, y);
            }
        }
    }

    /// <summary>
    /// 对列表进行原地随机打乱（Fisher–Yates 洗牌算法）。
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">需要打乱的列表</param>
    /// <exception cref="ArgumentNullException">当 list 为 null 时抛出</exception>
    /// <remarks>
    /// 时间复杂度 O(n)，均匀随机分布。
    /// 常用于卡牌洗牌、随机顺序执行等场景。
    /// </remarks>
    public void Shuffle<T>(IList<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Rand.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}