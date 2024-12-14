namespace Year2024.Day14;

[DebuggerDisplay("({X}, {Y})")]
public readonly struct Coordinate(long x, long y)
{
    public long X { get; } = x;

    public long Y { get; } = y;
}
