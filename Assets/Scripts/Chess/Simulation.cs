using System;
using System.Collections.Generic;
using NUnit.Framework;
using SideToMove = Board.SideToMove;

public static class SimulationUtils
{
    public static string ToEvalInt(this int eval)
    {
        if (eval is < Simulation.MinEval or > Simulation.MaxEval)
        {
            return "invalid";
        }

        if (eval == Simulation.MinEval)
        {
            return "0-1";
        }

        if (eval == Simulation.MaxEval)
        {
            return "1-0";
        }

        if (eval is > Simulation.MinEval and <= Simulation.BlackMateThreshold)
        {
            return $"#-{(eval - Simulation.MinEval) / 2}";
        }
        
        if (eval is < Simulation.MaxEval and >= Simulation.WhiteMateThreshold)
        {
            return $"#{(Simulation.MaxEval - eval) / 2}";
        }

        return $"{(float)eval / 100.0f}";
    }
    
    public static bool IsWhiteAbleToForceMate(int eval)
    {
        return eval >= Simulation.WhiteMateThreshold;
    }

    public static bool IsBlackAbleToForceMate(int eval)
    {
        return eval <= Simulation.BlackMateThreshold;
    }
}

public unsafe ref struct Simulation
{
    public Game game;
    public const int MinEval = -100000; // >=, 0-1
    public const int MaxEval = 100000; // <=, 1-0

    public const int AlphaBetaMax = MaxEval + 1;
    public const int AlphaBetaMin = MinEval - 1;

    public const int WhiteMateThreshold = MaxEval - 50; // >=
    public const int BlackMateThreshold = MinEval + 50; // <=
    
    public Simulation(string fen)
    {
        game = new Game(fen);
        moves = new List<(Move, int)>();
    }

    public List<(Move, int)> moves;
    
    public (Move, int) GetBestMove(int depth, int absoluteDepth = 8)
    {
        moves.Clear();
        
        int eval = AlphaBeta(depth, absoluteDepth, game.currentBoard->m_SideToMove, AlphaBetaMin, AlphaBetaMax, true);
        
        if (game.currentBoard->m_SideToMove == SideToMove.White)
        {
            int bestEval = MinEval;
            Move bestMove = default;
            foreach ((Move move, int evalAfterMove) in moves)
            {
                if (evalAfterMove >= bestEval)
                {
                    bestEval = evalAfterMove;
                    bestMove = move;
                }
            }
        
            return (bestMove, eval);
        }
        else
        {
            float bestEval = float.PositiveInfinity;
            Move bestMove = default;
            foreach ((Move move, int evalAfterMove) in moves)
            {
                if (evalAfterMove <= bestEval)
                {
                    bestEval = evalAfterMove;
                    bestMove = move;
                }
            }
        
            return (bestMove, eval);
        }
        
        
    }
    
    
    // alpha beta
    public int AlphaBeta(int depth, int absoluteMaxDepth, SideToMove sideToMove, int alpha, int beta, bool isRoot = false)
    {
        if (depth == 0 || absoluteMaxDepth == 0)
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
            int value = MinEval;
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
                
                // extend search if black is in check
                int moveExtension = 0;
                if (game.currentBoard->BlackKingInCheck)
                {
                    moveExtension++;
                }

                if ((move & Move.MoveTypeMask) != Move.QuietMove)
                {
                    moveExtension++;
                }

                int moveEval = AlphaBeta(depth - 1 + moveExtension, absoluteMaxDepth - 1, SideToMove.Black, alpha, beta);
                if (SimulationUtils.IsWhiteAbleToForceMate(moveEval))
                {
                    moveEval -= 1;
                }
                else if (SimulationUtils.IsBlackAbleToForceMate(moveEval))
                {
                    moveEval += 1;
                }
                if (isRoot)
                {
                    moves.Add((move, moveEval));
                }

                value = Math.Max(value, moveEval);
                if (value > beta)
                {
                    game.UnmakeMove();
                    return value;
                }
                
                alpha = Math.Max(alpha, value);
                game.UnmakeMove();
            }
            
            if (counter == 0)
            {
                // am i in check ?
                if (game.currentBoard->WhiteKingInCheck)
                {
                    // 0-1
                    return MinEval;
                }

                // stalemate
                return 0; // 1/2-1/2
            }
            
            return value;
        }
        else
        {
            int value = MaxEval;
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

                int moveExtension = 0;
                if (game.currentBoard->WhiteKingInCheck)
                {
                    moveExtension++;
                }

                if ((move & Move.MoveTypeMask) != Move.QuietMove)
                {
                    moveExtension++;
                }

                int moveEval = AlphaBeta(depth - 1 + moveExtension, absoluteMaxDepth -1, SideToMove.White, alpha, beta);
                if (SimulationUtils.IsWhiteAbleToForceMate(moveEval))
                {
                    moveEval -= 1;
                }
                else if (SimulationUtils.IsBlackAbleToForceMate(moveEval))
                {
                    moveEval += 1;
                }
                
                if (isRoot)
                {
                    moves.Add((move, moveEval));
                }

                value = Math.Min(value, moveEval);
                if (value < alpha)
                {
                    game.UnmakeMove();
                    return value;
                }
                
                beta = Math.Min(beta, value);
                game.UnmakeMove();
            }
            
            if (counter == 0)
            {
                // am i in check ?
                if (game.currentBoard->BlackKingInCheck)
                {
                    // 1-0
                    return MaxEval;
                }

                // stalemate
                return 0; // 1/2-1/2
            }
            
            return value;
        }
    }
}