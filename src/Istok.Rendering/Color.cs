using System.Numerics;

namespace Istok;

/// <summary>
/// Representation of RGBA colors. Each color stored in 32-bit floating-point value
/// </summary>
public struct Color : IEquatable<Color>
{
    Vector4 _channels;

    /// <summary>
    /// Red component of the color
    /// </summary>
    public float R => _channels.X;
    /// <summary>
    /// Green component of the color
    /// </summary>
    public float G => _channels.Y;
    /// <summary>
    /// Blue component of the color
    /// </summary>
    public float B => _channels.Z;
    /// <summary>
    /// Alpha component of the color
    /// </summary>
    public float A => _channels.W;


    /// <summary>
    /// Constructs a new Color from the given components
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    public Color(float r, float g, float b, float a)
    {
        _channels = new Vector4(r, g, b, a);
    }

    /// <summary>
    /// Constructs a new opaque Color from the given components (a = 1)
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    public Color(float r, float g, float b)
    {
        _channels = new Vector4(r, g, b, 1);
    }

    /// <summary>
    /// Constructs a new Color from the XYZW components of a vector
    /// </summary>
    /// <param name="channels">The vector containing the color components</param>
    public Color(Vector4 channels)
    {
        _channels = channels;
    }

    /// <summary>
    /// Opaque White (1.0, 1.0, 1.0, 1.0)
    /// </summary>
    public static readonly Color White = new Color(1, 1, 1, 1);
    /// <summary>
    /// Opaque Grey (0.5, 0.5, 0.5, 1)
    /// </summary>
    public static readonly Color Grey = new Color(.5f, .5f, .5f, 1);
    /// <summary>
    /// Opaque Black (0.0, 0.0, 0.0, 1.0)
    /// </summary>
    public static readonly Color Black = new Color(0, 0, 0, 1);
    /// <summary>
    /// Opaque red (1.0, 0.0, 0.0, 1.0)
    /// </summary>
    public static readonly Color Red = new Color(1, 0, 0, 1);
    /// <summary>
    /// Opaque Green (0.0, 1.0, 0.0, 1.0)
    /// </summary>
    public static readonly Color Green = new Color(0, 1, 0, 1);
    /// <summary>
    /// Opaque Blue (0.0, 0.0, 1.0, 1.0)
    /// </summary>
    public static readonly Color Blue = new Color(0, 0, 1, 1);
    /// <summary>
    /// Opaque Yellow (1.0, 1.0, 0.0, 1.0)
    /// </summary>
    public static readonly Color Yellow = new Color(1, 1, 0, 1);
    /// <summary>
    /// Opaque Cyan (0.0, 1.0, 1.0, 1.0)
    /// </summary>
    public static readonly Color Cyan = new Color(0, 1, 1, 1);
    /// <summary>
    /// Opaque Magenta (1.0, 0.0, 1.0, 1.0)
    /// </summary>
    public static readonly Color Magenta = new Color(1, 0, 1, 1);
    /// <summary>
    /// Clear - Transparent Black (0.0, 0.0, 0.0, 0.0)
    /// </summary>
    public static readonly Color Clear = new Color(0, 0, 0, 0);

    /// <summary>
    /// Gets or sets the element at the specified index
    /// </summary>
    /// <param name="index">The index of the element to get or set</param>
    public float this[int index]
    {
        readonly get => _channels[index];
        set => _channels[index] = value;
    }

    public static implicit operator Vector4(Color c) => new Vector4(c.R, c.G, c.B, c.A);

    public static implicit operator Color(Vector4 v) => new Color(v);

    /// <summary>Returns a value that indicates whether this instance and another vector are equal</summary>
    /// <param name="other">The other Color</param>
    /// <returns>
    /// <see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" /></returns>
    public bool Equals(Color other)
    {
        return _channels.Equals(other._channels);
    }
    /// <summary>
    /// Returns a value that indicates whether this instance and a specified object are equal
    /// </summary>
    /// <param name="obj">The object to compare with the current instance</param>
    /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        return obj is Color other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this instance
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B, A);
    }

    /// <summary>
    /// Returns the string representation of the current instance
    /// </summary>
    /// <returns>The string representation of the current instance</returns>
    public override string ToString()
    {
        return $"Color({R}, {G}, {B}, {A})";
    }

    public Silk.NET.Vulkan.ClearColorValue ToClearColorValue()
    {
        return new Silk.NET.Vulkan.ClearColorValue(R, G, B, A);
    }

    /// <summary>
    /// Returns a value that indicates whether each pair of elements in two specified Colors is equal
    /// </summary>
    /// <param name="left">The first Color to compare</param>
    /// <param name="right">The second Color to compare</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Returns a value that indicates whether two specified Colors are not equal
    /// </summary>
    /// <param name="left">The first Color to compare</param>
    /// <param name="right">The second Color to compare</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Color left, Color right)
    {
        return !left.Equals(right);
    }
}
