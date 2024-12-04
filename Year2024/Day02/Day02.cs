namespace Year2024.Day02;

public static class Day02
{
    public static void DoPart1()
    {
        int safeReports = 0;

        foreach (var line in Inputs.Part1.Split("\r\n"))
        {
            var isReportSafe = IsReportSafe(line.Split(' ').Select(int.Parse));

            if (isReportSafe)
            {
                safeReports++;
            }
        }

        Console.WriteLine($"Day02 Part 1: {safeReports}");
    }

    public static void DoPart2()
    {
        int safeReports = 0;

        foreach (var line in Inputs.Part2.Split("\r\n"))
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

        Console.WriteLine($"Day02 Part 2: {safeReports}");
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
