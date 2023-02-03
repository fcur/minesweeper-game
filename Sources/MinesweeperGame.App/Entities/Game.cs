using System.Transactions;

namespace MinesweeperGame.App.Entities;

public sealed class Game
{
    private readonly Cell[,] _cells;
    private int _state;
    private int _minesCounter;
    private int _marksCounter;

    private Game(Cell[,] cells, int minesCount)
    {
        ArgumentNullException.ThrowIfNull(cells);

        _cells = cells;
        _minesCounter = minesCount;
        _marksCounter = minesCount;
        _state = GameState.Initial;
    }

    public (int Heigh, int Width) GridSize => new(_cells.GetLength(0), _cells.GetLength(1));

    public int MarksCounter => _marksCounter;

    public int State => _state;

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
            GameState.Initial => OpenBoundaryCells(row, column),
            GameState.Active when operation == GameCellOperations.Mark => TryToggleCell(row, column),
            GameState.Active when operation == GameCellOperations.Open => TryOpenCell(row, column),
            _ => throw new ArgumentOutOfRangeException(nameof(_state), "Invalid game state")
        };
    }

    public CellState[,] GetCellState()
    {
        (int height, int width) = GridSize;
        var result = new CellState[height, width];

        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                result[row, column] = _cells[row, column].GetState();
            }
        }

        return result;
    }

    public static Game Create(int level)
    {
        (int rows, int columns) = GetGridSize(level);
        var minesCount = CalcMinesCount(columns, rows, level);

        var gridBuilder = GameGridBuilder.Create(rows, columns).WithMines(minesCount);
        var cells = gridBuilder.GetCells();
        return new Game(cells, minesCount);
    }

    public static Game Test(Cell[,] cells, int minesCount)
    {
        return new Game(cells, minesCount);
    }

    private bool AllMinesMarked => _minesCounter == 0;

    private bool AllMarksUsed => _marksCounter == 0;

    private CellState GetCellState(int row, int column) => _cells[row, column].GetState();

    private void SetLoss() => _state = GameState.Loss;

    private void SetWin() => _state = GameState.Win;

    private void SetActive() => _state = GameState.Active;

    private bool TryToggleCell(int row, int column)
    {
        var cellState = GetCellState(row, column);

        if (cellState.IsOpened || AllMarksUsed)
        {
            return true;
        }

        if (cellState.IsMarked)
        {
            TryRestoreMineCounter(cellState.IsMine);
            UnmarkCellImpl(row, column);
            return true;
        }

        MarkCellImpl(row, column);

        if (TryMineAndCheckWin(cellState.IsMine))
        {
            SetWin();
            return false;
        }

        return true;
    }

    private bool TryOpenCell(int row, int column)
    {
        var cellState = GetCellState(row, column);

        if (cellState.IsMarked)
        {
            TryRestoreMineCounter(cellState.IsMine);
            UnmarkCellImpl(row, column);
        }

        if (TryLossIfOpenMine(cellState.IsMine))
        {
            return false;
        }

        OpenCellImpl(row, column);
        return true;
    }

    private bool TryLossIfOpenMine(bool isMine)
    {
        if (!isMine)
        {
            return false;
        }

        SetLoss();
        return true;
    }

    private bool TryMineAndCheckWin(bool isMine)
    {
        if (!isMine)
        {
            return false;
        }

        _minesCounter--;


        if (AllMinesMarked)
        {
            return true;
        }

        return false;
    }

    private void TryRestoreMineCounter(bool isMine)
    {
        if (!isMine)
        {
            return;
        }

        _minesCounter++;
    }

    private bool OpenBoundaryCells(int row, int column)
    {
        var cellState = GetCellState(row, column);

        if (TryLossIfOpenMine(cellState.IsMine))
        {
            return false;
        }

        var counter = _minesCounter;
        var deep = 1;
        (int height, int width) = GridSize;

        do
        {
            for (var boundRow = Math.Max(0, row - deep); boundRow < Math.Min(row + deep, height); boundRow++)
            {
                for (var boundColumn = Math.Max(0, column - deep); boundColumn < Math.Min(column + deep, width); boundColumn++)
                {
                    cellState = GetCellState(boundRow, boundColumn);
                    var isVisible = cellState.IsOpened;
                    var isMine = cellState.IsMine;

                    if (isVisible || isMine || counter < 1)
                    {
                        continue;
                    }

                    OpenCellImpl(boundRow, boundColumn);
                    counter--;
                }
            }

            deep++;

        } while (counter > 0);

        SetActive();
        return true;
    }

    private void MarkCellImpl(int row, int column)
    {
        if (_cells[row, column].TryMark(out var cellValue))
        {
            _cells[row, column] = cellValue;
            _marksCounter--;
        }
    }

    private void UnmarkCellImpl(int row, int column)
    {
        if (_cells[row, column].TryUnmark(out var cellValue))
        {
            _cells[row, column] = cellValue;
            _marksCounter++;
        }
    }

    private void OpenCellImpl(int row, int column)
    {
        if (_cells[row, column].TryOpen(out var cellValue))
        {
            _cells[row, column] = cellValue;
        }
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
}
