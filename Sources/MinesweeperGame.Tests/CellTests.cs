namespace MinesweeperGame.Tests;

public class CellTests
{

    [Fact]
    public void CreateSimpleCell_Then_Success()
    {
        // Arrange
        var cell = new Cell(NoneValue.Simple);

        // Act
        var cellState = cell.GetState();

        // Assert

        using (new AssertionScope())
        {
            cell.Value.Should().Be(NoneValue.Simple);
            cellState.Value.Should().Be(NoneValue.Simple);
            cellState.IsMarked.Should().BeFalse();
            cellState.IsOpened.Should().BeFalse();
            cellState.IsMine.Should().BeFalse();
        }
    }

    [Fact]
    public void CreateMine_Then_Success()
    {
        // Arrange
        var cell = Cell.NewMine;

        // Act
        var cellState = cell.GetState();
        var cellIncrement = () => cell++;

        // Assert

        using (new AssertionScope())
        {
            cell.Value.Should().Be(MineValue.Simple);
            cellState.Value.Should().Be(MineValue.Simple);
            cellState.IsMarked.Should().BeFalse();
            cellState.IsOpened.Should().BeFalse();
            cellState.IsMine.Should().BeTrue();
            cellIncrement.Should().Throw<InvalidOperationException>();
        }
    }

    [Fact]
    public void MarkCell_Then_Success()
    {
        // Arrange
        var cell = new Cell(5);

        // Act
        var canMark = cell.TryMark(out var markedCell);
        var cellState = cell.GetState();
        var markedCellState = markedCell.GetState();
        var canMarkMarkedCell = markedCell.TryMark(out var markedTwiceCell);

        // Assert

        using (new AssertionScope())
        {
            cell.Value.Should().Be(5);
            canMark.Should().BeTrue();
            cellState.Value.Should().Be(5);
            cellState.IsMarked.Should().BeFalse();
            cellState.IsOpened.Should().BeFalse();
            cellState.IsMine.Should().BeFalse();

            markedCellState.IsMarked.Should().BeTrue();
            markedCellState.IsOpened.Should().BeFalse();
            markedCellState.IsMine.Should().BeFalse();
            markedCell.Value.Should().Be(15);
            markedCellState.Value.Should().Be(5);

            canMarkMarkedCell.Should().BeFalse();
            markedTwiceCell.Should().Be(markedCell);
        }
    }

    [Fact]
    public void OpenCell_Then_Success()
    {
        // Arrange
        var cell = new Cell(5);

        // Act
        var canOpen = cell.TryOpen(out var openedCell);
        var cellState = cell.GetState();
        var openedCellState = openedCell.GetState();
        var canOpenTwiceCell = openedCell.TryOpen(out var openedTwiceCell);

        // Assert

        using (new AssertionScope())
        {
            cell.Value.Should().Be(5);
            cellState.Value.Should().Be(5);
            cellState.IsMarked.Should().BeFalse();
            cellState.IsOpened.Should().BeFalse();
            cellState.IsMine.Should().BeFalse();

            canOpen.Should().BeTrue();
            openedCellState.IsMarked.Should().BeFalse();
            openedCellState.IsOpened.Should().BeTrue();
            openedCellState.IsMine.Should().BeFalse();
            openedCell.Value.Should().Be(105);
            openedCellState.Value.Should().Be(5);

            canOpenTwiceCell.Should().BeFalse();
            openedTwiceCell.Should().Be(openedCell);
        }
    }

    [Fact]
    public void UnmarkCell_Then_Success()
    {
        // Arrange
        var cell = new Cell(15);

        // Act
        var canUnmark = cell.TryUnmark(out var unmarkedCell);
        var cellState = cell.GetState();
        var unmarkedCellState = unmarkedCell.GetState();
        var canUnmarkTwiceCell = unmarkedCell.TryUnmark(out var unmarkedTwiceCell);

        // Assert

        using (new AssertionScope())
        {
            cell.Value.Should().Be(15);
            canUnmark.Should().BeTrue();
            cellState.Value.Should().Be(5);
            cellState.IsMarked.Should().BeTrue();
            cellState.IsOpened.Should().BeFalse();
            cellState.IsMine.Should().BeFalse();

            unmarkedCellState.IsMarked.Should().BeFalse();
            unmarkedCellState.IsOpened.Should().BeFalse();
            unmarkedCellState.IsMine.Should().BeFalse();
            unmarkedCell.Value.Should().Be(5);
            unmarkedCellState.Value.Should().Be(5);

            canUnmarkTwiceCell.Should().BeFalse();
            unmarkedTwiceCell.Should().Be(unmarkedCell);
        }
    }
}
