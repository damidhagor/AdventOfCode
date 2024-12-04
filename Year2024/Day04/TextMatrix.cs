namespace Year2024.Day04;

public sealed record TextMatrix(char[,] Text, int Rows, int Columns)
{
    public static TextMatrix Parse(string input)
    {
        var lines = input.Split(Environment.NewLine).ToArray();

        var rows = lines.Length;
        var columns = lines[0].Length;

        var textArray = new char[rows, columns];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                textArray[i, j] = lines[i][j];
            }
        }

        return new(textArray, rows, columns);
    }
}
