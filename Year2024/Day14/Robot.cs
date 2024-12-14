namespace Year2024.Day14;

[DebuggerDisplay("P ({Position.X}, {Position.Y})  V ({Velocity.X}, {Velocity.Y})")]
public readonly struct Robot(Coordinate position, Coordinate velocity)
{
    public Coordinate Position { get; } = position;

    public Coordinate Velocity { get; } = velocity;
}