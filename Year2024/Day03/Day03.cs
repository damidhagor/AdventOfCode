using System.Text.RegularExpressions;

namespace Year2024.Day03;

public sealed partial class Day03 : IDay
{
    public int Day => 3;

    public long DoPart1()
    {
        var sum = 0;
        foreach (Match match in Part1Regex().Matches(Inputs.Part1))
        {
            var number1 = int.Parse(match.Groups[1].ValueSpan);
            var number2 = int.Parse(match.Groups[2].ValueSpan);

            sum += number1 * number2;
        }

        return sum;
    }

    public long DoPart2()
    {
        var sum = 0;
        var shouldSum = true;

        foreach (Match match in Part2Regex().Matches(Inputs.Part2))
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

        return sum;
    }

    public void PrepareInput() { }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    private partial Regex Part1Regex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)", RegexOptions.Compiled)]
    private partial Regex Part2Regex();
}
