namespace Year2024.Day01;

public sealed class Day01 : IDay
{
    public int Day => 1;

    public long DoPart1()
    {
        var left = Inputs.Part1Left;
        var right = Inputs.Part1Right;

        var distance =
            left.Order()
                .Zip(
                    right.Order(),
                    (l, r) => Math.Abs(l - r))
                .Sum();

        return distance;
    }

    public long DoPart2()
    {
        var left = Inputs.Part2Left;
        var right = Inputs.Part2Right;

        var rightOccurrences = right.CountBy(x => x).ToDictionary(x => x.Key, x => x.Value);

        var similarity = left.Sum(l => l * rightOccurrences.GetValueOrDefault(l));

        return similarity;
    }

    public void PrepareInput() { }
}