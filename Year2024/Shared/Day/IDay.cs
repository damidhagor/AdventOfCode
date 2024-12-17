namespace Year2024.Shared.Day;

public interface IDay
{
    int Day { get; }

    void PrepareInput();

    string DoPart1();

    string DoPart2();
}
