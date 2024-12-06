namespace Year2024.Day06;

[System.Diagnostics.DebuggerDisplay("({Row},{Column})")]
public readonly ref struct Position(int row, int column)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public Position GetUp() => new(Row - 1, Column);

    public Position GetRight() => new(Row, Column + 1);

    public Position GetDown() => new(Row + 1, Column);

    public Position GetLeft() => new(Row, Column - 1);

    public static bool operator ==(Position left, Position right) => left.Row == right.Row && left.Column == right.Column;

    public static bool operator !=(Position left, Position right) => left.Row != right.Row || left.Column != right.Column;
}
