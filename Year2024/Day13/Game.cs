namespace Year2024.Day13;

public readonly struct Game(Coordinate buttonA, Coordinate buttonB, Coordinate prize)
{
    public Coordinate ButtonA { get; } = buttonA;

    public Coordinate ButtonB { get; } = buttonB;

    public Coordinate Prize { get; } = prize;
}
