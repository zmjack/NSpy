using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NSpy
{
    public static class MemorySpy
    {
        public static unsafe byte[] GetBytes<T>(T instance) where T : unmanaged => GetBytes(&instance, Marshal.SizeOf(instance));
        public static unsafe byte[] GetBytes(void* pointer, int size) => new byte[size].Let(i => *((byte*)pointer + i));

        #region Memory
        public static unsafe string GetHexString<T>(T instance) where T : unmanaged => GetString(ConvertToHexString, GetBytes(instance));
        public static unsafe string GetHexString(void* pointer, int size) => GetString(ConvertToHexString, GetBytes(pointer, size));
        public static unsafe string GetHexString(byte[] bytes) => GetString(ConvertToHexString, bytes);
        public static unsafe string GetHexString<T>(T instance, int columns) where T : unmanaged => GetString(ConvertToHexString, GetBytes(instance), columns);
        public static unsafe string GetHexString(void* pointer, int size, int columns) => GetString(ConvertToHexString, GetBytes(pointer, size), columns);
        public static unsafe string GetHexString(byte[] bytes, int columns) => GetString(ConvertToHexString, bytes, columns);

        public static unsafe string GetBinaryString<T>(T instance) where T : unmanaged => GetString(ConvertToBinaryString, GetBytes(instance));
        public static unsafe string GetBinaryString(void* pointer, int size) => GetString(ConvertToBinaryString, GetBytes(pointer, size));
        public static unsafe string GetBinaryString(byte[] bytes) => GetString(ConvertToBinaryString, bytes);
        public static unsafe string GetBinaryString<T>(T instance, int columns) where T : unmanaged => GetString(ConvertToBinaryString, GetBytes(instance), columns);
        public static unsafe string GetBinaryString(void* pointer, int size, int columns) => GetString(ConvertToBinaryString, GetBytes(pointer, size), columns);
        public static unsafe string GetBinaryString(byte[] bytes, int columns) => GetString(ConvertToBinaryString, bytes, columns);
        #endregion

        #region Sequence
        public static unsafe string GetSequenceHexString<T>(T instance) where T : unmanaged => GetSequenceString(ConvertToHexString, GetBytes(instance));
        public static unsafe string GetSequenceHexString(void* pointer, int size) => GetSequenceString(ConvertToHexString, GetBytes(pointer, size));
        public static unsafe string GetSequenceHexString(byte[] bytes) => GetSequenceString(ConvertToHexString, bytes);
        public static unsafe string GetSequenceHexString<T>(T instance, int columns) where T : unmanaged => GetSequenceString(ConvertToHexString, GetBytes(instance), columns);
        public static unsafe string GetSequenceHexString(void* pointer, int size, int columns) => GetSequenceString(ConvertToHexString, GetBytes(pointer, size), columns);
        public static unsafe string GetSequenceHexString(byte[] bytes, int columns) => GetSequenceString(ConvertToHexString, bytes, columns);

        public static unsafe string GetSequenceBinaryString<T>(T instance) where T : unmanaged => GetSequenceString(ConvertToBinaryString, GetBytes(instance));
        public static unsafe string GetSequenceBinaryString(void* pointer, int size) => GetSequenceString(ConvertToBinaryString, GetBytes(pointer, size));
        public static unsafe string GetSequenceBinaryString(byte[] bytes) => GetSequenceString(ConvertToBinaryString, bytes);
        public static unsafe string GetSequenceBinaryString<T>(T instance, int columns) where T : unmanaged => GetSequenceString(ConvertToBinaryString, GetBytes(instance), columns);
        public static unsafe string GetSequenceBinaryString(void* pointer, int size, int columns) => GetSequenceString(ConvertToBinaryString, GetBytes(pointer, size), columns);
        public static unsafe string GetSequenceBinaryString(byte[] bytes, int columns) => GetSequenceString(ConvertToBinaryString, bytes, columns);
        #endregion

        private static string ConvertToHexString(byte x) => x.ToString("x2").PadLeft(2, '0');
        private static string ConvertToBinaryString(byte x) => Convert.ToString(x, 2).PadLeft(8, '0');

        private static string GetString(Func<byte, string> convert, IEnumerable<byte> bytes)
        {
            if (!bytes.Any()) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(bytes.Select(x => convert(x)).Join(" "));
            sb.Length -= Environment.NewLine.Length;
            return sb.ToString();
        }

        private static string GetString(Func<byte, string> convert, IEnumerable<byte> bytes, int columns)
        {
            if (!bytes.Any()) return string.Empty;
            if (columns <= 0) throw new ArgumentException($"The {nameof(columns)} must be greater than 0.");

            var sb = new StringBuilder();
            var groups = bytes.Select((v, i) => new { Key = i, Value = v }).GroupBy(x => x.Key / columns, x => x.Value);
            foreach (var group in groups) sb.AppendLine(group.Select(x => convert(x)).Join(" "));
            sb.Length -= Environment.NewLine.Length;
            return sb.ToString();
        }

        private static string GetSequenceString(Func<byte, string> convert, byte[] bytes) => GetString(convert, bytes.Reverse());
        private static string GetSequenceString(Func<byte, string> convert, byte[] bytes, int columns) => GetString(convert, bytes.Reverse(), columns);

    }
}
