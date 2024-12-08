namespace Year2024.Day02;

public sealed class Day02 : IDay
{
    private string[] _part1Lines = default!;
    private string[] _part2Lines = default!;

    public int Day => 2;

    public long DoPart1()
    {
        var safeReports = 0;
        foreach (var line in _part1Lines)
        {
            var isReportSafe = IsReportSafe(line.Split(' ').Select(int.Parse));

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
            var numbers = line.Split(' ').Select(int.Parse).ToArray();

            if (IsReportSafe(numbers))
            {
                safeReports++;
                continue;
            }

            for (var i = 0; i < numbers.Length; i++)
            {
                var isReportSafe = IsReportSafe(
                    numbers
                        .Index()
                        .Where(x => x.Index != i)
                        .Select(x => x.Item));

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
        _part1Lines = Inputs.Part1.Split(Environment.NewLine);
        _part2Lines = Inputs.Part2.Split(Environment.NewLine);
    }

    private static bool IsReportSafe(IEnumerable<int> numbers)
    {
        int? lastNumber = null;
        int? firstDifference = null;

        foreach (var number in numbers)
        {
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
