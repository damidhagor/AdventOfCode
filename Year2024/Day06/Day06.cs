namespace Year2024.Day06;

public sealed class Day06 : IDay
{
    private Grid<int> _map = default!;
    private GuardPosition _initialGuardPosition;

    public int Day => 6;

    public long DoPart1()
    {
        var route = CalculateGuardRoute(_map, _initialGuardPosition, out var _);
        return route.Count;
    }

    public long DoPart2()
    {
        return FindGuardRouteLoops(_map, _initialGuardPosition);
    }

    public void PrepareInput()
    {
        _map = Grid<int>.Parse(
            Inputs.Room,
            c => c switch
            {
                '#' => Positions.Obstacle,
                '^' => Positions.GuardUp,
                _ => Positions.Empty
            });

        for (var r = 0; r < _map.Rows; r++)
        {
            for (var c = 0; c < _map.Columns; c++)
            {
                if (_map[r, c] is >= Positions.GuardUp and <= Positions.GuardLeft)
                {
                    _initialGuardPosition = new(new(r, c), _map[r, c]);
                    return;
                }
            }
        }
    }

    private static int FindGuardRouteLoops(Grid<int> map, GuardPosition guardPosition)
    {
        var originalRoute = CalculateGuardRoute(map, guardPosition, out _);

        var obstaclePositions = new HashSet<Position>();
        foreach (var position in originalRoute.Where(p => p != guardPosition.Position))
        {
            map[position] = Positions.Obstacle;
            CalculateGuardRoute(map, guardPosition, out var routeLooped);
            map[position] = Positions.Empty;

            if (routeLooped)
            {
                obstaclePositions.Add(position);
            }
        }

        return obstaclePositions.Count;
    }

    private static HashSet<Position> CalculateGuardRoute(Grid<int> map, GuardPosition guardPosition, out bool routeLooped)
    {
        var visited = new HashSet<Position>();
        var route = new HashSet<GuardPosition>();

        routeLooped = false;
        var routeChanging = true;
        while (routeChanging)
        {
            var newGuardPosition = MoveGuard(map, guardPosition);

            if (newGuardPosition.Position == guardPosition.Position)
            {
                guardPosition = newGuardPosition;
                continue;
            }

            // loop detected
            if (!route.Add(guardPosition))
            {
                routeChanging = false;
                routeLooped = true;
                continue;
            }

            visited.Add(guardPosition.Position);

            if (!map.IsInBounds(newGuardPosition.Position))
            {
                routeChanging = false;
                continue;
            }

            guardPosition = newGuardPosition;
        }

        return visited;
    }

    private static GuardPosition MoveGuard(Grid<int> map, GuardPosition position)
    {
        var nextPosition = position.MoveForward();

        if (map.IsInBounds(nextPosition.Position)
            && map[nextPosition.Position] == Positions.Obstacle)
        {
            return position.TurnRight();
        }

        return nextPosition;
    }
}
