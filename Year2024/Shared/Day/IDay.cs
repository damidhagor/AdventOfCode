namespace Year2024.Shared.Day;

public interface IDay
{
    int Day { get; }

    void PrepareInput();

    long DoPart1();

    long DoPart2();
}
