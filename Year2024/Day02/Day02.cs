namespace Year2024.Day02;

public sealed class Day02 : IDay
{
    private int[][] _part1Lines = default!;
    private int[][] _part2Lines = default!;

    public int Day => 2;

    public long DoPart1()
    {
        var safeReports = 0;
        foreach (var line in _part1Lines)
        {
            var isReportSafe = IsReportSafe(line, null);

            if (isReportSafe)
            {
                safeReports++;
            }
        }

        return safeReports;
    }

    public long DoPart2()
    {
        var safeReports = 0;
        foreach (var line in _part2Lines)
        {
            if (IsReportSafe(line, null))
            {
                safeReports++;
                continue;
            }

            for (var i = 0; i < line.Length; i++)
            {
                var isReportSafe = IsReportSafe(line, i);

                if (isReportSafe)
                {
                    safeReports++;
                    break;
                }
            }
        }

        return safeReports;
    }

    public void PrepareInput()
    {
        _part1Lines = Inputs.Part1
            .Split(Environment.NewLine)
            .Select(line => line
                .Split(' ')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        _part2Lines = Inputs.Part2
            .Split(Environment.NewLine)
            .Select(line => line
                .Split(' ')
                .Select(int.Parse)
                .ToArray())
            .ToArray();
    }

    private static bool IsReportSafe(int[] numbers, int? skipIndex)
    {
        int? lastNumber = null;
        int? firstDifference = null;

        for (var i = 0; i < numbers.Length; i++)
        {
            if (i == skipIndex)
            {
                continue;
            }

            var number = numbers[i];

            if (lastNumber is null)
            {
                lastNumber = number;
                continue;
            }

            var difference = lastNumber.Value - number;
            if (Math.Abs(difference) is < 1 or > 3
                || firstDifference is not null && firstDifference < 0 != difference < 0)
            {
                return false;
            }

            lastNumber = number;
            firstDifference ??= difference;
        }

        return true;
    }
}
