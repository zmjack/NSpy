using System.Runtime.InteropServices;
using Xunit;

namespace NSpy.Test
{
    public class MemorySpyTests
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Color
        {
            [FieldOffset(2)] public byte Red;
            [FieldOffset(1)] public byte Green;
            [FieldOffset(0)] public byte Blue;
            [FieldOffset(0)] public int RGB;
        }

        [Fact]
        public void ExplicitStructLayoutTest()
        {
            var color = new Color
            {
                Red = 3,
                Green = 7,
                Blue = 15,
            };

            Assert.Equal(0x03070f, color.RGB);
            Assert.Equal(4, Marshal.SizeOf<Color>());

            Assert.Equal("0f 07 03 00", MemorySpy.GetHexString(color));
            Assert.Equal("00001111 00000111 00000011 00000000", MemorySpy.GetBinaryString(color));
        }

        [Fact]
        public void GetSingleStructureTest()
        {
            const float value = 3.14F;
            var structure = new SingleStrcuture(MemorySpy.GetBytes(value));
            Assert.Equal(0, structure.Sign);
            Assert.Equal(128, structure.Exponent);
            Assert.Equal(4781507, structure.Mantissa);

            Assert.Equal(3.14F, structure.ToSingle());
            Assert.Equal(1078523331, structure.ToInt32());
            Assert.Equal(1078523331U, structure.ToUInt32());
        }

        [Fact]
        public void GetDoubleStructureTest()
        {
            const double value = 3.14D;
            var structure = new DoubleStrcuture(MemorySpy.GetBytes(value));
            Assert.Equal(0, structure.Sign);
            Assert.Equal(1024, structure.Exponent);
            Assert.Equal(2567051787601183, structure.Mantissa);

            Assert.Equal(3.14D, structure.ToDouble());
            Assert.Equal(4614253070214989087L, structure.ToInt64());
            Assert.Equal(4614253070214989087UL, structure.ToUInt64());
        }

        [Fact]
        public void NaNTest()
        {
            double a = 1;
            var structure = new DoubleStrcuture(MemorySpy.GetBytes(a / 0));
            Assert.True(structure.IsNaN);
        }

    }
}
