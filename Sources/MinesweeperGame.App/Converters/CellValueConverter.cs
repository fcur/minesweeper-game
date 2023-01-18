using MinesweeperGame.App.Entities;

namespace MinesweeperGame.App.Converters;

internal static class CellValueCOnverter
{
    public static ConsoleColor ToColor(this int cellValue, bool visible)
    {
        return cellValue switch
        {
            GameCellValue.None or GameMarkedCellValue.None when visible => ConsoleColor.Gray,
            GameCellValue.Cell1 or GameMarkedCellValue.Cell1 when visible => ConsoleColor.Blue,
            GameCellValue.Cell2 or GameMarkedCellValue.Cell2 when visible => ConsoleColor.Green,
            GameCellValue.Cell3 or GameMarkedCellValue.Cell3 when visible => ConsoleColor.Yellow,
            GameCellValue.Cell4 or GameMarkedCellValue.Cell4 when visible => ConsoleColor.Magenta,
            GameCellValue.Cell5 or GameMarkedCellValue.Cell5 when visible => ConsoleColor.DarkMagenta,
            GameCellValue.Cell6 or GameMarkedCellValue.Cell6 when visible => ConsoleColor.DarkRed,
            GameCellValue.Cell7 or GameMarkedCellValue.Cell7 when visible => ConsoleColor.DarkYellow,
            GameCellValue.Cell8 or GameMarkedCellValue.Cell8 when visible => ConsoleColor.Cyan,
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

            GameMarkedCellValue.Cell1 when visible => GameCellContent.Cell1,
            GameMarkedCellValue.Cell2 when visible => GameCellContent.Cell2,
            GameMarkedCellValue.Cell3 when visible => GameCellContent.Cell3,
            GameMarkedCellValue.Cell4 when visible => GameCellContent.Cell4,
            GameMarkedCellValue.Cell5 when visible => GameCellContent.Cell5,
            GameMarkedCellValue.Cell6 when visible => GameCellContent.Cell6,
            GameMarkedCellValue.Cell7 when visible => GameCellContent.Cell7,
            GameMarkedCellValue.Cell8 when visible => GameCellContent.Cell8,

            GameMarkedCellValue.None or 
            GameMarkedCellValue.Cell1 or 
            GameMarkedCellValue.Cell2 or 
            GameMarkedCellValue.Cell3 or 
            GameMarkedCellValue.Cell4 or 
            GameMarkedCellValue.Cell5 or 
            GameMarkedCellValue.Cell6 or 
            GameMarkedCellValue.Cell7 or 
            GameMarkedCellValue.Cell8 or 
            GameMarkedCellValue.Mine when !visible => GameCellContent.Marked,
            _ when visible => cellValue.ToString(),
            _ => GameCellContent.Unknown,
        };
    }
}
