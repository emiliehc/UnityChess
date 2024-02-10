using System;
using NUnit.Framework;

public unsafe class SimulationTest
{
    [Test]
    public void Test()
    {
        Simulation simulation = new Simulation(Game.StartingFen);
        float eval = simulation.AlphaBeta(7, Board.SideToMove.White, float.NegativeInfinity, float.PositiveInfinity, true);
        Console.WriteLine(eval);
        // current board eval simple
        Console.WriteLine(simulation.game.currentBoard->SimpleEvaluate());
        // print all moves
        foreach ((Move move, float evalAfterMove) in simulation.moves)
        {
            Console.WriteLine(
                $"{BoardUtils.GetMoveDescriptionWithBoard(*simulation.game.currentBoard, move)}: {evalAfterMove}");
        }
        
        // print board fen
        Console.WriteLine(simulation.game.Fen);
        Assert.AreEqual(Game.StartingFen, simulation.game.Fen);
    }
}