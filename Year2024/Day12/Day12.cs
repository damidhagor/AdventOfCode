using Plots = Year2024.Shared.Grid<(char Plant, bool Visited)>;
using Region = System.Collections.Generic.HashSet<Year2024.Shared.Position>;

namespace Year2024.Day12;

public sealed class Day12 : IDay
{
    private Plots _plotsPart1 = default!;
    private Plots _plotsPart2 = default!;

    public int Day => 12;

    public long DoPart1()
    {
        var plots = _plotsPart1;

        var regions = ExplorePlots(plots);

        var totalPrice = 0L;

        for (var r = 0; r < regions.Count; r++)
        {
            var region = regions[r];

            var perimeter = 0;
            foreach (var plot in region)
            {
                if (!region.Contains(plot.Up))
                {
                    perimeter++;
                }

                if (!plots.IsInBounds(plot.Right) || plots[plot.Right].Plant != plots[plot].Plant)
                {
                    perimeter++;
                }

                if (!plots.IsInBounds(plot.Down) || plots[plot.Down].Plant != plots[plot].Plant)
                {
                    perimeter++;
                }

                if (!plots.IsInBounds(plot.Left) || plots[plot.Left].Plant != plots[plot].Plant)
                {
                    perimeter++;
                }
            }

            totalPrice += region.Count * perimeter;
        }

        return totalPrice;
    }

    public long DoPart2()
    {
        var regions = ExplorePlots(_plotsPart2);

        var totalPrice = 0L;

        for (var r = 0; r < regions.Count; r++)
        {
            var region = regions[r];

            var sides = WalkPerimeter(region);

            totalPrice += region.Count * sides;
        }

        return totalPrice;
    }

    public void PrepareInput()
    {
        _plotsPart1 = Grid<(char, bool)>.Parse(Inputs.Map, c => (c, false));
        _plotsPart2 = Grid<(char, bool)>.Parse(Inputs.Map, c => (c, false));
    }

    private static List<Region> ExplorePlots(Plots plots)
    {
        var regions = new List<Region>();

        for (var r = 0; r < plots.Rows; r++)
        {
            for (var c = 0; c < plots.Columns; c++)
            {
                var position = new Position(r, c);
                var (plant, visited) = plots[position];

                if (!visited)
                {
                    var region = new Region();
                    ExploreRegion(plots, position, plant, region);
                    regions.Add(region);
                }
            }
        }

        return regions;
    }

    private static void ExploreRegion(Plots plots, Position position, char plant, Region region)
    {
        if (!plots.IsInBounds(position))
        {
            return;
        }

        var plot = plots[position];

        if (plot.Visited || plot.Plant != plant)
        {
            return;
        }

        plots[position] = (plot.Plant, true);
        region.Add(position);

        ExploreRegion(plots, position.Up, plant, region);
        ExploreRegion(plots, position.Right, plant, region);
        ExploreRegion(plots, position.Down, plant, region);
        ExploreRegion(plots, position.Left, plant, region);
    }

    private static int WalkPerimeter(Region region)
    {
        if (region.Count < 3)
        {
            return 4;
        }

        var unvisitedStartPlots = new HashSet<Position>();
        foreach (var plot in region)
        {
            if (!region.Contains(plot.Left))
            {
                unvisitedStartPlots.Add(plot);
            }
        }

        var totalSides = 0;
        while (unvisitedStartPlots.Count > 0)
        {
            var sides = 0;
            var startPlot = unvisitedStartPlots.First();
            unvisitedStartPlots.Remove(startPlot);

            var currentPlot = startPlot;
            var currentDirection = Directions.Up;
            var canMove = true;

            // Walk perimeter and hug the region to the left
            while (canMove)
            {
                var isLeftInRegion = currentDirection switch
                {
                    Directions.Up => region.Contains(currentPlot.Left),
                    Directions.Right => region.Contains(currentPlot.Up),
                    Directions.Down => region.Contains(currentPlot.Right),
                    Directions.Left => region.Contains(currentPlot.Down),
                    _ => throw new InvalidOperationException(nameof(currentDirection))
                };

                if (isLeftInRegion)
                {
                    currentDirection = TurnLeft(currentDirection);
                    sides++;
                }
                else
                {
                    var isFrontInRegion = currentDirection switch
                    {
                        Directions.Up => region.Contains(currentPlot.Up),
                        Directions.Right => region.Contains(currentPlot.Right),
                        Directions.Down => region.Contains(currentPlot.Down),
                        Directions.Left => region.Contains(currentPlot.Left),
                        _ => throw new InvalidOperationException(nameof(currentDirection))
                    };

                    if (!isFrontInRegion)
                    {
                        var isRightInRegion = currentDirection switch
                        {
                            Directions.Up => region.Contains(currentPlot.Right),
                            Directions.Right => region.Contains(currentPlot.Down),
                            Directions.Down => region.Contains(currentPlot.Left),
                            Directions.Left => region.Contains(currentPlot.Up),
                            _ => throw new InvalidOperationException(nameof(currentDirection))
                        };

                        if (!isRightInRegion)
                        {
                            if (!region.Contains(currentPlot.Left)
                                && (currentDirection == Directions.Left))
                            {
                                unvisitedStartPlots.Remove(currentPlot);
                            }

                            currentDirection = TurnAround(currentDirection);
                            sides += 2;
                        }
                        else
                        {
                            currentDirection = TurnRight(currentDirection);
                            sides++;
                        }

                        if (!region.Contains(currentPlot.Left) && currentDirection == Directions.Up)
                        {
                            unvisitedStartPlots.Remove(currentPlot);
                        }
                    }
                }

                currentPlot = MoveForward(currentPlot, currentDirection);

                if (!region.Contains(currentPlot.Left) && currentDirection == Directions.Up)
                {
                    unvisitedStartPlots.Remove(currentPlot);
                }

                if (startPlot == currentPlot)
                {
                    canMove = currentDirection switch
                    {
                        Directions.Up => region.Contains(currentPlot.Left),
                        Directions.Right => region.Contains(currentPlot.Up),
                        Directions.Down => region.Contains(currentPlot.Right),
                        Directions.Left => region.Contains(currentPlot.Down),
                        _ => throw new InvalidOperationException(nameof(currentDirection))
                    };

                    if (!canMove)
                    {
                        sides += currentDirection switch
                        {
                            Directions.Up => 0,
                            Directions.Down => 2,
                            Directions.Left => 1,
                            _ => throw new InvalidOperationException(nameof(currentDirection))
                        };
                    }
                }
            }

            totalSides += sides;
        }

        return totalSides;
    }

    public static Position MoveForward(Position position, int direction)
        => direction switch
        {
            Directions.Up => position.Up,
            Directions.Right => position.Right,
            Directions.Down => position.Down,
            Directions.Left => position.Left,
            _ => throw new InvalidOperationException(nameof(direction))
        };

    public static int TurnLeft(int direction)
        => direction switch
        {
            Directions.Up => Directions.Left,
            Directions.Right => Directions.Up,
            Directions.Down => Directions.Right,
            Directions.Left => Directions.Down,
            _ => throw new InvalidOperationException(nameof(direction))
        };

    public static int TurnRight(int direction)
        => direction switch
        {
            Directions.Up => Directions.Right,
            Directions.Right => Directions.Down,
            Directions.Down => Directions.Left,
            Directions.Left => Directions.Up,
            _ => throw new InvalidOperationException(nameof(direction))
        };

    public static int TurnAround(int direction)
        => direction switch
        {
            Directions.Up => Directions.Down,
            Directions.Right => Directions.Left,
            Directions.Down => Directions.Up,
            Directions.Left => Directions.Right,
            _ => throw new InvalidOperationException(nameof(direction))
        };
}
