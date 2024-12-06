using System.Diagnostics;

namespace Year2024.Day06;

public static class Day06
{
    public static void DoPart1()
    {
        var map = Map.Parse(Inputs.Room);

        var start = Stopwatch.GetTimestamp();

        var route = CalculateGuardRoute(map, out var _);

        var visitedPositions = 0;
        for (var r = 0; r < map.Rows; r++)
        {
            for (var c = 0; c < map.Columns; c++)
            {
                if (map.Room[r, c] == Map.Positions.Visited)
                {
                    visitedPositions++;
                }
            }
        }

        var time = Stopwatch.GetElapsedTime(start);

        Console.WriteLine($"Day06 Part 1: {visitedPositions} ({time})");
    }

    public static void DoPart2()
    {
        var map = Map.Parse(Inputs.Room);

        var start = Stopwatch.GetTimestamp();
        var loops = FindGuardRouteLoops(map);
        var time = Stopwatch.GetElapsedTime(start);

        Console.WriteLine($"Day06 Part 2: {loops} ({time})");
    }

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
