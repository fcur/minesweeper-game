namespace MinesweeperGame.App.Entities;

public static class GameLevel
{
    public const int Begginer = 1;
    public const int Normal = 2;
    public const int Expert = 3;
}

public static class GameMinesCoef
{
    public const float Beginner = 0.15f;
    public const float Normal = 0.2f;
    public const float Expert = 0.25f;
}

public static class GameCellOperations
{
    public const char Mark = 'M';
    public const char Open = 'O';
}

public static class GameCellContent
{
    public const string Mine = "X";
    public const string None = "#";
    public const string Marked = "V";
    public const string Unknown = "?";

    public const string Cell1 = "1";
    public const string Cell2 = "2";
    public const string Cell3 = "3";
    public const string Cell4 = "4";
    public const string Cell5 = "5";
    public const string Cell6 = "6";
    public const string Cell7 = "7";
    public const string Cell8 = "8";
}

public static class GameCellValue
{
    public const int None = 0;
    public const int Cell1 = 1;
    public const int Cell2 = 2;
    public const int Cell3 = 3;
    public const int Cell4 = 4;
    public const int Cell5 = 5;
    public const int Cell6 = 6;
    public const int Cell7 = 7;
    public const int Cell8 = 8;
    public const int Mine = 9;
    public const int MarkMineShift = 10;
}

public static class GameMarkedCellValue
{
    public const int None = 10;
    public const int Cell1 = 11;
    public const int Cell2 = 12;
    public const int Cell3 = 13;
    public const int Cell4 = 14;
    public const int Cell5 = 15;
    public const int Cell6 = 16;
    public const int Cell7 = 17;
    public const int Cell8 = 18;
    public const int Mine = 19;
}

public static class GameState
{
    public const int Loss = -1;
    public const int Active = 0;
    public const int Win = 1;
}