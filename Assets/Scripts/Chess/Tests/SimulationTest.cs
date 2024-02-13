using System;
using NUnit.Framework;

public unsafe class SimulationTest
{
    [Test]
    public void Test()
    {
        const string fen = "r5rk/2p1Nppp/3p3P/pp2p1P1/4P3/2qnPQK1/8/R6R w - - 1 0";
        Simulation simulation = new Simulation(fen);
        for (int i = 0; i < 60; i++)
        {
            (Move move, float eval) = simulation.GetBestMove(6);
            Console.WriteLine($"{i} {BoardUtils.GetMoveDescriptionWithBoard(*simulation.game.currentBoard, move)}");
            simulation.game.MakeMove(move);
        }
    }
}