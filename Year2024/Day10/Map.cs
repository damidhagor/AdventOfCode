namespace Year2024.Day10;

public sealed record Map(int[,] Positions, int Rows, int Columns)
{
    public static Map Parse(string input)
    {
        var lines = input.Split(Environment.NewLine).ToArray();

        var rows = lines.Length;
        var columns = lines[0].Length;

        var positions = new int[rows, columns];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                positions[i, j] = lines[i][j] - '0';
            }
        }

        return new(positions, rows, columns);
    }

    public int this[Position p] => Positions[p.Row, p.Column];

    public int this[int row, int column] => Positions[row, column];

    public bool IsOnMap(Position p)
        => p.Row >= 0 && p.Row < Rows && p.Column >= 0 && p.Column < Columns;
}
