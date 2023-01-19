using MinesweeperGame.App.Converters;
using MinesweeperGame.App.Entities;

namespace MinesweeperGame.App.Extensions;

internal static class GameConsoleOutputExtensions
{
    public static void Print(this Game game, string title, bool? visibilityOverride = null)
    {
        (int height, int width) = game.GridSize;
        (int[,] gridData, bool[,] gridVisibility) = game.Data;
        Console.WriteLine($"{title}\nMarks: {game.MarksCounter}");

        Console.BackgroundColor = ConsoleColor.Black;
        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                var gridCell = gridData[row, column];
                var cellVisibility = visibilityOverride ?? gridVisibility[row, column];
                PrintCellValue(gridCell, cellVisibility);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    public static void PrintResults(this Game game, string title)
    {
        Console.WriteLine(title);

        var state = game.State;
        switch (state)
        {
            case GameState.Loss:
                game.Print("You lost! Dont worry.", true);
                break;
            case GameState.Win:
                game.Print("Congratulations! You won.", true);
                break;
            default:
                break;
        }
    }

    private static void PrintCellValue(int cellValue, bool visible)
    {
        var cellColor = cellValue.ToColor(visible);
        var cellContent = cellValue.ToContent(visible);

        Console.ForegroundColor = cellColor;
        Console.Write($"{cellContent} ");
    }
}
