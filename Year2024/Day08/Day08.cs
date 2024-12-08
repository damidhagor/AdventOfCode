namespace Year2024.Day08;

public sealed class Day08 : IDay
{
    private Map _map = default!;

    public int Day => 8;

    public long DoPart1()
    {
        var antennas = GetAntennaPositions(_map);

        var antinodes = new HashSet<Position>();
        foreach (var positions in antennas.Values)
        {
            for (var a1 = 0; a1 < positions.Count - 1; a1++)
            {
                for (var a2 = a1 + 1; a2 < positions.Count; a2++)
                {
                    var distance = positions[a1] - positions[a2];

                    var antinode1 = positions[a1] + distance;
                    var antinode2 = positions[a2] - distance;

                    if (_map.IsOnMap(antinode1))
                    {
                        antinodes.Add(antinode1);
                    }

                    if (_map.IsOnMap(antinode2))
                    {
                        antinodes.Add(antinode2);
                    }
                }
            }
        }

        return antinodes.Count;
    }

    public long DoPart2()
    {
        var antennas = GetAntennaPositions(_map);

        var antinodes = new HashSet<Position>();
        foreach (var positions in antennas.Values)
        {
            for (var a1 = 0; a1 < positions.Count - 1; a1++)
            {
                for (var a2 = a1 + 1; a2 < positions.Count; a2++)
                {
                    antinodes.Add(positions[a1]);
                    antinodes.Add(positions[a2]);

                    var distance = positions[a1] - positions[a2];

                    var antinode1 = positions[a1] + distance;
                    while (_map.IsOnMap(antinode1))
                    {
                        antinodes.Add(antinode1);
                        antinode1 += distance;
                    }


                    var antinode2 = positions[a2] - distance;
                    while (_map.IsOnMap(antinode2))
                    {
                        antinodes.Add(antinode2);
                        antinode2 -= distance;
                    }
                }
            }
        }

        return antinodes.Count;
    }

    public void PrepareInput() => _map = Map.Parse(Inputs.Map);

    private static Dictionary<char, List<Position>> GetAntennaPositions(Map map)
    {
        var antennas = new Dictionary<char, List<Position>>();
        for (var r = 0; r < map.Rows; r++)
        {
            for (var c = 0; c < map.Columns; c++)
            {
                if (map[r, c] == '.')
                {
                    continue;
                }

                if (antennas.TryGetValue(map[r, c], out var positions))
                {
                    positions.Add(new(r, c));
                }
                else
                {
                    antennas[map[r, c]] = [new(r, c)];
                }
            }
        }

        return antennas;
    }
}