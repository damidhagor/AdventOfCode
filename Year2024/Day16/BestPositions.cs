namespace Year2024.Day16;

public sealed class BestPositions
{
    public long Score { get; private set; } = long.MaxValue;

    public HashSet<Position> Positions { get; } = [];

    public void Update(long newScore, Stack<Position> newRoute)
    {
        if (newScore > Score)
        {
            return;
        }

        if (newScore < Score)
        {
            Score = newScore;
            Positions.Clear();
        }

        foreach (var position in newRoute)
        {
            Positions.Add(position);
        }
    }
}