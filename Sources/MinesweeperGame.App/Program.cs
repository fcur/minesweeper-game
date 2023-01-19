using MinesweeperGame.App.Entities;
using MinesweeperGame.App.Extension;

Console.WriteLine("Minesweeper console game.\nPress enter to start new game");
Console.ReadLine();

int level = default;
Console.WriteLine("Chose your game level. (1: Begginer, 2: Normal, 3: Expert");
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

bool continueGame = true;
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
    if (!(operation == GameCellOperations.Mark
        || operation == GameCellOperations.Open)
        || !int.TryParse(userInputParts[1], out var row)
        || !int.TryParse(userInputParts[2], out var column)
        || !game.IsValidCoordinates(row, column))
    {
        continue;
    }

    continueGame = game.MoveNext(operation, row, column);

}
while (continueGame);


game.PrintResults("Game is over.");
