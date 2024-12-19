namespace Year2024.Day19;

public sealed class Day19 : IDay
{
    private Dictionary<char, string[]> _availablePatterns = default!;
    private string[] _neededPatterns = default!;

    public int Day => 18;

    public string DoPart1()
    {
        var possiblePatterns = 0;

        var cache = new Dictionary<string, long>().GetAlternateLookup<ReadOnlySpan<char>>();
        for (var i = 0; i < _neededPatterns.Length; i++)
        {
            if (GetPatternPossibilities(_neededPatterns[i], _availablePatterns, cache) > 0)
            {
                possiblePatterns++;
            }
        }

        return possiblePatterns.ToString();
    }

    public string DoPart2()
    {
        var possiblePatterns = 0L;

        var cache = new Dictionary<string, long>().GetAlternateLookup<ReadOnlySpan<char>>();
        for (var i = 0; i < _neededPatterns.Length; i++)
        {
            possiblePatterns += GetPatternPossibilities(_neededPatterns[i], _availablePatterns, cache);
        }

        return possiblePatterns.ToString();
    }

    public void PrepareInput()
    {
        var parts = Inputs.Input.Split($"{Environment.NewLine}{Environment.NewLine}");

        _availablePatterns = parts[0]
            .Split(',', StringSplitOptions.TrimEntries)
            .GroupBy(x => x[0])
            .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Length).ToArray());
        _neededPatterns = parts[1].Split(Environment.NewLine);
    }

    private static long GetPatternPossibilities(
        ReadOnlySpan<char> pattern,
        Dictionary<char, string[]> availablePatterns,
        Dictionary<string, long>.AlternateLookup<ReadOnlySpan<char>> cache)
    {
        if (cache.TryGetValue(pattern, out var value))
        {
            return value;
        }

        if (pattern.Length == 0)
        {
            return 1L;
        }

        if (!availablePatterns.TryGetValue(pattern[0], out var matchingPatterns))
        {
            return 0L;
        }

        var possiblePatterns = 0L;

        for (var i = 0; i < matchingPatterns.Length; i++)
        {
            if (pattern.StartsWith(matchingPatterns[i]))
            {
                possiblePatterns += GetPatternPossibilities(pattern[matchingPatterns[i].Length..], availablePatterns, cache);
            }
        }

        return cache[pattern] = possiblePatterns;
    }
}