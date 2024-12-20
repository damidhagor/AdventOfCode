namespace Year2024.Day10;

public sealed class Day10 : IDay
{
    private Grid<int> _map = default!;

    public int Day => 10;

    public string DoPart1()
    {
        var scores = 0L;
        for (var r = 0; r < _map.Rows; r++)
        {
            for (var c = 0; c < _map.Columns; c++)
            {
                if (_map[r, c] == 0)
                {
                    var trailheads = new HashSet<Position>();
                    GetTrailScore(_map, new Position(r, c), trailheads);
                    scores += trailheads.Count;
                }
            }
        }

        return scores.ToString();
    }

    public string DoPart2()
    {
        var ratings = 0L;
        for (var r = 0; r < _map.Rows; r++)
        {
            for (var c = 0; c < _map.Columns; c++)
            {
                if (_map[r, c] == 0)
                {
                    ratings += GetTrailRating(_map, new Position(r, c));
                }
            }
        }

        return ratings.ToString();
    }

    private static void GetTrailScore(Grid<int> map, Position position, HashSet<Position> trailheads)
    {
        var height = map[position];
        if (height == 9)
        {
            trailheads.Add(position);
            return;
        }

        // Up
        var up = position.Up;
        if (map.IsInBounds(up) && map[up] == height + 1)
        {
            GetTrailScore(map, up, trailheads);
        }

        // Right
        var right = position.Right;
        if (map.IsInBounds(right) && map[right] == height + 1)
        {
            GetTrailScore(map, right, trailheads);
        }

        // Down
        var down = position.Down;
        if (map.IsInBounds(down) && map[down] == height + 1)
        {
            GetTrailScore(map, down, trailheads);
        }

        // Left
        var left = position.Left;
        if (map.IsInBounds(left) && map[left] == height + 1)
        {
            GetTrailScore(map, left, trailheads);
        }
    }

    private static int GetTrailRating(Grid<int> map, Position position)
    {
        var height = map[position];
        if (height == 9)
        {
            return 1;
        }

        var trails = 0;

        var up = position.Up;
        if (map.IsInBounds(up) && map[up] == height + 1)
        {
            trails += GetTrailRating(map, up);
        }

        var right = position.Right;
        if (map.IsInBounds(right) && map[right] == height + 1)
        {
            trails += GetTrailRating(map, right);
        }

        var down = position.Down;
        if (map.IsInBounds(down) && map[down] == height + 1)
        {
            trails += GetTrailRating(map, down);
        }

        var left = position.Left;
        if (map.IsInBounds(left) && map[left] == height + 1)
        {
            trails += GetTrailRating(map, left);
        }

        return trails;
    }

    public void PrepareInput() => _map = Grid<int>.Parse(Inputs.Map, c => c - '0');
}
