using System;

namespace NSpy
{
    public struct SingleStrcuture
    {
        public SingleStrcuture(byte[] bytes)
        {
            Sign = bytes[3] & 0b10000000;
            Exponent = ((bytes[3] & 0b01111111) << 1) | bytes[2] >> 7;
            Mantissa = ((bytes[2] & 0b01111111) << 16) | bytes[1] << 8 | bytes[0];
        }

        public int Sign;
        public int Exponent;
        public int Mantissa;
        public bool IsZero => Exponent == 0 && Mantissa == 0;
        public bool IsNaN => Exponent == 0xff && Mantissa > 0;
        public bool IsInfinity => Exponent == 0xff && Mantissa == 0;

        public byte[] GetBytes()
        {
            var ret = new byte[4]
            {
                (byte)Mantissa,
                (byte)(Mantissa >> 8),
                (byte)(Exponent << 7 | Mantissa >> 16),
                (byte)(Sign << 7 | Exponent >> 1),
            };
            return ret;
        }

        public float ToSingle() => BitConverter.ToSingle(GetBytes(), 0);
        public int ToInt32() => BitConverter.ToInt32(GetBytes(), 0);
        public uint ToUInt32() => BitConverter.ToUInt32(GetBytes(), 0);

    }
}
