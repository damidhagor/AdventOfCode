namespace Year2024;

public interface IDay
{
    int Day { get; }

    void PrepareInput();

    long DoPart1();

    long DoPart2();
}
