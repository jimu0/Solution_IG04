using System;

namespace Mycelia;

public struct Vec3 : IEquatable<Vec3>
{
    public float x;
    public float y;
    public float z;

    //构造
    public Vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // 常量
    public static Vec3 Zero => new Vec3(0f, 0f, 0f);
    public static Vec3 One => new Vec3(1f, 1f, 1f);
    public static Vec3 Right => new Vec3(1f, 0f, 0f);
    public static Vec3 Left => new Vec3(-1f, 0f, 0f);
    public static Vec3 Up => new Vec3(0f, 1f, 0f);
    public static Vec3 Down => new Vec3(0f, -1f, 0f);
    public static Vec3 Front => new Vec3(0f, 0f, 1f);
    public static Vec3 Back => new Vec3(0f, 0f, -1f);

    // ─────────────────────────────────────
    // 基础运算
    // ─────────────────────────────────────

    /// <summary>
    ///  平方和
    /// </summary>
    /// <returns>返回向量长度的平方</returns>
    public readonly float LengthSq()
    {
        return x * x + y * y + z * z;
    }

    /// <summary>
    /// 范数（模）
    /// </summary>
    /// <returns>返回向量的实际长度（模）</returns>
    public float Length()
    {
        return MathF.Sqrt(LengthSq());
    }

    /// <summary>
    /// 归一化
    /// </summary>
    /// <returns>返回单位向量</returns>
    public readonly Vec3 Normalized()
    {
        // float len = Length();
        // return len < 1e-6f ? Zero : new Vec2(x / len, y / len);

        float lenSq = LengthSq();
        if (lenSq < 1e-12f) return Zero;
        float invLen = 1.0f / MathF.Sqrt(lenSq);
        return new Vec3(x * invLen, y * invLen, z * invLen);
    }

    // ─────────────────────────────────────
    // 静态函数
    // ─────────────────────────────────────

    /// <summary>
    /// 点积
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>返回float</returns>
    public static float Dot(Vec3 a, Vec3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    /// <summary>
    /// 线性插值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns>返回Vec2</returns>
    public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
    {
        return new Vec3(
            a.x + (b.x - a.x) * t,
            a.y + (b.y - a.y) * t,
            a.z + (b.z - a.z) * t
        );
    }

    // ─────────────────────────────────────
    // 运算符重载
    // ─────────────────────────────────────

    public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vec3 operator -(Vec3 v) => new Vec3(-v.x, -v.y, -v.z);

    public static Vec3 operator *(Vec3 v, float s) => new Vec3(v.x * s, v.y * s, v.z * s);

    public static Vec3 operator *(float s, Vec3 v) => new Vec3(v.x * s, v.y * s, v.z * s);

    public static Vec3 operator /(Vec3 v, float s) => new Vec3(v.x / s, v.y / s, v.z / s);

    // ─────────────────────────────────────
    // 相等判断（值语义）
    // ─────────────────────────────────────

    /// <summary>
    /// 分量相等比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns>返回bool</returns>
    public bool Equals(Vec3 other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
    }

    /// <summary>
    /// 多态相等性比较
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>返回bool</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Vec2 other) return false;
        return MathF.Abs(x - other.x) < Const.Epsilon && MathF.Abs(y - other.y) < Const.Epsilon;
    }

    /// <summary>
    /// 哈希码生成
    /// </summary>
    /// <returns>返回int</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public static bool operator ==(Vec3 a, Vec3 b) => a.Equals(b);

    public static bool operator !=(Vec3 a, Vec3 b) => !a.Equals(b);

    // ─────────────────────────────────────
    // Debug
    // ─────────────────────────────────────

    public override string ToString()
    {
        return $"({x:0.###}, {y:0.###}, {z:0.###})";
    }
}