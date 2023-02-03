namespace MinesweeperGame.App.Entities;

public record struct CellState(int State)
{
    public int Value => GetValue();

    public bool IsMarked => (State >= NoneValue.Marked && State <= MineValue.Marked) || (State >= NoneValue.VisibleMarked && State <= MineValue.VisibleMarked);

    public bool IsOpened => State >= NoneValue.VisibleSimple && State <= MineValue.VisibleMarked;

    public bool IsMine => State is MineValue.Simple or MineValue.Marked or MineValue.Visible or MineValue.VisibleMarked;

    public static CellState None => new(0);

    private int GetValue()
    {
        var result = State;
        if (IsOpened)
        {
            result -= ValueShift.Open;
        }

        if (IsMarked)
        {
            result -= ValueShift.Mark;
        }

        return result;
    }
}
