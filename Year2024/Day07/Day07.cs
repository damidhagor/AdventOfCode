using Equation = (long Result, long[] Numbers);

namespace Year2024.Day07;

public sealed class Day07 : IDay
{
    private Equation[] _equations = default!;

    public int Day => 7;

    public long DoPart1()
    {
        var sum = 0L;
        foreach (var equation in _equations)
        {
            if (IsValidWithAddMultiply(equation, 0, 0, true))
            {
                sum += equation.Result;
            }
        }

        return sum;
    }

    public long DoPart2()
    {
        var sum = 0L;
        foreach (var equation in _equations)
        {
            if (IsValidWithAddMultiplyConcat(equation, 0, 0, 0))
            {
                sum += equation.Result;
            }
        }

        return sum;
    }

    public void PrepareInput()
        => _equations = Inputs.Equations
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split(": ");

                var result = long.Parse(parts[0]);
                var numbers = parts[1].Split(" ").Select(long.Parse).ToArray();

                return (result, numbers);
            })
            .ToArray();

    private static bool IsValidWithAddMultiply(Equation equation, long currentResult, int nextNumberIndex, bool addNextNumber)
    {
        if (nextNumberIndex == equation.Numbers.Length)
        {
            return equation.Result == currentResult;
        }

        currentResult = addNextNumber
            ? currentResult + equation.Numbers[nextNumberIndex]
            : currentResult * equation.Numbers[nextNumberIndex];

        return IsValidWithAddMultiply(equation, currentResult, nextNumberIndex + 1, true)
            || IsValidWithAddMultiply(equation, currentResult, nextNumberIndex + 1, false);
    }

    private static bool IsValidWithAddMultiplyConcat(Equation equation, long currentResult, int nextNumberIndex, int operatorType)
    {
        if (nextNumberIndex == equation.Numbers.Length)
        {
            return equation.Result == currentResult;
        }

        var nextNumber = equation.Numbers[nextNumberIndex];

        currentResult = operatorType switch
        {
            0 => currentResult + nextNumber,
            1 => currentResult * nextNumber,
            2 => currentResult * (int)Math.Pow(10, GetDigitCount(nextNumber)) + nextNumber,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType))
        };

        return IsValidWithAddMultiplyConcat(equation, currentResult, nextNumberIndex + 1, 0)
            || IsValidWithAddMultiplyConcat(equation, currentResult, nextNumberIndex + 1, 1)
            || IsValidWithAddMultiplyConcat(equation, currentResult, nextNumberIndex + 1, 2);
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
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
    }
}
