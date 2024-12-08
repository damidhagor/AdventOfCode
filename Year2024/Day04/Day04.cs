namespace Year2024.Day04;

public sealed class Day04 : IDay
{
    private TextMatrix _matrixPart1 = default!;
    private TextMatrix _matrixPart2 = default!;

    public int Day => 4;

    public long DoPart1()
    {
        var input = _matrixPart1;

        var count = 0;
        for (var row = 0; row < input.Rows; row++)
        {
            for (var col = 0; col < input.Columns; col++)
            {
                count += FindXMAS(input, row, col, 0, 1);   // right
                count += FindXMAS(input, row, col, 1, 1);   // right down
                count += FindXMAS(input, row, col, -1, 0);  // down
                count += FindXMAS(input, row, col, 1, -1);  // left down
                count += FindXMAS(input, row, col, 0, -1);  // left
                count += FindXMAS(input, row, col, -1, -1); // left up
                count += FindXMAS(input, row, col, 1, 0);   // up
                count += FindXMAS(input, row, col, -1, 1);  // right up
            }
        }

        return count;
    }

    public long DoPart2()
    {
        var input = _matrixPart2;

        var count = 0;
        for (var row = 1; row < input.Rows - 1; row++)
        {
            for (var col = 1; col < input.Columns - 1; col++)
            {
                if (input.Text[row, col] == 'A'
                    && ((input.Text[row - 1, col - 1] == 'M' && input.Text[row + 1, col + 1] == 'S')
                     || (input.Text[row - 1, col - 1] == 'S' && input.Text[row + 1, col + 1] == 'M'))
                    && ((input.Text[row - 1, col + 1] == 'M' && input.Text[row + 1, col - 1] == 'S')
                     || (input.Text[row - 1, col + 1] == 'S' && input.Text[row + 1, col - 1] == 'M')))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void PrepareInput()
    {
        _matrixPart1 = TextMatrix.Parse(Inputs.Part1);
        _matrixPart2 = TextMatrix.Parse(Inputs.Part2);
    }

    private static int FindXMAS(TextMatrix input, int row, int col, int rowDirection, int columnDirection)
    {
        if ((rowDirection < 0 && row < 3)
         || (rowDirection > 0 && row > input.Rows - 4)
         || (columnDirection < 0 && col < 3)
         || (columnDirection > 0 && col > input.Columns - 4))
        {
            return 0;
        }

        if (input.Text[row, col] == 'X'
         && input.Text[row + rowDirection, col + columnDirection] == 'M'
         && input.Text[row + rowDirection * 2, col + columnDirection * 2] == 'A'
         && input.Text[row + rowDirection * 3, col + columnDirection * 3] == 'S')
        {
            return 1;
        }

        return 0;
    }
}
