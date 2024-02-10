using System;
using System.Collections.Generic;
using NUnit.Framework;
using SideToMove = Board.SideToMove;

public unsafe ref struct Simulation
{
    public Game game;
    
    public Simulation(string fen)
    {
        game = new Game(fen);
        moves = new List<(Move, float)>();
    }

    public List<(Move, float)> moves;
    
    // alpha beta
    public float AlphaBeta(int depth, SideToMove sideToMove, float alpha, float beta, bool isRoot = false)
    {
        if (depth == 0)
        {
            return game.currentBoard->SimpleEvaluate();
        }
        
        MoveList list;
        int count = game.currentBoard->GenerateMoves(&list);
        if (count == 0)
        {
            // if neither is in check
            if (game.currentBoard->IsSideInCheck(sideToMove) == false)
            {
                // simple stalemate (not a proven stalemate)
                return 0;
            }
        }

        if (sideToMove == SideToMove.White)
        {
            float value = float.NegativeInfinity;
            int counter = 0;
            for (int i = 0; i < count; i++)
            {
                Move move = list.Take();
                game.MakeMove(move);
                // is the move resulting in the side to move being in check?
                if (game.currentBoard->WhiteKingInCheck)
                {
                    game.UnmakeMove();
                    continue;
                }

                counter++;

                float evalAfterMove = AlphaBeta(depth - 1, SideToMove.Black, alpha, beta);
                if (isRoot)
                {
                    moves.Add((move, evalAfterMove));
                }

                value = MathF.Max(value, evalAfterMove);
                if (value > beta)
                {
                    game.UnmakeMove();
                    return value;
                }
                
                alpha = MathF.Max(alpha, value);
                game.UnmakeMove();
            }
            
            if (counter == 0)
            {
                // am i in check ?
                if (game.currentBoard->WhiteKingInCheck)
                {
                    // 0-1
                    return float.NegativeInfinity;
                }

                // stalemate
                return 0; // 1/2-1/2
            }
            
            return value;
        }
        else
        {
            float value = float.PositiveInfinity;
            int counter = 0;
            for (int i = 0; i < count; i++)
            {
                Move move = list.Take();
                game.MakeMove(move);
                // is the move resulting in the side to move being in check?
                if (game.currentBoard->BlackKingInCheck)
                {
                    game.UnmakeMove();
                    continue;
                }

                counter++;

                float evalAfterMove = AlphaBeta(depth - 1, SideToMove.White, alpha, beta);
                if (isRoot)
                {
                    moves.Add((move, evalAfterMove));
                }

                value = MathF.Min(value, evalAfterMove);
                if (value < alpha)
                {
                    game.UnmakeMove();
                    return value;
                }
                
                beta = MathF.Min(beta, value);
                game.UnmakeMove();
            }
            
            if (counter == 0)
            {
                // am i in check ?
                if (game.currentBoard->BlackKingInCheck)
                {
                    // 1-0
                    return float.PositiveInfinity;
                }

                // stalemate
                return 0; // 1/2-1/2
            }
            
            return value;
        }
    }
}