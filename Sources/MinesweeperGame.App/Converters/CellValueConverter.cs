using MinesweeperGame.App.Entities;

namespace MinesweeperGame.App.Converters;

internal static class CellValueCOnverter
{
    public static ConsoleColor ToColor(this int cellValue, bool visible)
    {
        return cellValue switch
        {
            GameCellValue.None or GameMarkedCellValue.None when visible => ConsoleColor.Gray,
            GameCellValue.C1 or GameMarkedCellValue.C1 when visible => ConsoleColor.Blue,
            GameCellValue.C2 or GameMarkedCellValue.C2 when visible => ConsoleColor.Green,
            GameCellValue.C3 or GameMarkedCellValue.C3 when visible => ConsoleColor.Yellow,
            GameCellValue.C4 or GameMarkedCellValue.C4 when visible => ConsoleColor.Magenta,
            GameCellValue.C5 or GameMarkedCellValue.C5 when visible => ConsoleColor.DarkMagenta,
            GameCellValue.C6 or GameMarkedCellValue.C6 when visible => ConsoleColor.DarkRed,
            GameCellValue.C7 or GameMarkedCellValue.C7 when visible => ConsoleColor.DarkYellow,
            GameCellValue.C8 or GameMarkedCellValue.C8 when visible => ConsoleColor.Cyan,
            GameCellValue.Mine or GameMarkedCellValue.Mine when visible => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    public static string ToContent(this int cellValue, bool visible)
    {
        return cellValue switch
        {
            GameCellValue.Mine or GameMarkedCellValue.Mine when visible => GameCellContent.Mine,
            GameCellValue.None or GameMarkedCellValue.None when visible => GameCellContent.None,

            GameMarkedCellValue.C1 when visible => GameCellContent.C1,
            GameMarkedCellValue.C2 when visible => GameCellContent.C2,
            GameMarkedCellValue.C3 when visible => GameCellContent.C3,
            GameMarkedCellValue.C4 when visible => GameCellContent.C4,
            GameMarkedCellValue.C5 when visible => GameCellContent.C5,
            GameMarkedCellValue.C6 when visible => GameCellContent.C6,
            GameMarkedCellValue.C7 when visible => GameCellContent.C7,
            GameMarkedCellValue.C8 when visible => GameCellContent.C8,

            GameMarkedCellValue.None or 
            GameMarkedCellValue.C1 or 
            GameMarkedCellValue.C2 or 
            GameMarkedCellValue.C3 or 
            GameMarkedCellValue.C4 or 
            GameMarkedCellValue.C5 or 
            GameMarkedCellValue.C6 or 
            GameMarkedCellValue.C7 or 
            GameMarkedCellValue.C8 or 
            GameMarkedCellValue.Mine when !visible => GameCellContent.Marked,
            _ when visible => cellValue.ToString(),
            _ => GameCellContent.Unknown,
        };
    }
}
