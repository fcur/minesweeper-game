namespace MinesweeperGame.App.Entities;

public class GameGridBuilder
{
    private readonly Cell[,] _cells;
    private readonly int _width;
    private readonly int _height;

    private GameGridBuilder(int rows, int columns)
    {
        _cells = new Cell[rows, columns];
        _height = rows;
        _width = columns;
    }

    public GameGridBuilder WithMines(int minesCount)
    {
        var random = new Random();
        do
        {
            var rowIndex = random.Next(_height - 1);
            var columnIndex = random.Next(_width - 1);

            if(TryGetState(rowIndex, columnIndex, out var state) && !state.IsMine)
            {
                ApplyMine(rowIndex, columnIndex);

                minesCount--;
            }

        } while (minesCount > 0);

        return this;
    }

    public Cell[,] GetCells()
    {
        return _cells;
    }

    public static GameGridBuilder Create(int rows, int columns)
    {
        return new(rows, columns);
    }

    private void ApplyMine(int mineRow, int mineColumn)
    {
        _cells[mineRow, mineColumn] = Cell.NewMine;

        var boundaryCellIndexes = PrepareMineBoundary(mineRow, mineColumn);

        for (var i = 0; i < boundaryCellIndexes.GetLength(0); i++)
        {
            var cellRow = boundaryCellIndexes[i, 0];
            var cellColumn = boundaryCellIndexes[i, 1];

            if(!TryGetState(cellRow, cellColumn, out var cellState) || cellState.IsMine)
            {
                continue;
            }

            _cells[cellRow, cellColumn]++;
        }
    }

    private bool TryGetState(int cellRow, int cellColumn, out CellState cellState)
    {
        if (cellRow < 0 || cellRow >= _height || cellColumn < 0 || cellColumn >= _width)
        {
            cellState = CellState.None;
            return false;
        }

        cellState = _cells[cellRow, cellColumn].GetState();
        return true;
    }

    private static int[,] PrepareMineBoundary(int mineRow, int mineColumn)
    {
        var prewRow = mineRow - 1;
        var prewColumn = mineColumn - 1;
        var nextRow = mineRow + 1;
        var nextColumn = mineColumn + 1;

        var boundaryCellIndexes = new int[,] {
            { prewRow, prewColumn }, { prewRow, mineColumn }, { prewRow, nextColumn },
            { mineRow, prewColumn }, { mineRow, nextColumn },
            { nextRow, prewColumn }, { nextRow, mineColumn }, { nextRow, nextColumn },
        };

        return boundaryCellIndexes;
    }
}
