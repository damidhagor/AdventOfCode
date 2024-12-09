namespace Year2024.Day09;

public sealed class Day09 : IDay
{
    private int[] _diskMap = default!;

    public int Day => 9;

    public long DoPart1()
    {
        var disk = GetDisk(_diskMap);

        DefragDiskBlocks(disk);

        return CalculateChecksum(disk);
    }

    public long DoPart2()
    {
        var disk = GetDisk(_diskMap);

        DefragDiskFiles(disk);

        return CalculateChecksum(disk);
    }

    public void PrepareInput()
    {
        var input = Inputs.DiskMap;
        _diskMap = new int[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            _diskMap[i] = input[i] - '0';
        }
    }

    private static int[] GetDisk(int[] diskMap)
    {
        var diskSize = 0;
        for (var i = 0; i < diskMap.Length; i++)
        {
            diskSize += diskMap[i];
        }

        var disk = new int[diskSize];
        bool blocksAreFree = false;
        var diskIndex = 0;
        var blockId = 0;
        for (var i = 0; i < diskMap.Length; i++)
        {
            var value = blocksAreFree ? -1 : blockId++;

            for (var j = 0; j < diskMap[i]; j++)
            {
                disk[diskIndex++] = value;
            }

            blocksAreFree = !blocksAreFree;
        }

        return disk;
    }

    private static void DefragDiskBlocks(int[] disk)
    {
        var readIndex = disk.Length - 1;
        var writeIndex = Array.IndexOf(disk, -1);

        while (readIndex > writeIndex)
        {
            disk[writeIndex] = disk[readIndex];
            disk[readIndex] = -1;

            readIndex--;
            for (; disk[writeIndex] != -1; writeIndex++) ;
        }
    }

    private static void DefragDiskFiles(int[] disk)
    {
        var attemptedBlockIds = new HashSet<int>();

        var readIndexStart = disk.Length;
        var readIndexEnd = disk.Length;

        while (readIndexEnd >= 0)
        {
            for (readIndexEnd = readIndexStart - 1; readIndexEnd >= 0 && disk[readIndexEnd] == -1; readIndexEnd--) ;
            for (readIndexStart = readIndexEnd; readIndexStart >= 1 && disk[readIndexStart - 1] == disk[readIndexEnd]; readIndexStart--) ;

            if (readIndexEnd < 0 || !attemptedBlockIds.Add(disk[readIndexEnd]))
            {
                continue;
            }

            var readSize = readIndexEnd - readIndexStart + 1;

            var writeIndexStart = 0;
            var writeIndexEnd = -1;
            var writeSize = 0;

            while (readSize > writeSize && writeIndexStart < readIndexStart)
            {
                for (writeIndexStart = writeIndexEnd + 1; writeIndexStart < disk.Length && disk[writeIndexStart] != -1; writeIndexStart++) ;
                for (writeIndexEnd = writeIndexStart; writeIndexEnd < disk.Length - 1 && disk[writeIndexEnd + 1] == -1; writeIndexEnd++) ;

                writeSize = writeIndexEnd - writeIndexStart + 1;
            }

            if (readSize > writeSize || writeIndexStart >= readIndexStart)
            {
                continue;
            }

            for (var i = 0; i < readSize; i++)
            {
                disk[writeIndexStart + i] = disk[readIndexStart + i];
                disk[readIndexStart + i] = -1;
            }
        }
    }

    private static long CalculateChecksum(int[] disk)
    {
        var checksum = 0L;
        for (var i = 0; i < disk.Length; i++)
        {
            if (disk[i] == -1)
            {
                continue;
            }

            checksum += disk[i] * i;
        }

        return checksum;
    }
}
