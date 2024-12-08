using System.Collections.Frozen;

namespace Year2024.Day05;

public sealed class Day05 : IDay
{
    private FrozenDictionary<int, int[]> _successorsByPredecessor = default!;
    private int[][] _updates = default!;

    public int Day => 5;

    public long DoPart1()
    {
        var pageSums = 0;
        for (var u = 0; u < _updates.Length; u++)
        {
            var update = _updates[u];
            if (IsUpdateCorrect(update, _successorsByPredecessor))
            {
                pageSums += update[update.Length / 2];
            }
        }

        return pageSums;
    }

    public long DoPart2()
    {
        var pageSums = 0;
        for (var u = 0; u < _updates.Length; u++)
        {
            var update = _updates[u];
            if (!IsUpdateCorrect(update, _successorsByPredecessor))
            {
                var correctedUpdate = CorrectUpdate(update, _successorsByPredecessor);
                pageSums += correctedUpdate[correctedUpdate.Length / 2];
            }
        }

        return pageSums;
    }

    public void PrepareInput()
    {
        _successorsByPredecessor = Inputs.Rules
            .Split(Environment.NewLine)
            .Select(line => line.Split('|'))
            .GroupBy(rule => rule[0])
            .ToFrozenDictionary(g => int.Parse(g.Key), g => g.Select(n => int.Parse(n[1])).ToArray());

        _updates = Inputs.Updates
            .Split(Environment.NewLine)
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .ToArray();
    }

    private static bool IsUpdateCorrect(int[] update, FrozenDictionary<int, int[]> successorsByPredecessor)
    {
        var isUpdateCorrect = true;
        for (var p = 0; p < update.Length; p++)
        {
            if (!successorsByPredecessor.TryGetValue(update[p], out var predecessors))
            {
                continue;
            }

            for (var pr = 0; pr < predecessors.Length; pr++)
            {
                var predecessorIndex = Array.IndexOf(update, predecessors[pr]);
                if (predecessorIndex > -1 && predecessorIndex < p)
                {
                    isUpdateCorrect = false;
                    break;
                }
            }
        }

        return isUpdateCorrect;
    }

    private static int[] CorrectUpdate(int[] update, FrozenDictionary<int, int[]> successorsByPredecessor)
    {
        var correctedUpdate = new LinkedList<int>([update[0]]);

        for (var u = 1; u < update.Length; u++)
        {
            var page = update[u];

            if (!successorsByPredecessor.TryGetValue(page, out var successors))
            {
                correctedUpdate.AddLast(page);
                continue;
            }

            var correctedPage = correctedUpdate.First;
            while (correctedPage is not null)
            {
                if (successors.Contains(correctedPage.Value))
                {
                    correctedUpdate.AddBefore(correctedPage, page);
                    break;
                }

                if (correctedPage.Next is null)
                {
                    correctedUpdate.AddLast(page);
                    break;
                }

                correctedPage = correctedPage.Next;
            }
        }

        return [.. correctedUpdate];
    }
}
