using System;

namespace NSpy
{
    public struct DoubleStrcuture
    {
        public DoubleStrcuture(byte[] bytes)
        {
            Sign = bytes[7] & 0b10000000;
            Exponent = ((bytes[7] & 0b01111111) << 4) | ((bytes[6] & 0b10000000) >> 4);
            Mantissa = (((long)bytes[6] & 0b01111111) << 48)
                | (long)bytes[5] << 40 | (long)bytes[4] << 32 | (long)bytes[3] << 24
                | (long)bytes[2] << 16 | (long)bytes[1] << 8 | bytes[0];
        }

        public int Sign;
        public int Exponent;
        public long Mantissa;
        public bool IsZero => Exponent == 0 && Mantissa == 0;
        public bool IsNaN => Exponent == 0b11111111 && Mantissa > 0;
        public bool IsInfinity => Exponent == 0b111_11111111 && Mantissa == 0;

        public byte[] GetBytes()
        {
            var ret = new byte[8]
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
            return ret;
        }

        public double ToDouble() => BitConverter.ToDouble(GetBytes(), 0);
        public long ToInt64() => BitConverter.ToInt64(GetBytes(), 0);
        public ulong ToUInt64() => BitConverter.ToUInt64(GetBytes(), 0);

    }
}
