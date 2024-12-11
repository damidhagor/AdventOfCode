namespace Year2024.Day11;

public sealed class Day11 : IDay
{
    private readonly Dictionary<(long, int), long> _cache = [];
    private long[] _stones = default!;

    public int Day => 11;

    public long DoPart1()
    {
        var stones = new List<long>(_stones);

        var stoneCount = 0L;
        for (var s = 0; s < _stones.Length; s++)
        {
            stoneCount += Blink(stones[s], Inputs.BlinksPart1);
        }

        return stoneCount;
    }

    public long DoPart2()
    {
        var stones = new List<long>(_stones);

        var stoneCount = 0L;
        for (var s = 0; s < _stones.Length; s++)
        {
            stoneCount += Blink(stones[s], Inputs.BlinksPart2);
        }

        return stoneCount;
    }

    public void PrepareInput() => _stones = Inputs.Stones.Split(' ').Select(long.Parse).ToArray();

    private long Blink(long stone, int blinksRemaining)
    {
        if (blinksRemaining == 0)
        {
            return 1;
        }

        if (stone == 0)
        {
            return blinksRemaining switch
            {
                1 => 1,
                2 => 1,
                3 => 2,
                _ => Blink(20, blinksRemaining - 3) + Blink(24, blinksRemaining - 3)
            };
        }

        var cacheKey = (stone, blinksRemaining);
        if (_cache.TryGetValue(cacheKey, out long value))
        {
            return value;
        }

        var digitCount = GetDigitCount(stone);

        if (digitCount % 2 != 0)
        {
            return Blink(stone * 2024, blinksRemaining - 1);
        }

        var divider = GetDivider(digitCount);

        var rightNumber = stone % divider;
        var leftNumber = stone / divider;

        var leftStones = Blink(leftNumber, blinksRemaining - 1);
        var rightStones = Blink(rightNumber, blinksRemaining - 1);

        var stoneCount = leftStones + rightStones;
        _cache[cacheKey] = stoneCount;

        return stoneCount;
    }

    private static int GetDigitCount(long number)
    {
        return number switch
        {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            < 10000000 => 7,
            < 100000000 => 8,
            < 1000000000 => 9,
            < 10000000000 => 10,
            < 100000000000 => 11,
            < 1000000000000 => 12,
            < 10000000000000 => 13,
            < 100000000000000 => 14,
            < 1000000000000000 => 15,
            < 10000000000000000 => 16,
            < 100000000000000000 => 17,
            < 1000000000000000000 => 18,
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
    }

    private static long GetDivider(int digitCount)
    {
        return digitCount switch
        {
            2 => 10,
            4 => 100,
            6 => 1000,
            8 => 10000,
            10 => 100000,
            12 => 1000000,
            14 => 10000000,
            16 => 100000000,
            18 => 1000000000,
            _ => throw new ArgumentOutOfRangeException(nameof(digitCount))
        };
    }
}
