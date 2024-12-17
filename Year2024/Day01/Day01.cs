namespace Year2024.Day01;

public sealed class Day01 : IDay
{
    public int Day => 1;

    public string DoPart1()
    {
        var left = Inputs.Part1Left;
        var right = Inputs.Part1Right;

        Array.Sort(left);
        Array.Sort(right);

        var distance = 0;
        for (var i = 0; i < left.Length - 1; i++)
        {
            distance += Math.Abs(left[i] - right[i]);
        }

        return distance.ToString();
    }

    public string DoPart2()
    {
        var left = Inputs.Part2Left;
        var right = Inputs.Part2Right;

        var rightOccurrences = new Dictionary<int, int>();
        for (var i = 0; i < right.Length - 1; i++)
        {
            rightOccurrences[right[i]] = rightOccurrences.TryGetValue(right[i], out var value) ? ++value : 1;
        }

        var similarity = left.Sum(l => l * rightOccurrences.GetValueOrDefault(l));

        var sum = 0;
        for (var i = 0; i < left.Length - 1; i++)
        {
            sum += left[i] * rightOccurrences.GetValueOrDefault(left[i]);
        }

        return similarity.ToString();
    }

    public void PrepareInput() { }
}