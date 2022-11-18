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
            const float value = 3.14f;
            var structure = new SingleStructure(MemorySpy.GetBytes(value));
            Assert.Equal(0, structure.Sign);
            Assert.Equal(128, structure.Exponent);
            Assert.Equal(4781507, structure.Mantissa);

            Assert.Equal(3.14f, structure);
        }

        [Fact]
        public void GetDoubleStructureTest()
        {
            const double value = 3.14d;
            var structure = new DoubleStructure(MemorySpy.GetBytes(value));
            Assert.Equal(0, structure.Sign);
            Assert.Equal(1024, structure.Exponent);
            Assert.Equal(2567051787601183, structure.Mantissa);

            Assert.Equal(3.14d, structure);
        }

        [Fact]
        public void ZeroTest()
        {
            var structure = new DoubleStructure(MemorySpy.GetBytes(0d));
            Assert.True(structure.IsZero);
        }

        [Fact]
        public void NaNTest()
        {
            double a = 0;
            var structure = new DoubleStructure(MemorySpy.GetBytes(a / 0));
            Assert.True(structure.IsNaN);
        }

        [Fact]
        public void PositiveInfinityTest()
        {
            double a = 1;
            var structure = new DoubleStructure(MemorySpy.GetBytes(a / 0));
            Assert.Equal(0, structure.Sign);
            Assert.True(structure.IsInfinity);
        }

        [Fact]
        public void NegativeInfinityTest()
        {
            double a = -1;
            var structure = new DoubleStructure(MemorySpy.GetBytes(a / 0));
            Assert.Equal(1, structure.Sign);
            Assert.True(structure.IsInfinity);
        }

    }
}
