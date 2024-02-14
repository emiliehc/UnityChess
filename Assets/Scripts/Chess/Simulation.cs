using System;
using System.Collections.Generic;
using NUnit.Framework;
using SideToMove = Board.SideToMove;

public unsafe ref struct Simulation
{
    public Game game;
    private const int MinEval = -100000; // >=
    private const int MaxEval = 100000; // <=

    private const int AlphaBetaMax = MaxEval + 1;
    private const int AlphaBetaMin = MinEval - 1;

    private const int WhiteMateThreshold = MaxEval - 50; // >=
    private const int BlackMateThreshold = MinEval + 50; // <=
    
    public Simulation(string fen)
    {
        game = new Game(fen);
        moves = new List<(Move, float)>();
    }

    public List<(Move, float)> moves;
    
    public (Move, float) GetBestMove(int depth, int absoluteDepth = 8)
    {
        moves.Clear();
        
        float eval = AlphaBeta(depth, absoluteDepth, game.currentBoard->m_SideToMove, float.NegativeInfinity, float.PositiveInfinity, true);
        
        if (game.currentBoard->m_SideToMove == SideToMove.White)
        {
            float bestEval = float.NegativeInfinity;
            Move bestMove = default;
            foreach ((Move move, float evalAfterMove) in moves)
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
            foreach ((Move move, float evalAfterMove) in moves)
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
    public float AlphaBeta(int depth, int absoluteMaxDepth, SideToMove sideToMove, float alpha, float beta, bool isRoot = false)
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

                float evalAfterMove = AlphaBeta(depth - 1 + moveExtension, absoluteMaxDepth - 1, SideToMove.Black, alpha, beta);
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

                int moveExtension = 0;
                if (game.currentBoard->WhiteKingInCheck)
                {
                    moveExtension++;
                }

                if ((move & Move.MoveTypeMask) != Move.QuietMove)
                {
                    moveExtension++;
                }

                float evalAfterMove = AlphaBeta(depth - 1 + moveExtension, absoluteMaxDepth -1, SideToMove.White, alpha, beta);
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