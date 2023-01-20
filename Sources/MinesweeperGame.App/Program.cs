using MinesweeperGame.App.Entities;
using MinesweeperGame.App.Extensions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Minesweeper console game.\nPress enter to start new game");
        Console.ReadLine();

        bool playGame;
        do
        {
            StartGame();

            playGame = StartAgain();

        } while (playGame);
    }

    static void StartGame()
    {
        int level = default;
        Console.WriteLine("\nChose your game level. (1: Begginer, 2: Normal, 3: Expert");

        do
        {
            var inputKey = Console.ReadKey();
            switch (inputKey.KeyChar)
            {
                case '1': level = GameLevel.Begginer; break;
                case '2': level = GameLevel.Normal; break;
                case '3': level = GameLevel.Expert; break;
                default: Console.WriteLine(" is wrong input, try again"); break;
            }
        } while (level == default);

        var game = Game.Create(level);

        bool canContinueGame = true;
        do
        {
            Console.Clear();
            game.Print("There is your game board.");
            Console.WriteLine("Seelct action (open/mark), enter coordinates (numbers) and press 'enter'.\n(For example, input 'o 1 2' means open cell at the second row and third column)");
            var inputLine = Console.ReadLine()?.ToUpper();
            if (string.IsNullOrWhiteSpace(inputLine))
            {
                continue;
            }

            var userInputParts = inputLine.Split(' ');
            if (userInputParts.Length != 3)
            {
                continue;
            }

            var operation = Convert.ToChar(userInputParts[0]);
            if (!(operation is GameCellOperations.Mark or GameCellOperations.Open)
                || !int.TryParse(userInputParts[1], out var row)
                || !int.TryParse(userInputParts[2], out var column)
                || !game.IsValidCoordinates(row, column))
            {
                continue;
            }

            canContinueGame = game.MoveNext(operation, row, column);
        }
        while (canContinueGame);

        game.PrintResults("Game is over.");
    }

    static bool StartAgain()
    {
        Console.WriteLine("Would you like to play again? (Yes: Y, No: N)");

        var inputKey = Console.ReadKey();
        return inputKey.KeyChar switch
        {
            'Y' or 'y' => true,
            'N' or 'n' => false,
            _ => false
        };
    }
}
 
