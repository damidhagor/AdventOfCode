namespace Year2024.Day06;

[DebuggerDisplay("({Position.Row},{Position.Column},{Orientation})")]
public readonly record struct GuardPosition(Position Position, int Orientation)
{
    public GuardPosition MoveForward() => Orientation switch
    {
        Positions.GuardUp => new(Position.Up, Orientation),
        Positions.GuardRight => new(Position.Right, Orientation),
        Positions.GuardDown => new(Position.Down, Orientation),
        Positions.GuardLeft => new(Position.Left, Orientation),
        _ => throw new InvalidOperationException()
    };

    public GuardPosition TurnRight() => Orientation switch
    {
        Positions.GuardUp => new(Position, Positions.GuardRight),
        Positions.GuardRight => new(Position, Positions.GuardDown),
        Positions.GuardDown => new(Position, Positions.GuardLeft),
        Positions.GuardLeft => new(Position, Positions.GuardUp),
        _ => throw new InvalidOperationException()
    };
}