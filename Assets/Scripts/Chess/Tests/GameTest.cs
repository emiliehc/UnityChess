using System;
using System.Diagnostics;
using NUnit.Framework;

public unsafe class GameTest
{
    [Test]
    public void Test()
    {
        Game game = new Game(Game.StartingFen);
        Assert.AreEqual(Game.StartingFen, game.Fen);
        
        // time
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < 100000; i++)
        {
            MoveList list;
            int count = game.currentBoard->GenerateMoves(&list);
            for (int j = 0; j < count; j++)
            {
                Move move = list.Take();
                game.MakeMove(move);
            }
        }
        sw.Stop();
        Console.WriteLine(sw.Elapsed);
    }
}