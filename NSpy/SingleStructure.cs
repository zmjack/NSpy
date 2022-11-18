using System;

namespace NSpy
{
    public readonly struct SingleStructure : IStructure754<float>
    {
        private readonly float value;

        public SingleStructure(float value) : this(BitConverter.GetBytes(value)) { }
        public SingleStructure(byte[] bytes)
        {
            if (bytes.Length != 4) throw new ArgumentException("The length of bytes must be 4.", nameof(bytes));

            Sign = bytes[3] >> 7;
            Exponent =
                (bytes[3] & 0b01111111) << 1
                | bytes[2] >> 7;
            Mantissa =
                (bytes[2] & 0b01111111) << 16
                | bytes[1] << 8
                | bytes[0];
            value = BitConverter.ToSingle(bytes, 0);
        }

        public SingleStructure(int sign, int exponent, long mantissa)
        {
            Sign = sign & 0b00000001;
            Exponent = exponent & 0b11111111;
            Mantissa = mantissa & 0b01111111_11111111_11111111;
            value = BitConverter.ToSingle(GetBytes(), 0);
        }

        public int Sign { get; }
        public int Exponent { get; }
        public long Mantissa { get; }

        public bool IsInfinity => Exponent == 0b11111111 && Mantissa == 0;
        public bool IsNaN => Exponent == 0b11111111 && Mantissa > 0;
        public bool IsZero => Exponent == 0 && Mantissa == 0;

        public float Value => value;
        public byte[] GetBytes()
        {
            return new byte[4]
            {
                (byte)Mantissa,
                (byte)(Mantissa >> 8),
                (byte)((long)Exponent << 7 | Mantissa >> 16),
                (byte)(Sign << 7 | Exponent >> 1),
            };
        }

        public override int GetHashCode() => value.GetHashCode();

        public static implicit operator float(SingleStructure structure)
        {
            return structure.value;
        }
    }
}
