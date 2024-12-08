using System.Collections.Frozen;

namespace Year2024.Day05;

public sealed class Day05 : IDay
{
    private FrozenDictionary<int, int[]> _successorsByPredecessor = default!;
    private int[][] _updates = default!;

    public int Day => 5;

    public long DoPart1()
    {
        var correctUpdates = FilterUpdates(_successorsByPredecessor, _updates, true);

        var pageSums = 0;
        foreach (var update in correctUpdates)
        {
            pageSums += update[update.Length / 2];
        }

        return pageSums;
    }

    public long DoPart2()
    {
        var incorrectUpdates = FilterUpdates(_successorsByPredecessor, _updates, false);

        var pageSums = 0;
        foreach (var update in incorrectUpdates)
        {
            var correctedUpdate = CorrectUpdates(_successorsByPredecessor, update);
            pageSums += correctedUpdate[correctedUpdate.Length / 2];
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

    private static IEnumerable<int[]> FilterUpdates(FrozenDictionary<int, int[]> successorsByPredecessor, int[][] updates, bool returnCorrectUpdates)
    {
        foreach (var update in updates)
        {
            var isUpdateCorrect = true;

            foreach (var page in update.Index())
            {
                if (!successorsByPredecessor.TryGetValue(page.Item, out var predecessors))
                {
                    continue;
                }

                foreach (var predecessor in predecessors)
                {
                    var predecessorIndex = Array.IndexOf(update, predecessor);
                    if (predecessorIndex > -1 && predecessorIndex < page.Index)
                    {
                        isUpdateCorrect = false;
                        break;
                    }
                }

                if (!isUpdateCorrect)
                {
                    break;
                }
            }

            if (isUpdateCorrect == returnCorrectUpdates)
            {
                yield return update;
            }
        }
    }

    private static int[] CorrectUpdates(FrozenDictionary<int, int[]> successorsByPredecessor, int[] update)
    {
        var correctedUpdate = new LinkedList<int>();
        correctedUpdate.AddFirst(update[0]);

        foreach (var page in update.Skip(1))
        {
            if (!successorsByPredecessor.TryGetValue(page, out var successors))
            {
                correctedUpdate.AddLast(page);
                continue;
            }

            var correctedPage = correctedUpdate.First;
            while (correctedPage != null)
            {
                if (successors.Contains(correctedPage.Value))
                {
                    correctedUpdate.AddBefore(correctedPage, page);
                    break;
                }

                if (correctedPage.Next == null)
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
