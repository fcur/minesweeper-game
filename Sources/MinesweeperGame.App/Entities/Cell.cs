namespace MinesweeperGame.App.Entities;

public record struct Cell(int Value)
{
    public CellState GetState() => new(Value);

    public bool TryMark(out Cell result)
    {
        result = this;
        var state = GetState();
        if (state.IsMarked)
        {
            return false;
        }

        result = new(Value + ValueShift.Mark);
        return true;
    }

    public bool TryUnmark(out Cell result)
    {
        result = this;
        var state = GetState();
        if (!state.IsMarked)
        {
            return false;
        }

        result = new(Value - ValueShift.Mark);
        return true;
    }

    public bool TryOpen(out Cell result)
    {
        result = this;
        var state = GetState();

        if (state.IsOpened)
        {
            return false;
        }

        result = new(Value + ValueShift.Open);
        return true;
    }

    public static Cell operator ++(Cell target)
    {
        var state = target.GetState();
        if (state.IsMine)
        {
            throw new InvalidOperationException($"Can't perfrom operation for mine: '{target.Value}'");
        }

        return new(target.Value + 1);
    }

    public static Cell NewMine => new(MineValue.Simple);
}