namespace Year2024.Day18;

public sealed class Day18 : IDay
{
    private Grid<int> _memoryPart1 = default!;
    private Grid<int> _memoryPart2 = default!;
    private Position[] _bytes = default!;
    private int _fallenBytes;

    public int Day => 18;

    public string DoPart1()
    {
        var path = FindPath(_memoryPart1, new(0, 0), 0);

        return path is null
            ? "No path found"
            : (path.Count - 1).ToString();
    }

    public string DoPart2()
    {
        var left = _fallenBytes;
        var right = _bytes.Length - 1;

        while (left != right)
        {
            var memory = _memoryPart2.Clone();

            var middle = (left + right) / 2;
            for (var b = _fallenBytes; b <= middle; b++)
            {
                memory[_bytes[b]] = Fields.Byte;
            }

            var path = FindPath(memory, new(0, 0), 0);

            if (path is null)
            {
                right = middle;
            }
            else
            {
                left = middle + 1;
            }
        }

        return $"{_bytes[left].Column},{_bytes[left].Row}";
    }

    public void PrepareInput()
    {
        var memorySize = Inputs.MemorySize;
        var bytes = Inputs.Bytes;
        var fallenBytes = Inputs.FallenBytes;

        _bytes = bytes
            .Split(Environment.NewLine)
            .Select(x => x.Split(','))
            .Select(x => new Position(int.Parse(x[1]), int.Parse(x[0])))
            .ToArray();

        _fallenBytes = fallenBytes;

        _memoryPart1 = new(new int[memorySize + 1, memorySize + 1]);
        _memoryPart2 = new(new int[memorySize + 1, memorySize + 1]);

        for (var r = 0; r <= memorySize; r++)
        {
            for (var c = 0; c <= memorySize; c++)
            {
                _memoryPart1[r, c] = Fields.Empty;
                _memoryPart2[r, c] = Fields.Empty;
            }
        }

        for (var b = 0; b < fallenBytes; b++)
        {
            _memoryPart1[_bytes[b]] = Fields.Byte;
            _memoryPart2[_bytes[b]] = Fields.Byte;
        }
    }

    private static List<Position>? FindPath(Grid<int> memory, Position position, int steps)
    {
        if (position.Row == memory.Rows - 1 && position.Column == memory.Columns - 1)
        {
            return [position];
        }

        if (!memory.IsInBounds(position) || memory[position] <= steps)
        {
            return null;
        }

        memory[position] = steps;

        var path = FindPath(memory, position.Up, steps + 1);

        var rightPath = FindPath(memory, position.Right, steps + 1);
        if (rightPath is not null && (path is null || rightPath.Count < path.Count))
        {
            path = rightPath;
        }

        var downPath = FindPath(memory, position.Down, steps + 1);
        if (downPath is not null && (path is null || downPath.Count < path.Count))
        {
            path = downPath;
        }

        var leftPath = FindPath(memory, position.Left, steps + 1);
        if (leftPath is not null && (path is null || leftPath.Count < path.Count))
        {
            path = leftPath;
        }

        path?.Add(position);

        return path;
    }
}

public static class Fields
{
    public const int Empty = int.MaxValue;
    public const int Byte = -1;
}