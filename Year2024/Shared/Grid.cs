namespace Year2024.Shared;

public sealed class Grid<T>(T[,] positions)
{
    private readonly T[,] _positions = positions;

    public int Rows { get; } = positions.GetLength(0);

    public int Columns { get; } = positions.GetLength(1);

    public static Grid<T> Parse(string input, Func<char, T> convert)
    {
        var lines = input.Split(Environment.NewLine).ToArray();

        var rows = lines.Length;
        var columns = lines[0].Length;

        var positions = new T[rows, columns];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                positions[i, j] = convert(lines[i][j]);
            }
        }

        return new(positions);
    }

    public T this[Position p]
    {
        get => _positions[p.Row, p.Column];
        set => _positions[p.Row, p.Column] = value;
    }

    public T this[int row, int column]
    {
        get => _positions[row, column];
        set => _positions[row, column] = value;
    }

    public bool IsInBounds(Position p)
        => p.Row >= 0 && p.Row < Rows && p.Column >= 0 && p.Column < Columns;
}
