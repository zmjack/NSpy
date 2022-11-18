using System;

namespace NSpy
{
    public readonly struct DoubleStructure : IStructure754<double>
    {
        private readonly double value;

        public DoubleStructure(double value) : this(BitConverter.GetBytes(value)) { }
        public DoubleStructure(byte[] bytes)
        {
            if (bytes.Length != 8) throw new ArgumentException("The length of bytes must be 8.", nameof(bytes));

            Sign = bytes[7] >> 7;
            Exponent =
                (bytes[7] & 0b01111111) << 4
                | bytes[6] >> 4;
            Mantissa =
                ((long)bytes[6] & 0b00001111) << 48
                | (long)bytes[5] << 40
                | (long)bytes[4] << 32
                | (long)bytes[3] << 24
                | (long)bytes[2] << 16
                | (long)bytes[1] << 8
                | bytes[0];
            value = BitConverter.ToDouble(bytes, 0);
        }

        public DoubleStructure(int sign, int exponent, long mantissa)
        {
            Sign = sign & 0b00000001;
            Exponent = exponent & 0b00000111_11111111;
            Mantissa = mantissa & 0b00001111_11111111_11111111_11111111_11111111_11111111_11111111;
            value = BitConverter.ToDouble(GetBytes(), 0);
        }

        public int Sign { get; }
        public int Exponent { get; }
        public long Mantissa { get; }

        public bool IsInfinity => Exponent == 0b111_11111111 && Mantissa == 0;
        public bool IsNaN => Exponent == 0b111_11111111 && Mantissa > 0;
        public bool IsZero => Exponent == 0 && Mantissa == 0;

        public double Value => value;
        public byte[] GetBytes()
        {
            return new byte[8]
            {
                (byte)Mantissa,
                (byte)(Mantissa >> 8),
                (byte)(Mantissa >> 16),
                (byte)(Mantissa >> 24),
                (byte)(Mantissa >> 32),
                (byte)(Mantissa >> 40),
                (byte)((long)Exponent << 4 | Mantissa >> 48),
                (byte)(Sign << 7 | Exponent >> 4),
            };
        }

        public override int GetHashCode() => value.GetHashCode();

        public static implicit operator double(DoubleStructure structure)
        {
            return structure.value;
        }
    }
}
