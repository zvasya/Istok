using System.Runtime.CompilerServices;

namespace Istok;

/// <summary>
/// Representation of RGBA colors. Each color stored as byte value
/// </summary>
[InlineArray(4)]
public struct Color32 : IEquatable<Color32>
{
    byte item;

    /// <summary>
    /// Red component of the color
    /// </summary>
    byte R
    {
        get => this[0];
        set => this[0] = value;
    }
    /// <summary>
    /// Green component of the color
    /// </summary>
    public byte G
    {
        get => this[1];
        set => this[1] = value;
    }
    /// <summary>
    /// Blue component of the color
    /// </summary>
    public byte B
    {
        get => this[2];
        set => this[2] = value;
    }
    /// <summary>
    /// Alpha component of the color
    /// </summary>
    public byte A
    {
        get => this[3];
        set => this[3] = value;
    }

    /// <summary>
    /// Constructs a new Color32 from the given components
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    public Color32(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>
    /// Constructs a new opaque Color from the given components (a = 255)
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    public Color32(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
        A = 255;
    }

    /// <summary>
    /// Opaque White (255, 255, 255, 255)
    /// </summary>
    public static readonly Color32 White = new Color32(255, 255, 255, 255);
    /// <summary>
    /// Grey (128, 128, 128, 255)
    /// </summary>
    public static readonly Color32 Grey = new Color32(128, 128, 128, 255);
    /// <summary>
    /// Opaque Black (0, 0, 0, 255)
    /// </summary>
    public static readonly Color32 Black = new Color32(0, 0, 0, 255);
    /// <summary>
    /// Opaque Red (255, 0, 0, 255)
    /// </summary>
    public static readonly Color32 Red = new Color32(255, 0, 0, 255);
    /// <summary>
    /// Opaque Green (0, 255, 0, 255)
    /// </summary>
    public static readonly Color32 Green = new Color32(0, 255, 0, 255);
    /// <summary>
    /// Opaque Blue (0, 0, 255, 255)
    /// </summary>
    public static readonly Color32 Blue = new Color32(0, 0, 255, 255);
    /// <summary>
    /// Opaque Yellow (255, 255, 0, 255)
    /// </summary>
    public static readonly Color32 Yellow = new Color32(255, 255, 0, 255);
    /// <summary>
    /// Opaque Cyan (0, 255, 255, 255)
    /// </summary>
    public static readonly Color32 Cyan = new Color32(0, 255, 255, 255);
    /// <summary>
    /// Opaque Magenta (255, 0, 255, 255)
    /// </summary>
    public static readonly Color32 Magenta = new Color32(255, 0, 255, 255);
    /// <summary>
    /// Clear - Transparent Black (0, 0, 0, 0)
    /// </summary>
    public static readonly Color32 Clear = new Color32(0, 0, 0, 0);

    /// <summary>Returns a value that indicates whether this instance and another vector are equal</summary>
    /// <param name="other">The other Color32</param>
    /// <returns>
    /// <see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" /></returns>
    public bool Equals(Color32 other)
    {
        return Unsafe.As<Color32, uint>(ref this) == Unsafe.As<Color32, uint>(ref other);
    }
/// <summary>
/// Returns a value that indicates whether this instance and a specified object are equal
/// </summary>
/// <param name="obj">The object to compare with the current instance</param>
/// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        return obj is Color32 other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this instance
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return Unsafe.As<Color32, uint>(ref this).GetHashCode();
    }

    /// <summary>
    /// Returns the string representation of the current instance
    /// </summary>
    /// <returns>The string representation of the current instance</returns>
    public override string ToString()
    {
        return $"Color32({R}, {G}, {B}, {A})";
    }

    /// <summary>
    /// Returns a value that indicates whether each pair of elements in two specified Colors32 is equal
    /// </summary>
    /// <param name="left">The first Color32 to compare</param>
    /// <param name="right">The second Color32 to compare</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Color32 left, Color32 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Returns a value that indicates whether two specified Colors32 are not equal
    /// </summary>
    /// <param name="left">The first Color32 to compare</param>
    /// <param name="right">The second Color32 to compare</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Color32 left, Color32 right)
    {
        return !left.Equals(right);
    }
}
