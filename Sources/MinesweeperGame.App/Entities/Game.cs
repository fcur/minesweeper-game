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
        _state = GameState.FirstStep;
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
        (int height, int width) = GridSize;
        return InRange(row, -1, height) && InRange(column, -1, width);
    }

    public bool MoveNext(char operation, int row, int column)
    {
        if (!IsValidCoordinates(row, column))
        {
            SetLoss();
            return false;
        }

        return _state switch
        {
            GameState.Loss or GameState.Win => false,
            GameState.FirstStep => OpenBoundaryCells(row, column),
            GameState.Active when operation == GameCellOperations.Mark => TryToggleCell(row, column),
            GameState.Active when operation == GameCellOperations.Open => TryOpenCell(row, column),
            _ => throw new ArgumentOutOfRangeException(nameof(_state), "Invalid game state")
        };
    }

    public (int[,] GridData, bool[,] GridVisibility) Data => new(_gridData, _gridVisibility);

    public (int Heigh, int Width) GridSize => new(_gridData.GetLength(0), _gridData.GetLength(1));

    public int MarksCounter => _marksCounter;

    public int State => _state;

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

    private void SetActive()
    {
        _state = GameState.Active;
    }

    private bool TryToggleCell(int row, int column)
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

    private bool OpenBoundaryCells(int row, int column)
    {
        if (CellIsMine(_gridData[row, column]))
        {
            SetLoss();
            return false;
        }

        var counter = _minesCounter;
        var deep = 1;
        (int height, int width) = GridSize;

        do
        {
            for (var x = Math.Max(0, column - deep); x < Math.Min(column + deep, width); x++)
            {
                for (var y = Math.Max(0, row - deep); y < Math.Min(row + deep, height); y++)
                {
                    var visible = _gridVisibility[x, y];
                    var data = _gridData[x, y];

                    if (visible || CellIsMine(data) || counter < 1)
                    {
                        continue;
                    }

                    _gridVisibility[x, y] = true;
                    counter--;
                }
            }

            deep++;

        } while (counter > 0);

        SetActive();
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
