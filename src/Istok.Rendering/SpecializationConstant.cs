using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Istok.Rendering;

/// <summary>
/// Specialization constants are a mechanism whereby constants in a SPIR-V module can have their constant value specified at the time the <see cref="Pipeline"/> is created
/// </summary>
public readonly record struct SpecializationConstant(uint ID, uint Size, SpecializationConstant.Bytes Data)
{
    public SpecializationConstant(uint id, bool value) : this(id, 4, value){}

    public SpecializationConstant(uint id, ushort value) : this(id, 2, value){}

    public SpecializationConstant(uint id, short value) : this(id, 2, value){}

    public SpecializationConstant(uint id, uint value) : this(id, 4, value){}

    public SpecializationConstant(uint id, int value) : this(id, 4, value){}

    public SpecializationConstant(uint id, ulong value) : this(id, 8, value){}

    public SpecializationConstant(uint id, long value) : this(id, 8, value){}

    public SpecializationConstant(uint id, float value) : this(id, 4, value){}

    public SpecializationConstant(uint id, double value) : this(id, 8, value){}

    [InlineArray(8)]
    public struct Bytes
    {
        byte data;

        delegate void Fill<in T>(Span<byte> a, T value);

        static Bytes Create<T>(T value, Fill<T> fill)
        {
            Bytes def = new Bytes();
            fill(def[..8], value);
            return def;
        }

        public static implicit operator Bytes(bool value) => Create(value, (span, b) => span[0] = b ? (byte)1 : (byte)0);
        public static implicit operator Bytes(short value) => Create(value, BinaryPrimitives.WriteInt16LittleEndian);
        public static implicit operator Bytes(ushort value) => Create(value, BinaryPrimitives.WriteUInt16LittleEndian);
        public static implicit operator Bytes(int value) => Create(value, BinaryPrimitives.WriteInt32LittleEndian);
        public static implicit operator Bytes(uint value) => Create(value, BinaryPrimitives.WriteUInt32LittleEndian);
        public static implicit operator Bytes(long value) => Create(value, BinaryPrimitives.WriteInt64LittleEndian);
        public static implicit operator Bytes(ulong value) => Create(value, BinaryPrimitives.WriteUInt64LittleEndian);
        public static implicit operator Bytes(float value) => Create(value, BinaryPrimitives.WriteSingleLittleEndian);
        public static implicit operator Bytes(double value) => Create(value, BinaryPrimitives.WriteDoubleLittleEndian);
    }
}
