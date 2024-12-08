namespace Year2024.Day06;

[DebuggerDisplay("({Position.Row},{Position.Column},{Orientation})")]
public readonly record struct GuardPosition(Position position, int orientation)
{
    public readonly Position Position { get; } = position;

    public int Orientation { get; } = orientation;

    public GuardPosition MoveForward() => Orientation switch
    {
        Map.Positions.GuardUp => new(Position.GetUp(), Orientation),
        Map.Positions.GuardRight => new(Position.GetRight(), Orientation),
        Map.Positions.GuardDown => new(Position.GetDown(), Orientation),
        Map.Positions.GuardLeft => new(Position.GetLeft(), Orientation),
        _ => throw new InvalidOperationException()
    };

    public GuardPosition TurnRight() => Orientation switch
    {
        Map.Positions.GuardUp => new(Position, Map.Positions.GuardRight),
        Map.Positions.GuardRight => new(Position, Map.Positions.GuardDown),
        Map.Positions.GuardDown => new(Position, Map.Positions.GuardLeft),
        Map.Positions.GuardLeft => new(Position, Map.Positions.GuardUp),
        _ => throw new InvalidOperationException()
    };
}