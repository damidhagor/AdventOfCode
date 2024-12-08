namespace Year2024;

public static class DayRunner
{
    public static void Run<T>()
        where T : IDay, new()
    {
        var day = new T();

        Console.WriteLine($"Day {day.Day:D2}");

        day.PrepareInput();

        var part1Start = Stopwatch.GetTimestamp();
        var part1Result = day.DoPart1();
        var part1Time = Stopwatch.GetElapsedTime(part1Start);

        Console.WriteLine($"  Part 1: {part1Result} ({part1Time.TotalMilliseconds}ms)");

        var part2Start = Stopwatch.GetTimestamp();
        var part2Result = day.DoPart2();
        var part2Time = Stopwatch.GetElapsedTime(part2Start);

        Console.WriteLine($"  Part 2: {part2Result} ({part2Time.TotalMilliseconds}ms)");
    }
}