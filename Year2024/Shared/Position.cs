namespace Year2024.Shared;

[DebuggerDisplay("({Row},{Column})")]
public readonly record struct Position(int Row, int Column)
{
    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Column + b.Column);

    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Column - b.Column);

    public Position Up => new(Row - 1, Column);

    public Position Right => new(Row, Column + 1);

    public Position Down => new(Row + 1, Column);

    public Position Left => new(Row, Column - 1);

    public override string ToString() => $"({Row},{Column})";
}
