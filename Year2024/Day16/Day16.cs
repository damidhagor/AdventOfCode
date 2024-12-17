namespace Year2024.Day16;

public sealed class Day16 : IDay
{
    private Grid<int> _labyrinth = default!;
    private Position _start = default!;
    private Position _end = default!;

    public int Day => 16;

    public string DoPart1()
    {
        var bestPositions = new BestPositions();
        var score = Move(_labyrinth, _end, _start, Directions.Right, 0, [], [], bestPositions);

        return score.ToString();
    }

    public string DoPart2()
    {
        var bestPositions = new BestPositions();
        Move(_labyrinth, _end, _start, Directions.Right, 0, [], [], bestPositions);

        return bestPositions.Positions.Count.ToString();
    }

    public void PrepareInput()
    {
        _labyrinth = Grid<int>.Parse(
            Inputs.Labyrinth,
            c => c switch
            {
                '.' => Fields.Empty,
                '#' => Fields.Wall,
                'S' => Fields.Start,
                'E' => Fields.End,
                _ => throw new InvalidOperationException()
            });

        for (var r = 0; r < _labyrinth.Rows; r++)
        {
            for (var c = 0; c < _labyrinth.Columns; c++)
            {
                if (_labyrinth[r, c] == Fields.Start)
                {
                    _start = new(r, c);
                    _labyrinth[r, c] = Fields.Empty;
                }
                else if (_labyrinth[r, c] == Fields.End)
                {
                    _end = new(r, c);
                    _labyrinth[r, c] = Fields.Empty;
                }
            }
        }
    }

    private static long Move(
        Grid<int> labyrinth,
        Position end,
        Position position,
        int direction,
        long score,
        Dictionary<Position, long> visited,
        Stack<Position> route,
        BestPositions bestPositions)
    {
        route.Push(position);

        if (labyrinth[position] == Fields.Wall)
        {
            route.Pop();
            return long.MaxValue;
        }

        if ((visited.TryGetValue(position, out var visitedScore)
            && (visitedScore < (score - 1_000))))
        {
            route.Pop();
            return long.MaxValue;
        }

        if (position == end)
        {
            bestPositions.Update(score, route);

            route.Pop();
            return score;
        }

        visited[position] = score;

        var forwardScore = Move(labyrinth, end, MoveForward(position, direction), direction, score + 1, visited, route, bestPositions);

        var clockwiseDirection = TurnClockwise(direction);
        var clockwiseScore = Move(labyrinth, end, MoveForward(position, clockwiseDirection), clockwiseDirection, score + 1_001, visited, route, bestPositions);

        var counterClockwiseDirection = TurnCounterClockwise(direction);
        var counterClockwiseScore = Move(labyrinth, end, MoveForward(position, counterClockwiseDirection), counterClockwiseDirection, score + 1_001, visited, route, bestPositions);

        route.Pop();
        return Math.Min(forwardScore, Math.Min(clockwiseScore, counterClockwiseScore));
    }

    private static Position MoveForward(Position position, int direction)
        => direction switch
        {
            Directions.Up => position.Up,
            Directions.Right => position.Right,
            Directions.Down => position.Down,
            Directions.Left => position.Left,
            _ => throw new InvalidOperationException()
        };

    private static int TurnClockwise(int direction)
        => direction switch
        {
            Directions.Up => Directions.Right,
            Directions.Right => Directions.Down,
            Directions.Down => Directions.Left,
            Directions.Left => Directions.Up,
            _ => throw new InvalidOperationException()
        };

    private static int TurnCounterClockwise(int direction)
        => direction switch
        {
            Directions.Up => Directions.Left,
            Directions.Right => Directions.Up,
            Directions.Down => Directions.Right,
            Directions.Left => Directions.Down,
            _ => throw new InvalidOperationException()
        };
}
