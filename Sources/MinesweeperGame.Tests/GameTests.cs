using MinesweeperGame.App.Entities;
using FluentAssertions.Execution;
using FluentAssertions;

namespace MinesweeperGame.Tests;

public class GameTests
{
    [Fact]
    public void CreateGame_Then_Success()
    {
        // Arrange
        var game = Game.Create(3);

        // Act
        var cellsState = game.GetCellState();

        // Assert
        using (new AssertionScope())
        {
            game.Should().NotBeNull();
            cellsState.Should().NotBeNull();
            game.State.Should().Be(GameState.Initial);
            cellsState.Should().NotBeNull();
            game.MarksCounter.Should().BeGreaterThan(0);

        }
    }

    [Fact]
    public void OpenBoundaryCells_Then_CheckState()
    {
        // Arrange
        var game = Game.Create(3);

        // Act
        game.MoveNext(GameCellOperations.Open, 0, 1);

        // Assert
        using (new AssertionScope())
        {
            game.Should().NotBeNull();
            game.State.Should().BeOneOf(GameState.Active, GameState.Loss);
            game.MarksCounter.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public void CreateTestGame_Then_Loss()
    {
        // Arrange
        int rows = 4, columns = 3, minesCount = 4;
        var gridBuilder = GameGridBuilder.Create(rows, columns);
        var cells = gridBuilder.GetCells();
        cells[0, 1] = Cell.NewMine;
        cells[1, 1] = Cell.NewMine;
        cells[2, 1] = Cell.NewMine;
        cells[3, 1] = Cell.NewMine;

        var game = Game.Test(cells, minesCount);

        // Act
        var firstMove = game.MoveNext(GameCellOperations.Open, 0, 0);
        var secondMove = game.MoveNext(GameCellOperations.Mark, 0, 1);
        var thirdMove = game.MoveNext(GameCellOperations.Open, 2, 2);
        var fourthMove = game.MoveNext(GameCellOperations.Open, 3, 1);

        // Assert
        using (new AssertionScope())
        {
            firstMove.Should().BeTrue();
            secondMove.Should().BeTrue();
            thirdMove.Should().BeTrue();
            fourthMove.Should().BeFalse();

            game.State.Should().BeOneOf(GameState.Loss);
            game.MarksCounter.Should().Be(3);
        }
    }

    [Fact]
    public void CreateTestGame_Then_Win()
    {
        // Arrange
        int rows = 4, columns = 3, minesCount = 4;
        var gridBuilder = GameGridBuilder.Create(rows, columns);
        var cells = gridBuilder.GetCells();
        cells[0, 1] = Cell.NewMine;
        cells[1, 1] = Cell.NewMine;
        cells[2, 1] = Cell.NewMine;
        cells[3, 1] = Cell.NewMine;

        var game = Game.Test(cells, minesCount);

        // Act
        var firstMove = game.MoveNext(GameCellOperations.Open, 0, 0);
        var secondMove = game.MoveNext(GameCellOperations.Mark, 0, 1);
        var thirdMove = game.MoveNext(GameCellOperations.Mark, 1, 1);
        var fourthMove = game.MoveNext(GameCellOperations.Mark, 2, 1);
        var fifthhMove = game.MoveNext(GameCellOperations.Mark, 3, 1);

        // Assert
        using (new AssertionScope())
        {
            firstMove.Should().BeTrue();
            secondMove.Should().BeTrue();
            thirdMove.Should().BeTrue();
            fourthMove.Should().BeTrue();
            fifthhMove.Should().BeFalse();

            game.State.Should().BeOneOf(GameState.Win);
            game.MarksCounter.Should().Be(0);
        }
    }
}