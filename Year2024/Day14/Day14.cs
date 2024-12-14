using System.Text.RegularExpressions;

namespace Year2024.Day14;

public sealed partial class Day14 : IDay
{
    [GeneratedRegex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
    private partial Regex RobotRegex { get; }

    private Robot[] _robots = default!;

    public int Day => 14;

    public long DoPart1()
    {
        var positions = MoveRobots(_robots, Inputs.RoomWidth, Inputs.RoomHeight, Inputs.Seconds);

        var center = new Coordinate(Inputs.RoomWidth / 2, Inputs.RoomHeight / 2);

        var topLeftCount = 0;
        var topRightCount = 0;
        var bottomLeftCount = 0;
        var bottomRightCount = 0;

        for (var p = 0; p < positions.Length; p++)
        {
            var position = positions[p];

            if (position.X < center.X && position.Y < center.Y)
            {
                topLeftCount++;
            }
            else if (position.X > center.X && position.Y < center.Y)
            {
                topRightCount++;
            }
            else if (position.X < center.X && position.Y > center.Y)
            {
                bottomLeftCount++;
            }
            else if (position.X > center.X && position.Y > center.Y)
            {
                bottomRightCount++;
            }
        }

        return topLeftCount * topRightCount * bottomLeftCount * bottomRightCount;
    }

    public long DoPart2()
    {
        var center = new Coordinate(Inputs.RoomWidth / 2, Inputs.RoomHeight / 2);
        var top = center.Y - 5;
        var bottom = center.Y + 5;
        var left = center.X - 5;
        var right = center.X + 5;

        for (var s = 0; s < 1_000_000; s++)
        {
            var positions = MoveRobots(_robots, Inputs.RoomWidth, Inputs.RoomHeight, s);

            var robotsInCenter = 0;
            for (var p = 0; p < positions.Length; p++)
            {
                var position = positions[p];
                if (position.X >= left && position.X <= right
                    && position.Y >= top && position.Y <= bottom)
                {
                    robotsInCenter++;
                }
            }

            var ratioInCenter = (double)robotsInCenter / positions.Length;
            if (ratioInCenter > 0.1)
            {
                return s;
            }
        }

        return -1L;
    }

    public void PrepareInput()
    {
        _robots = RobotRegex.Matches(Inputs.Robots)
            .Select(m => new Robot(
                new(long.Parse(m.Groups[1].Value), long.Parse(m.Groups[2].Value)),
                new(long.Parse(m.Groups[3].Value), long.Parse(m.Groups[4].Value))))
            .ToArray();
    }

    private static Coordinate[] MoveRobots(Robot[] robots, int roomWidth, int roomHeight, int seconds)
    {
        var positions = new Coordinate[robots.Length];

        for (var r = 0; r < robots.Length; r++)
        {
            var position = robots[r].Position;
            var velocity = robots[r].Velocity;

            var x = (position.X + (velocity.X * seconds)) % roomWidth;
            var y = (position.Y + (velocity.Y * seconds)) % roomHeight;

            positions[r] = new Coordinate(
                x < 0 ? roomWidth + x : x,
                y < 0 ? roomHeight + y : y);
        }

        return positions;
    }
}
