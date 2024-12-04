using System.Text.RegularExpressions;

namespace Year2024.Day03;

public static partial class Day03
{
    [GeneratedRegex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    private static partial Regex Part1Regex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)", RegexOptions.Compiled)]
    private static partial Regex Part2Regex();

    public static void DoPart1()
    {
        var input = Inputs.Part1;

        var sum = 0;
        foreach (Match match in Part1Regex().Matches(input))
        {
            var number1 = int.Parse(match.Groups[1].ValueSpan);
            var number2 = int.Parse(match.Groups[2].ValueSpan);

            sum += number1 * number2;
        }

        Console.WriteLine($"Day03 Part 1: {sum}");
    }

    public static void DoPart2()
    {
        var input = Inputs.Part2;

        var sum = 0;
        var shouldSum = true;

        foreach (Match match in Part2Regex().Matches(input))
        {
            if (match.Value == "do()")
            {
                shouldSum = true;
                continue;
            }

            if (match.Value == "don't()")
            {
                shouldSum = false;
                continue;
            }

            var number1 = int.Parse(match.Groups[1].ValueSpan);
            var number2 = int.Parse(match.Groups[2].ValueSpan);

            if (shouldSum)
            {
                sum += number1 * number2;
            }
        }

        Console.WriteLine($"Day03 Part 2: {sum}");
    }
}
