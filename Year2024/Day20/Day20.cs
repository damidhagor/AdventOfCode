namespace Year2024.Day20;

public sealed class Day20 : IDay
{
    private Grid<int> _mapPart1 = default!;
    private Grid<int> _mapPart2 = default!;
    private Position _start = default!;
    private Position _end = default!;

    public int Day => 20;

    public string DoPart1()
    {
        var route = CalculateRoute(_mapPart1, _start, _end);

        var cheats = new HashSet<(Position, Position)>();

        for (var r = 0; r < route.Count; r++)
        {
            CalculateCheats(_mapPart1, route[r], 2, 100, cheats);
        }

        return cheats.Count.ToString();
    }

    public string DoPart2()
    {
        var route = CalculateRoute(_mapPart2, _start, _end);

        var cheats = new HashSet<(Position, Position)>();

        for (var r = 0; r < route.Count; r++)
        {
            CalculateCheats(_mapPart2, route[r], 20, 100, cheats);
        }

        return cheats.Count.ToString();
    }

    public void PrepareInput()
    {
        _mapPart1 = Grid<int>.Parse(
            Inputs.Input,
            c => c switch
            {
                '#' => Fields.Wall,
                'S' => Fields.Start,
                'E' => Fields.End,
                _ => Fields.Empty
            });

        for (var r = 0; r < _mapPart1.Rows; r++)
        {
            for (var c = 0; c < _mapPart1.Columns; c++)
            {
                if (_mapPart1[r, c] == Fields.Start)
                {
                    _start = new(r, c);
                    _mapPart1[r, c] = Fields.Empty;
                }
                else if (_mapPart1[r, c] == Fields.End)
                {
                    _end = new(r, c);
                    _mapPart1[r, c] = Fields.Empty;
                }
            }
        }

        _mapPart2 = _mapPart1.Clone();
    }

    private static List<Position> CalculateRoute(Grid<int> map, Position start, Position end)
    {
        var route = new List<Position>();

        var position = start;
        var steps = 0;
        var reachedEnd = false;

        while (!reachedEnd)
        {
            map[position] = steps++;
            route.Add(position);

            if (map[position.Up] == Fields.Empty)
            {
                position = position.Up;
            }
            else if (map[position.Right] == Fields.Empty)
            {
                position = position.Right;
            }
            else if (map[position.Down] == Fields.Empty)
            {
                position = position.Down;
            }
            else if (map[position.Left] == Fields.Empty)
            {
                position = position.Left;
            }
            else
            {
                reachedEnd = true;
            }
        }

        return route;
    }

    private static void CalculateCheats(
        Grid<int> map,
        Position start,
        int stepsAvailable,
        int threshold,
        HashSet<(Position, Position)> cheats)
    {
        for (var r = start.Row - stepsAvailable; r <= start.Row + stepsAvailable; r++)
        {
            for (var c = start.Column - stepsAvailable; c <= start.Column + stepsAvailable; c++)
            {
                var end = new Position(r, c);

                var stepsTaken = Math.Abs(start.Row - r) + Math.Abs(start.Column - c);

                if (!map.IsInBounds(end)
                    || map[end] == Fields.Wall
                    || stepsTaken > stepsAvailable)
                {
                    continue;
                }

                var stepsSaved = map[end] - map[start] - stepsTaken;
                if (stepsSaved >= threshold)
                {
                    cheats.Add((start, end));
                }
            }
        }
    }
}
