namespace Year2024.Day17;

public sealed class Computer(long initialRegisterA, long initialRegisterB, long initialRegisterC, int[] program)
{
    private readonly long _initialRegisterA = initialRegisterA;
    private readonly long _initialRegisterB = initialRegisterB;
    private readonly long _initialRegisterC = initialRegisterC;

    public int[] Program { get; private set; } = program;

    public string Run(long? registerAOverride = null)
    {
        var registerA = registerAOverride ?? _initialRegisterA;
        var registerB = _initialRegisterB;
        var registerC = _initialRegisterC;
        var instructionPointer = 0;

        var output = new List<long>();

        while (instructionPointer < Program.Length)
        {
            var opcode = Program[instructionPointer];
            var operand = Program[instructionPointer + 1];

            switch (opcode)
            {
                case 0: // adv
                    registerA >>= (int)DecodeOperand(operand, registerA, registerB, registerC);
                    instructionPointer += 2;
                    break;
                case 1: // bxl
                    registerB ^= operand;
                    instructionPointer += 2;
                    break;
                case 2: // bst
                    registerB = (int)DecodeOperand(operand, registerA, registerB, registerC) & 7;
                    instructionPointer += 2;
                    break;
                case 3: // jnz
                    instructionPointer = registerA != 0 ? operand : instructionPointer + 2;
                    break;
                case 4: // bxc
                    registerB ^= registerC;
                    instructionPointer += 2;
                    break;
                case 5: // out
                    output.Add(DecodeOperand(operand, registerA, registerB, registerC) & 7);
                    instructionPointer += 2;
                    break;
                case 6: // bdv
                    registerB = registerA >> (int)DecodeOperand(operand, registerA, registerB, registerC);
                    instructionPointer += 2;
                    break;
                case 7: // cdv
                    registerC = registerA >> (int)DecodeOperand(operand, registerA, registerB, registerC);
                    instructionPointer += 2;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown opcode: {opcode}");
            }
        }

        return string.Join(',', output);
    }

    private static long DecodeOperand(int operand, long registerA, long registerB, long registerC)
        => operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => registerA,
            5 => registerB,
            6 => registerC,
            _ => throw new InvalidOperationException($"Unknown operand: {operand}")
        };
}