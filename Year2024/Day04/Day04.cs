namespace Year2024.Day04;

public sealed class Day04 : IDay
{
    private Grid<char> _gridPart1 = default!;
    private Grid<char> _gridPart2 = default!;

    public int Day => 4;

    public string DoPart1()
    {
        var input = _gridPart1;

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

        return count.ToString();
    }

    public string DoPart2()
    {
        var input = _gridPart2;

        var count = 0;
        for (var row = 1; row < input.Rows - 1; row++)
        {
            for (var col = 1; col < input.Columns - 1; col++)
            {
                if (input[row, col] == 'A'
                    && ((input[row - 1, col - 1] == 'M' && input[row + 1, col + 1] == 'S')
                     || (input[row - 1, col - 1] == 'S' && input[row + 1, col + 1] == 'M'))
                    && ((input[row - 1, col + 1] == 'M' && input[row + 1, col - 1] == 'S')
                     || (input[row - 1, col + 1] == 'S' && input[row + 1, col - 1] == 'M')))
                {
                    count++;
                }
            }
        }

        return count.ToString();
    }

    public void PrepareInput()
    {
        _gridPart1 = Grid<char>.Parse(Inputs.Part1, c => c);
        _gridPart2 = Grid<char>.Parse(Inputs.Part2, c => c);
    }

    private static int FindXMAS(Grid<char> input, int row, int col, int rowDirection, int columnDirection)
    {
        if ((rowDirection < 0 && row < 3)
         || (rowDirection > 0 && row > input.Rows - 4)
         || (columnDirection < 0 && col < 3)
         || (columnDirection > 0 && col > input.Columns - 4))
        {
            return 0;
        }

        if (input[row, col] == 'X'
         && input[row + rowDirection, col + columnDirection] == 'M'
         && input[row + rowDirection * 2, col + columnDirection * 2] == 'A'
         && input[row + rowDirection * 3, col + columnDirection * 3] == 'S')
        {
            return 1;
        }

        return 0;
    }
}
