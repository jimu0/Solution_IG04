using System;

namespace Mycelia;

public struct Vec2Double : IEquatable<Vec2Double>
{


    public double x;
    public double y;

    public Vec2Double(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    // 常量
    public static Vec2Double Zero => new(0f, 0f);
    public static Vec2Double One => new(1f, 1f);
    public static Vec2Double Right => new(1f, 0f);
    public static Vec2Double Left => new(-1f, 0f);
    public static Vec2Double Up => new(0f, 1f);
    public static Vec2Double Down => new(0f, -1f);
    
    // ─────────────────────────────────────
    // 基础运算
    // ─────────────────────────────────────
    
    /// <summary>
    ///  平方和
    /// </summary>
    /// <returns>返回向量长度的平方</returns>
    public double LengthSq() { return x * x + y * y; }

    /// <summary>
    /// 范数（模）
    /// </summary>
    /// <returns>返回向量的实际长度（模）</returns>
    public double Length() => System.Math.Sqrt(LengthSq());
    
    /// <summary>
    /// 归一化
    /// </summary>
    /// <returns>返回单位向量</returns>
    public Vec2Double Normalized()
    {
        double len = Length();
        return len < 1e-12 ? Zero : new Vec2Double(x / len, y / len);
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
    public static double Dot(Vec2Double a, Vec2Double b)
    {
        return a.x * b.x + a.y * b.y;
    }
    
    /// <summary>
    /// 线性插值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns>返回Vec2</returns>
    public static Vec2Double Lerp(Vec2Double a, Vec2Double b, double t)
    {
        return new Vec2Double(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
    }
    
    // ─────────────────────────────────────
    // 运算符重载
    // ─────────────────────────────────────
    
    public static Vec2Double operator +(Vec2Double a, Vec2Double b) => new(a.x + b.x, a.y + b.y);
    public static Vec2Double operator -(Vec2Double a, Vec2Double b) => new(a.x - b.x, a.y - b.y);
    public static Vec2Double operator -(Vec2Double v) => new(-v.x, -v.y);
    public static Vec2Double operator *(Vec2Double v, double s) => new(v.x * s, v.y * s);
    public static Vec2Double operator *(double s, Vec2Double v) => new(v.x * s, v.y * s);
    public static Vec2Double operator /(Vec2Double v, double s) => new(v.x / s, v.y / s);
    
    // ─────────────────────────────────────
    // 相等判断（值语义）
    // ─────────────────────────────────────
    
    /// <summary>
    /// 分量相等比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns>返回bool</returns>
    public bool Equals(Vec2Double other)
    {
        return x.Equals(other.x) && y.Equals(other.y);
    }
    public override bool Equals(object? obj)
    {
        return obj is Vec2Double other && Equals(other);
    }
    // /// <summary>
    // /// 多态相等性比较
    // /// </summary>
    // /// <param name="obj"></param>
    // /// <returns>返回bool</returns>
    // public override bool Equals(object? obj)
    // {
    //     if (obj is not Vec3 other) return false;
    //     return MathF.Abs(x - other.x) < Const.Epsilon && MathF.Abs(y - other.y) < Const.Epsilon;
    // }
    
    /// <summary>
    /// 哈希码生成
    /// </summary>
    /// <returns>返回int</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public static bool operator ==(Vec2Double a, Vec2Double b) => a.Equals(b);
    public static bool operator !=(Vec2Double a, Vec2Double b) => !a.Equals(b);
    
    // ─────────────────────────────────────
    // Debug
    // ─────────────────────────────────────

    public override string ToString()
    {
        return $"({x:0.###}, {y:0.###})";
    }
}