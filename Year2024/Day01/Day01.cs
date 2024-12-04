namespace Year2024.Day01;

public static class Day01
{
    public static void DoPart1()
    {
        var left = Inputs.Part1Left;
        var right = Inputs.Part1Right;

        var distance =
            left.Order()
                .Zip(
                    right.Order(),
                    (l, r) => Math.Abs(l - r))
                .Sum();

        Console.WriteLine($"Day01 Part 1: {distance}");
    }

    public static void DoPart2()
    {
        var left = Inputs.Part2Left;
        var right = Inputs.Part2Right;

        var rightOccurrences = right.CountBy(x => x).ToDictionary(x => x.Key, x => x.Value);

        var similarity = left.Sum(l => l * rightOccurrences.GetValueOrDefault(l));

        Console.WriteLine($"Day01 Part 2: {similarity}");
    }
}