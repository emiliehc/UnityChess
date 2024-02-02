using NUnit.Framework;

public unsafe class GameTest
{
    [Test]
    public void Test()
    {
        Game game = new Game(Game.StartingFen);
        Assert.AreEqual(Game.StartingFen, game.Fen);

        MoveList list;
        game.currentBoard->GenerateMoves(&list);
        
    }
}