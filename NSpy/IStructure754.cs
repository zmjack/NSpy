namespace NSpy
{
    public interface IStructure754
    {
        int Sign { get; }
        int Exponent { get; }
        long Mantissa { get; }

        bool IsInfinity { get; }
        bool IsNaN { get; }
        bool IsZero { get; }

        byte[] GetBytes();

        bool Equals(object obj);
        int GetHashCode();
    }

    public interface IStructure754<TValue> : IStructure754 where TValue : struct
    {
        TValue Value { get; }
    }
}