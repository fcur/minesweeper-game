namespace MinesweeperGame.App.Entities;

public class GameGridBuilder
{
    private readonly int[,] _grid;
    private readonly int _width;
    private readonly int _height;

    private GameGridBuilder(int rows, int columns)
    {
        _grid = new int[rows, columns];
        _height = rows;
        _width = columns;
    }

    public static GameGridBuilder Create(int rows, int columns)
    {
        return new(rows, columns);
    }

    public GameGridBuilder WithMines(int minesCount)
    {
        AddMines(minesCount);
        UseMines();

        return this;
    }

    public int[,] GetGrid()
    {
        return _grid;
    }

    private void AddMines(int minesCount)
    {
        var random = new Random();

        do
        {
            var columnIndex = random.Next(_width - 1);
            var rowIndex = random.Next(_height - 1);

            if (_grid[rowIndex, columnIndex] != GameCellValue.Mine)
            {
                _grid[rowIndex, columnIndex] = GameCellValue.Mine;
                minesCount--;
            }

        } while (minesCount > 0);
    }

    private void UseMines()
    {
        for (var column = 0; column < _width; column++)
        {
            for (var row = 0; row < _height; row++)
            {
                var gridCell = _grid[row, column];

                if (gridCell != GameCellValue.Mine)
                {
                    continue;
                }

                ApplyMine(row, column);
            }
        }
    }

    private void ApplyMine(int mineRow, int mineColumn)
    {
        var boundaryCellIndexes = PrepareMineBoundary(mineRow, mineColumn);

        for (var i = 0; i < boundaryCellIndexes.GetLength(0); i++)
        {
            var cellRow = boundaryCellIndexes[i, 0];
            var cellColumn = boundaryCellIndexes[i, 1];

            if (cellRow < 0 || cellRow >= _height
                || cellColumn < 0 || cellColumn >= _width
                || _grid[cellRow, cellColumn] == GameCellValue.Mine)
            {
                continue;
            }
            _grid[cellRow, cellColumn]++;
        }
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
