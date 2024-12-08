namespace Year2024.Day08;

public readonly record struct Position(int row, int column)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Column + b.Column);

    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Column - b.Column);
}