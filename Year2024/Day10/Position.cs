namespace Year2024.Day10;

public readonly struct Position(int row, int column)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public Position Up => new(Row - 1, Column);

    public Position Right => new(Row, Column + 1);

    public Position Down => new(Row + 1, Column);

    public Position Left => new(Row, Column - 1);
}