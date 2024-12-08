namespace Year2024.Day06;

public sealed class Map(int[,] room, GuardPosition guardPosition)
{
    private readonly int[,] _room = room;

    public int Rows { get; } = room.GetLength(0);

    public int Columns { get; } = room.GetLength(1);

    public GuardPosition GuardPosition { get; } = guardPosition;

    public static Map Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var rows = lines.Length;
        var columns = lines[0].Length;

        var map = new int[rows, columns];
        GuardPosition? initialGuardPosition = null;

        for (var r = 0; r < lines.Length; r++)
        {
            for (var c = 0; c < lines[r].Length; c++)
            {
                map[r, c] = lines[r][c] switch
                {
                    '#' => Positions.Obstacle,
                    '^' => Positions.GuardUp,
                    _ => Positions.Empty
                };

                if (map[r, c] is >= Positions.GuardUp and <= Positions.GuardLeft)
                {
                    initialGuardPosition = new(new(r, c), map[r, c]);
                }
            }
        }

        return new(map, initialGuardPosition!.Value);
    }

    public int this[Position position]
    {
        get => _room[position.Row, position.Column];
        set => _room[position.Row, position.Column] = value;
    }

    public bool IsPositionInRoom(Position position) =>
        position.Row >= 0 && position.Row < Rows
     && position.Column >= 0 && position.Column < Columns;

    public static class Positions
    {
        public const int Empty = 0;
        public const int Obstacle = 1;
        public const int Visited = 2;
        public const int GuardUp = 3;
        public const int GuardRight = 4;
        public const int GuardDown = 5;
        public const int GuardLeft = 6;
    }
}
