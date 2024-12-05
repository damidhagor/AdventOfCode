namespace Year2024.Day05;

public static class Day05
{
    public static void DoPart1()
    {
        int pageSums = 0;

        var (successorsByPredecessor, updates) = ParseInput(Inputs.Rules, Inputs.Updates);

        var correctUpdates = FilterUpdates(successorsByPredecessor, updates, true);

        foreach (var update in correctUpdates)
        {
            pageSums += update[update.Length / 2];
        }

        Console.WriteLine($"Day05 Part 1: {pageSums}");
    }

    public static void DoPart2()
    {
        int pageSums = 0;

        var (successorsByPredecessor, updates) = ParseInput(Inputs.Rules, Inputs.Updates);

        var incorrectUpdates = FilterUpdates(successorsByPredecessor, updates, false);

        foreach (var update in incorrectUpdates)
        {
            var correctedUpdate = CorrectUpdates(successorsByPredecessor, update);
            pageSums += correctedUpdate[correctedUpdate.Length / 2];
        }

        Console.WriteLine($"Day05 Part 2: {pageSums}");
    }

    private static IEnumerable<int[]> FilterUpdates(Dictionary<int, int[]> successorsByPredecessor, int[][] updates, bool returnCorrectUpdates)
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

    private static int[] CorrectUpdates(Dictionary<int, int[]> successorsByPredecessor, int[] update)
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

    private static (Dictionary<int, int[]> SuccessorsByPredecessor, int[][] updates) ParseInput(string rulesInput, string updatesInput)
    {
        var successorsByPredecessor = rulesInput
            .Split(Environment.NewLine)
            .Select(line => line.Split('|'))
            .GroupBy(rule => rule[0])
            .ToDictionary(g => int.Parse(g.Key), g => g.Select(n => int.Parse(n[1])).ToArray());

        var updates = updatesInput
            .Split(Environment.NewLine)
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        return (successorsByPredecessor, updates);
    }
}
