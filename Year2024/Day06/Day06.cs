namespace Year2024.Day06;

public sealed class Day06 : IDay
{
    private Map _map = default!;

    public int Day => 6;

    public long DoPart1()
    {
        var route = CalculateGuardRoute(_map, out var _);
        return route.Count;
    }

    public long DoPart2()
    {
        return FindGuardRouteLoops(_map);
    }

    public void PrepareInput() => _map = Map.Parse(Inputs.Room);

    private static int FindGuardRouteLoops(Map map)
    {
        var guardPosition = map.GuardPosition;
        var originalRoute = CalculateGuardRoute(map, out _);

        var obstaclePositions = new HashSet<Position>();
        foreach (var position in originalRoute.Where(p => p != guardPosition.Position))
        {
            map[position] = Map.Positions.Obstacle;
            CalculateGuardRoute(map, out var routeLooped);
            map[position] = Map.Positions.Empty;

            if (routeLooped)
            {
                obstaclePositions.Add(position);
            }
        }

        return obstaclePositions.Count;
    }

    private static HashSet<Position> CalculateGuardRoute(Map map, out bool routeLooped)
    {
        var visited = new HashSet<Position>();
        var route = new HashSet<GuardPosition>();
        var guardPosition = map.GuardPosition;

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

            if (!map.IsPositionInRoom(newGuardPosition.Position))
            {
                routeChanging = false;
                continue;
            }

            guardPosition = newGuardPosition;
        }

        return visited;
    }

    private static GuardPosition MoveGuard(Map map, GuardPosition position)
    {
        var nextPosition = position.MoveForward();

        if (map.IsPositionInRoom(nextPosition.Position)
            && map[nextPosition.Position] == Map.Positions.Obstacle)
        {
            return position.TurnRight();
        }

        return nextPosition;
    }
}
