using NStandard;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace NSpy
{
    public static class MemorySpy
    {
        public static unsafe byte[] GetBytes<T>(T instance) where T : unmanaged => GetBytes(&instance, Marshal.SizeOf(instance));
        public static unsafe byte[] GetBytes(void* pointer, int size) => new byte[size].Let(i => *((byte*)pointer + i));

        public static unsafe string[] GetHexString<T>(T instance, int columns = 0) where T : unmanaged => GetHexString(&instance, Marshal.SizeOf(instance), columns);
        public static unsafe string[] GetHexString(void* pointer, int size, int columns = 0) => GetHexString(GetBytes(pointer, size), columns);
        public static unsafe string[] GetHexString(byte[] bytes, int columns = 0)
        {
            var ret = bytes.Select(x => x.ToString("X2").PadLeft(2, '0')).Join(" ");
            if (columns > 0)
                return StringEx.SplitIntoLines(ret, 3 * columns);
            else return new[] { ret };
        }

        public static unsafe string[] GetBinaryString<T>(T instance, int columns = 0) where T : unmanaged => GetBinaryString(&instance, Marshal.SizeOf(instance), columns);
        public static unsafe string[] GetBinaryString(void* pointer, int size, int columns = 0) => GetBinaryString(GetBytes(pointer, size), columns);
        public static unsafe string[] GetBinaryString(byte[] bytes, int columns = 0)
        {
            var ret = bytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).Join(" ");
            if (columns > 0)
                return StringEx.SplitIntoLines(ret, 9 * columns);
            else return new[] { ret };
        }

    }
}
