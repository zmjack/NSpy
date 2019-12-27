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

            Assert.Equal(new[] { "0F 07 03 00" }, MemorySpy.GetHexString(color));
            Assert.Equal(new[] { "00001111 00000111 00000011 00000000" }, MemorySpy.GetBinaryString(color));
        }

    }
}
