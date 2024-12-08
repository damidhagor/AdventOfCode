namespace Year2024.Day08;

public sealed record Map(char[,] Positions, int Rows, int Columns)
{
    public static Map Parse(string input)
    {
        var lines = input.Split(Environment.NewLine).ToArray();

        var rows = lines.Length;
        var columns = lines[0].Length;

        var positions = new char[rows, columns];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                positions[i, j] = lines[i][j];
            }
        }

        return new(positions, rows, columns);
    }

    public char this[int row, int column] => Positions[row, column];

    public bool IsOnMap(Position position)
        => position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;
}
