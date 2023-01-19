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

    public const string C1 = "1";
    public const string C2 = "2";
    public const string C3 = "3";
    public const string C4 = "4";
    public const string C5 = "5";
    public const string C6 = "6";
    public const string C7 = "7";
    public const string C8 = "8";
}

public static class GameCellValue
{
    public const int None = 0;
    public const int C1 = 1;
    public const int C2 = 2;
    public const int C3 = 3;
    public const int C4 = 4;
    public const int C5 = 5;
    public const int C6 = 6;
    public const int C7 = 7;
    public const int C8 = 8;
    public const int Mine = 9;
    public const int MarkMineShift = 10;
}

public static class GameMarkedCellValue
{
    public const int None = 10;
    public const int C1 = 11;
    public const int C2 = 12;
    public const int C3 = 13;
    public const int C4 = 14;
    public const int C5 = 15;
    public const int C6 = 16;
    public const int C7 = 17;
    public const int C8 = 18;
    public const int Mine = 19;
}

public static class GameState
{
    public const int Loss = -1;
    public const int FirstStep = 0;
    public const int Active = 1;
    public const int Win = 2;
}