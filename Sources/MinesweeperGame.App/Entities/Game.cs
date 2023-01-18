using MinesweeperGame.App.Converters;

namespace MinesweeperGame.App.Entities;

public sealed class Game
{
    private readonly int[,] _gridData;
    private readonly bool[,] _gridVisibility;
    private int _state;
    private int _minesCounter;
    private int _marksCounter;

    private Game(int[,] gridData, bool[,] gridVisibility, int minesCount)
    {
        ArgumentNullException.ThrowIfNull(gridData);
        ArgumentNullException.ThrowIfNull(gridVisibility);

        _gridData = gridData;
        _gridVisibility = gridVisibility;
        _minesCounter = minesCount;
        _marksCounter = minesCount;
        _state = GameState.Active;
    }

    public static Game Create(int level)
    {
        (int rows, int columns) = GetGridSize(level);
        var minesCount = CalcMinesCount(columns, rows, level);
        var gridBuilder = GameGridBuilder.Create(rows, columns).WithMines(minesCount);

        var grid = gridBuilder.GetGrid();
        var gridVisibility = new bool[rows, columns];
        return new Game(grid, gridVisibility, minesCount);
    }

    public bool IsValidCoordinates(int row, int column)
    {
        var height = _gridData.GetLength(0);
        var witdh = _gridData.GetLength(1);

        return InRange(row, -1, height) && InRange(column, -1, witdh);
    }

    public bool MoveNext(char operation, int row, int column)
    {
        if (!IsValidCoordinates(row, column))
        {
            SetLoss();
            return false;
        }

        switch (_state)
        {
            case GameState.Loss:
                Print("You lost! Dont worry.");
                return false;
            case GameState.Active when operation == GameCellOperations.Mark:
                return TryMarkMine(row, column);
            case GameState.Active when operation == GameCellOperations.Open:
                return TryOpenCell(row, column);
            case GameState.Win:
                Print("Congratulations! You won.");
                return false;
            default:
                SetLoss();
                return false;
        }
    }

    public void PrintResults(string title)
    {
        Console.WriteLine(title);

        switch (_state)
        {
            case GameState.Loss:
                Print("You lost! Dont worry.", true);
                break;
            case GameState.Win:
                Print("Congratulations! You won.", true);
                break;
            default:
                break;
        }
    }

    public void Print(string title, bool? visibility = null)
    {
        Console.WriteLine($"{title}\nMarks: {_marksCounter}");

        var height = _gridData.GetLength(0);
        var witdh = _gridData.GetLength(1);

        Console.BackgroundColor = ConsoleColor.Black;
        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < witdh; column++)
            {
                var gridCell = _gridData[row, column];
                var cellVisibility = visibility ?? _gridVisibility[row, column];
                PrintCellValue(gridCell, cellVisibility);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    private bool AllMinesAreMarked => _minesCounter == 0;

    private bool AllMarksUsed => _marksCounter == 0;

    private void SetLoss()
    {
        _state = GameState.Loss;
    }

    private void SetWin()
    {
        _state = GameState.Win;
    }

    private bool TryMarkMine(int row, int column)
    {
        if (CellIsMarked(_gridData[row, column]))
        {
            return UnmarkCell(row, column);
        }

        return MarkCell(row, column);
    }

    private bool TryOpenCell(int row, int column)
    {
        if (CellIsMarked(_gridData[row, column]))
        {
            UnmarkCell(row, column);
        }

        if (CellIsMine(_gridData[row, column]))
        {
            SetLoss();
            return false;
        }

        _gridVisibility[row, column] = true;
        return true;
    }

    private bool MarkCell(int row, int column)
    {
        if (CellIsMine(_gridData[row, column]))
        {
            _minesCounter--;
        }

        if (AllMinesAreMarked)
        {
            SetWin();
            return false;
        }

        if (AllMarksUsed)
        {
            return true;
        }

        _gridData[row, column] += GameCellValue.MarkMineShift;
        _marksCounter--;

        return true;
    }

    private bool UnmarkCell(int row, int column)
    {
        if (CellIsMarkedMine(_gridData[row, column]))
        {
            _minesCounter++;
        }

        _gridData[row, column] -= GameCellValue.MarkMineShift;
        _marksCounter++;

        return true;
    }

    private static void PrintCellValue(int cellValue, bool visible)
    {
        var cellColor = cellValue.ToColor(visible);
        var cellContent = cellValue.ToContent(visible);

        Console.ForegroundColor = cellColor;
        Console.Write($"{cellContent} ");
    }

    private static (int rows, int columns) GetGridSize(int level) => level switch
    {
        GameLevel.Begginer => (8, 8),
        GameLevel.Normal => (14, 14),
        GameLevel.Expert => (20, 20),
        _ => throw new ArgumentOutOfRangeException(nameof(level))
    };

    private static int CalcMinesCount(int columns, int rows, int level)
    {
        var minesCoef = level switch
        {
            GameLevel.Begginer => GameMinesCoef.Beginner,
            GameLevel.Normal => GameMinesCoef.Normal,
            GameLevel.Expert => GameMinesCoef.Expert,
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };

        var minesCount = (int)(rows * columns * minesCoef + 0.5f);

        return minesCount;
    }

    private static bool InRange(int value, int min, int max) => value > min && value < max;

    private static bool CellIsMarked(int cellValue) => cellValue >= GameCellValue.MarkMineShift;

    private static bool CellIsMine(int cellValue) => cellValue == GameCellValue.Mine;

    private static bool CellIsMarkedMine(int cellValue) => cellValue == GameMarkedCellValue.Mine;
}
