namespace Year2024.Day17;

public sealed class Day17 : IDay
{
    private Computer _computer = default!;

    public int Day => 17;

    public string DoPart1()
    {
        return _computer.Run();
    }

    public string DoPart2()
    {
        var registerA = FindRegisterA(_computer, 0L, 1);

        return registerA?.ToString() ?? throw new InvalidOperationException("No solution found");
    }

    public void PrepareInput()
    {
        var lines = Inputs.Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var registerA = long.Parse(lines[0].Split(": ")[1]);
        var registerB = long.Parse(lines[1].Split(": ")[1]);
        var registerC = long.Parse(lines[2].Split(": ")[1]);
        var program = lines[3].Split(": ")[1].Split(',').Select(int.Parse).ToArray();

        _computer = new Computer(registerA, registerB, registerC, program);
    }

    private static long? FindRegisterA(Computer computer, long registerA, int outputLength)
    {
        if (outputLength > computer.Program.Length)
        {
            return registerA;
        }

        var expectedOutput = string.Join(',', computer.Program[^outputLength..]);
        registerA <<= 3;

        for (var a = 0; a < 8; a++)
        {
            var output = computer.Run(registerA ^ a);
            if (output == expectedOutput)
            {
                var isSolution = FindRegisterA(computer, registerA ^ a, outputLength + 1);
                if (isSolution.HasValue)
                {
                    return isSolution;
                }
            }
        }

        return null;
    }
}
