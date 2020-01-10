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

        public static unsafe string GetHexString<T>(T instance) where T : unmanaged => GetHexStrings(instance, 0)[0];
        public static unsafe string GetHexString(void* pointer, int size) => GetHexStrings(pointer, size, 0)[0];
        public static unsafe string GetHexString(byte[] bytes) => GetHexStrings(bytes, 0)[0];
        public static unsafe string[] GetHexStrings<T>(T instance, int columns) where T : unmanaged => GetHexStrings(&instance, Marshal.SizeOf(instance), columns);
        public static unsafe string[] GetHexStrings(void* pointer, int size, int columns) => GetHexStrings(GetBytes(pointer, size), columns);
        public static unsafe string[] GetHexStrings(byte[] bytes, int columns)
        {
            var ret = bytes.Select(x => x.ToString("x2").PadLeft(2, '0')).Join(" ");
            if (columns > 0)
                return StringEx.SplitIntoLines(ret, 3 * columns);
            else return new[] { ret };
        }

        public static unsafe string GetBinaryString<T>(T instance) where T : unmanaged => GetBinaryStrings(instance, 0)[0];
        public static unsafe string GetBinaryString(void* pointer, int size) => GetBinaryStrings(pointer, size, 0)[0];
        public static unsafe string GetBinaryString(byte[] bytes) => GetBinaryStrings(bytes, 0)[0];
        public static unsafe string[] GetBinaryStrings<T>(T instance, int columns = 0) where T : unmanaged => GetBinaryStrings(&instance, Marshal.SizeOf(instance), columns);
        public static unsafe string[] GetBinaryStrings(void* pointer, int size, int columns = 0) => GetBinaryStrings(GetBytes(pointer, size), columns);
        public static unsafe string[] GetBinaryStrings(byte[] bytes, int columns = 0)
        {
            var ret = bytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).Join(" ");
            if (columns > 0)
                return StringEx.SplitIntoLines(ret, 9 * columns);
            else return new[] { ret };
        }

    }
}
