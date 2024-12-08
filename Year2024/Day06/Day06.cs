namespace Year2024.Day06;

public sealed class Day06 : IDay
{
    private Map _map = default!;

    public int Day => 6;

    public long DoPart1()
    {
        CalculateGuardRoute(_map.Copy(), out var _);

        var visitedPositions = 0;
        for (var r = 0; r < _map.Rows; r++)
        {
            for (var c = 0; c < _map.Columns; c++)
            {
                if (_map.Room[r, c] == Map.Positions.Visited)
                {
                    visitedPositions++;
                }
            }
        }

        return visitedPositions;
    }

    public long DoPart2()
    {
        return FindGuardRouteLoops(_map.Copy());
    }

    public void PrepareInput() => _map = Map.Parse(Inputs.Room);

    private static int FindGuardRouteLoops(Map map)
    {
        var initialGuardRow = map.GetInitialGuardPosition().Position.Row;
        var initialGuardColumn = map.GetInitialGuardPosition().Position.Column;
        var originalRoute = CalculateGuardRoute(map.Copy(), out _);

        var obstaclePositions = new HashSet<(int Row, int Column)>();
        foreach (var position in originalRoute.Where(p => p.Row != initialGuardRow || p.Column != initialGuardColumn))
        {
            var newMap = map.Copy();
            newMap.Room[position.Row, position.Column] = Map.Positions.Obstacle;

            CalculateGuardRoute(newMap, out var routeLooped);

            if (routeLooped)
            {
                obstaclePositions.Add((position.Row, position.Column));
            }
        }

        return obstaclePositions.Count;
    }

    private static HashSet<(int Row, int Column, int Orientation)> CalculateGuardRoute(Map map, out bool routeLooped)
    {
        var route = new HashSet<(int Row, int Column, int Orientation)>();
        var previousPosition = map.GetInitialGuardPosition();

        routeLooped = false;
        var routeChanging = true;
        while (routeChanging)
        {
            var newPosition = MoveGuard(map, previousPosition);

            if (newPosition.Position == previousPosition.Position)
            {
                previousPosition = newPosition;
                map[newPosition.Position] = previousPosition.Orientation;
                continue;
            }

            // loop detected
            if (!route.Add(previousPosition.ToTuple()))
            {
                routeChanging = false;
                routeLooped = true;
                continue;
            }

            map[previousPosition.Position] = Map.Positions.Visited;

            if (!map.IsPositionInRoom(newPosition.Position))
            {
                routeChanging = false;
                continue;
            }

            previousPosition = newPosition;
            map[newPosition.Position] = newPosition.Orientation;
        }

        return route;
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
