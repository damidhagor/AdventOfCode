using System.Text.RegularExpressions;

namespace Year2024.Day13;

public sealed partial class Day13 : IDay
{
    private Game[] _games = default!;

    public int Day => 13;

    [GeneratedRegex(@"X[+=](\d+), Y[+=](\d+)")]
    private partial Regex CoordinateRegex { get; }

    public string DoPart1()
    {
        var tokens = 0L;

        for (var g = 0; g < _games.Length; g++)
        {
            var game = _games[g];
            tokens += Calc(game.ButtonA, game.ButtonB, game.Prize);
        }

        return tokens.ToString();
    }

    public string DoPart2()
    {
        var tokens = 0L;

        for (var g = 0; g < _games.Length; g++)
        {
            var game = _games[g];
            var prize = new Coordinate(game.Prize.X + 10_000_000_000_000, game.Prize.Y + 10_000_000_000_000);
            tokens += Calc(game.ButtonA, game.ButtonB, prize);
        }

        return tokens.ToString();
    }

    public void PrepareInput()
    {
        var gameInput = Inputs.Games.Split($"{Environment.NewLine}{Environment.NewLine}");

        _games = new Game[gameInput.Length];
        for (var g = 0; g < gameInput.Length; g++)
        {
            var lines = gameInput[g].Split(Environment.NewLine);

            var buttonAMatches = CoordinateRegex.Matches(lines[0]);
            var buttonBMatches = CoordinateRegex.Matches(lines[1]);
            var prizeMatches = CoordinateRegex.Matches(lines[2]);

            _games[g] = new(
                new(int.Parse(buttonAMatches[0].Groups[1].Value), int.Parse(buttonAMatches[0].Groups[2].Value)),
                new(int.Parse(buttonBMatches[0].Groups[1].Value), int.Parse(buttonBMatches[0].Groups[2].Value)),
                new(int.Parse(prizeMatches[0].Groups[1].Value), int.Parse(prizeMatches[0].Groups[2].Value)));
        }
    }

    private static long Calc(Coordinate buttonA, Coordinate buttonB, Coordinate prize)
    {
        // https://github.com/lamperi/aoc/blob/main/2024/13/solve.py
        // m = (ax * py - ay * px)/(ax * by - ay * bx)
        // n = (px - m * bx) / (ax)

        var buttonBPushes = (((double)buttonA.X * prize.Y) - (buttonA.Y * prize.X)) / ((buttonA.X * buttonB.Y) - (buttonA.Y * buttonB.X));
        var buttonAPushes = (prize.X - (buttonBPushes * buttonB.X)) / buttonA.X;

        if (double.IsInteger(buttonAPushes) && double.IsInteger(buttonBPushes))
        {
            return ((long)buttonAPushes * Inputs.ButtonATokens) + ((long)buttonBPushes * Inputs.ButtonBTokens);
        }

        return 0L;
    }
}
