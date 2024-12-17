namespace Year2024.Day15;

public sealed class Day15 : IDay
{
    private Grid<int> _mapPart1 = default!;
    private Position _robotPart1 = default!;

    private Grid<int> _mapPart2 = default!;
    private Position _robotPart2 = default!;

    private int[] _steps = default!;

    public int Day => 15;

    public string DoPart1()
    {
        DoSteps(_mapPart1, _steps, _robotPart1);

        var sum = 0L;

        for (var r = 0; r < _mapPart1.Rows; r++)
        {
            for (var c = 0; c < _mapPart1.Columns; c++)
            {
                if (_mapPart1[r, c] == Objects.Box)
                {
                    sum += (100 * r) + c;
                }
            }
        }

        return sum.ToString();
    }

    public string DoPart2()
    {
        DoSteps(_mapPart2, _steps, _robotPart2);

        var sum = 0L;

        for (var r = 0; r < _mapPart2.Rows; r++)
        {
            for (var c = 0; c < _mapPart2.Columns; c++)
            {
                if (_mapPart2[r, c] == Objects.WideBoxLeft)
                {
                    sum += (100 * r) + c;
                }
            }
        }

        return sum.ToString();
    }

    public void PrepareInput()
    {
        var parts = Inputs.Input.Split($"{Environment.NewLine}{Environment.NewLine}");

        _mapPart1 = Grid<int>.Parse(parts[0], c => c switch
        {
            '.' => Objects.Empty,
            '#' => Objects.Wall,
            'O' => Objects.Box,
            '@' => Objects.Robot,
            _ => throw new InvalidOperationException()
        });

        _mapPart2 = new Grid<int>(new int[_mapPart1.Rows, _mapPart1.Columns * 2]);

        for (var r = 0; r < _mapPart1.Rows; r++)
        {
            for (var c = 0; c < _mapPart1.Columns; c++)
            {
                _mapPart2[r, c * 2] = _mapPart1[r, c] switch
                {
                    Objects.Empty => Objects.Empty,
                    Objects.Wall => Objects.Wall,
                    Objects.Box => Objects.WideBoxLeft,
                    Objects.Robot => Objects.Empty,
                    _ => throw new InvalidOperationException()
                };

                _mapPart2[r, (c * 2) + 1] = _mapPart1[r, c] switch
                {
                    Objects.Empty => Objects.Empty,
                    Objects.Wall => Objects.Wall,
                    Objects.Box => Objects.WideBoxRight,
                    Objects.Robot => Objects.Empty,
                    _ => throw new InvalidOperationException()
                };

                if (_mapPart1[r, c] == Objects.Robot)
                {
                    _robotPart1 = new Position(r, c);
                    _mapPart1[r, c] = Objects.Empty;

                    _robotPart2 = new Position(r, c * 2);
                    _mapPart2[r, c * 2] = Objects.Empty;
                }
            }
        }

        _steps = parts[1]
            .Where(c => !char.IsWhiteSpace(c))
            .Select(c => c switch
            {
                '^' => Directions.Up,
                '>' => Directions.Right,
                'v' => Directions.Down,
                '<' => Directions.Left,
                _ => throw new InvalidOperationException()
            })
            .ToArray();
    }

    private static void DoSteps(Grid<int> map, int[] steps, Position position)
    {
        for (var s = 0; s < steps.Length; s++)
        {
            var direction = steps[s];
            var nextPosition = GetNextPosition(position, direction);

            position = map[nextPosition] switch
            {
                Objects.Empty => nextPosition,
                Objects.Wall => position,
                Objects.Box => TryPushSmallBox(map, position, nextPosition, direction),
                Objects.WideBoxLeft or Objects.WideBoxRight => TryPushWideBox(map, position, nextPosition, direction),
                _ => throw new InvalidOperationException()
            };
        }
    }

    private static Position TryPushSmallBox(Grid<int> map, Position position, Position boxPosition, int direction)
    {
        var nextNonBoxPosition = GetNextPosition(boxPosition, direction);
        while (map[nextNonBoxPosition] is Objects.Box)
        {
            nextNonBoxPosition = GetNextPosition(nextNonBoxPosition, direction);
        }

        if (map[nextNonBoxPosition] is Objects.Empty)
        {
            map[boxPosition] = Objects.Empty;
            map[nextNonBoxPosition] = Objects.Box;
            return boxPosition;
        }

        return position;
    }

    private static Position TryPushWideBox(Grid<int> map, Position position, Position boxPosition, int direction)
    {
        if (CanWideBoxMove(map, boxPosition, direction))
        {
            MoveWideBox(map, boxPosition, direction);
            return boxPosition;
        }

        return position;
    }

    private static bool CanWideBoxMove(Grid<int> map, Position boxPosition, int direction)
    {
        var leftBoxSide = map[boxPosition] is Objects.WideBoxLeft ? boxPosition : boxPosition.Left;
        var rightBoxSide = leftBoxSide.Right;

        if (direction is Directions.Left)
        {
            var nextPositionLeft = GetNextPosition(leftBoxSide, direction);
            return map[nextPositionLeft] switch
            {
                Objects.Empty => true,
                Objects.WideBoxLeft or Objects.WideBoxRight => CanWideBoxMove(map, nextPositionLeft, direction),
                _ => false
            };
        }
        else if (direction is Directions.Right)
        {
            var nextPositionRight = GetNextPosition(rightBoxSide, direction);
            return map[nextPositionRight] switch
            {
                Objects.Empty => true,
                Objects.WideBoxLeft or Objects.WideBoxRight => CanWideBoxMove(map, nextPositionRight, direction),
                _ => false
            };
        }
        else if (direction is Directions.Up or Directions.Down)
        {
            var nextPositionLeft = GetNextPosition(leftBoxSide, direction);
            var nextPositionRight = GetNextPosition(rightBoxSide, direction);

            var canLeftMove = map[nextPositionLeft] switch
            {
                Objects.Empty => true,
                Objects.WideBoxLeft or Objects.WideBoxRight => CanWideBoxMove(map, nextPositionLeft, direction),
                _ => false
            };

            var canRightMove = map[nextPositionRight] switch
            {
                Objects.Empty => true,
                Objects.WideBoxLeft or Objects.WideBoxRight => CanWideBoxMove(map, nextPositionRight, direction),
                _ => false
            };

            return canLeftMove && canRightMove;
        }

        return false;
    }

    private static void MoveWideBox(Grid<int> map, Position boxPosition, int direction)
    {
        if (map[boxPosition] is Objects.Empty)
        {
            return;
        }
        else if (map[boxPosition] is Objects.Wall)
        {
            throw new InvalidOperationException();
        }

        var leftBoxSide = map[boxPosition] is Objects.WideBoxLeft ? boxPosition : boxPosition.Left;
        var rightBoxSide = leftBoxSide.Right;

        if (direction is Directions.Left)
        {
            var nextPositionLeft = GetNextPosition(leftBoxSide, direction);

            MoveWideBox(map, nextPositionLeft, direction);

            map[nextPositionLeft] = Objects.WideBoxLeft;
            map[leftBoxSide] = Objects.WideBoxRight;
            map[rightBoxSide] = Objects.Empty;
        }
        else if (direction is Directions.Right)
        {
            var nextPositionRight = GetNextPosition(rightBoxSide, direction);

            MoveWideBox(map, nextPositionRight, direction);

            map[nextPositionRight] = Objects.WideBoxRight;
            map[rightBoxSide] = Objects.WideBoxLeft;
            map[leftBoxSide] = Objects.Empty;
        }
        else if (direction is Directions.Up or Directions.Down)
        {
            var nextPositionLeft = GetNextPosition(leftBoxSide, direction);
            var nextPositionRight = GetNextPosition(rightBoxSide, direction);

            MoveWideBox(map, nextPositionLeft, direction);
            if (map[nextPositionRight] is not Objects.Empty)
            {
                MoveWideBox(map, nextPositionRight, direction);
            }

            map[nextPositionLeft] = Objects.WideBoxLeft;
            map[nextPositionRight] = Objects.WideBoxRight;
            map[leftBoxSide] = Objects.Empty;
            map[rightBoxSide] = Objects.Empty;
        }
    }

    private static Position GetNextPosition(Position position, int direction)
        => direction switch
        {
            Directions.Up => position.Up,
            Directions.Right => position.Right,
            Directions.Down => position.Down,
            Directions.Left => position.Left,
            _ => throw new InvalidOperationException()
        };
}
