namespace Year2024.Day06;

[DebuggerDisplay("({Row},{Column})")]
public readonly record struct Position(int row, int column)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public Position GetUp() => new(Row - 1, Column);

    public Position GetRight() => new(Row, Column + 1);

    public Position GetDown() => new(Row + 1, Column);

    public Position GetLeft() => new(Row, Column - 1);
}
