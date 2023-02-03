using MinesweeperGame.App.Entities;

namespace MinesweeperGame.App.Converters;

internal static class CellValueCOnverter
{
    public static (string Value, ConsoleColor Color) ToColoredValue(this CellState cellState, bool? visibilityOverride = null)
    {
        var visible = visibilityOverride ?? cellState.IsOpened;
        var cellValue = cellState.Value;
        var isMarked = cellState.IsMarked;

        var value = cellValue switch
        {
            GameCellValue.None when visible => GameCellContent.None,
            GameCellValue.Mine when visible => GameCellContent.Mine,
            _ when isMarked => GameCellContent.Marked,
            _ when visible => cellValue.ToString(),
            _ => GameCellContent.Unknown,
        };

        var color = cellValue switch
        {
            GameCellValue.None when visible => ConsoleColor.Gray,
            GameCellValue.C1 when visible => ConsoleColor.Blue,
            GameCellValue.C2 when visible => ConsoleColor.Green,
            GameCellValue.C3 when visible => ConsoleColor.Yellow,
            GameCellValue.C4 when visible => ConsoleColor.Magenta,
            GameCellValue.C5 when visible => ConsoleColor.DarkMagenta,
            GameCellValue.C6 when visible => ConsoleColor.DarkRed,
            GameCellValue.C7 when visible => ConsoleColor.DarkYellow,
            GameCellValue.C8 when visible => ConsoleColor.Cyan,
            GameCellValue.Mine when visible => ConsoleColor.Red,
            _ when isMarked => ConsoleColor.DarkCyan,
            _ => ConsoleColor.White
        };

        return new(value, color);
    }
}
