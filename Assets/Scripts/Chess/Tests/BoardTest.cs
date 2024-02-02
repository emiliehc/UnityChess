using System;
using System.IO;
using NUnit.Framework;

public unsafe class BoardTest
{
    private const string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    [Test]
    public void PieceTest()
    {
        // white pawn is pawn
        Assert.AreEqual(Piece.Pawn, PieceUtils.WhitePawn & Piece.PieceMask);
        // white pawn is white
        Assert.AreEqual(Piece.White, PieceUtils.WhitePawn & Piece.ColorMask);
        // black rook is rook
        Assert.AreEqual(Piece.Rook, PieceUtils.BlackRook & Piece.PieceMask);
        // black rook is black
        Assert.AreEqual(Piece.Black, PieceUtils.BlackRook & Piece.ColorMask);

        Piece whiteKing = PieceUtils.CreatePiece(Piece.King, Piece.White);
        // white king is king
        Assert.AreEqual(Piece.King, whiteKing & Piece.PieceMask);
        // white king is white
        Assert.AreEqual(Piece.White, whiteKing & Piece.ColorMask);
    }

    [Test]
    public void SquareTest()
    {
        {
            // specific test
            byte square = BoardUtils.SquareAlgebraicTo0X88("e4");
            Assert.AreEqual(0x34, square);
            string algebraic = BoardUtils.Square0X88ToAlgebraic(0x34);
            Assert.AreEqual("e4", algebraic);
        }
        
        // all valid square test
        for (byte square = 0; square < 128; square++)
        {
            bool isSquareValid = BoardUtils.IsSquareValid(square);
            if (square % 16 < 8)
            {
                // valid squares
                Assert.IsTrue(isSquareValid);
                
                string algebraic = BoardUtils.Square0X88ToAlgebraic(square);
                byte square2 = BoardUtils.SquareAlgebraicTo0X88(algebraic);
                Assert.AreEqual(square, square2);
            }
            else
            {
                // invalid squares
                Assert.IsFalse(isSquareValid);
            }
        }
    }

    [Test]
    public void BoardInitializationTest()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = new Board(fen);
        Assert.AreEqual(fen, board.Fen);
    }

    [Test]
    public void BoardSquareValidityTest()
    {
        {
            byte square = BoardUtils.SquareAlgebraicTo0X88("b1"); // knight on b1
            byte validSquare1 = (byte)(square + BoardUtils.DirectionNNE);
            Assert.AreEqual("c3", BoardUtils.Square0X88ToAlgebraic(validSquare1));
            Assert.IsTrue(BoardUtils.IsSquareValid(validSquare1));

            byte invalidSquare1 = (byte)(square + BoardUtils.DirectionSEE);
            byte invalidSquare2 = (byte)(square + BoardUtils.DirectionSSE);
            byte invalidSquare3 = (byte)(square + BoardUtils.DirectionSSW);
            byte invalidSquare4 = (byte)(square + BoardUtils.DirectionSWW);
            byte invalidSquare5 = (byte)(square + BoardUtils.DirectionNWW);

            // assert invalid squares
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare1));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare2));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare3));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare4));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare5));

            byte validSquare2 = (byte)(square + BoardUtils.DirectionNNW);
            Assert.AreEqual("a3", BoardUtils.Square0X88ToAlgebraic(validSquare2));
            Assert.IsTrue(BoardUtils.IsSquareValid(validSquare2));
        }
        {
            byte square = BoardUtils.SquareAlgebraicTo0X88("g8"); // knight on g8
            byte invalidSquare1 = (byte)(square + BoardUtils.DirectionNNE);
            byte invalidSquare2 = (byte)(square + BoardUtils.DirectionNEE);
            byte invalidSquare3 = (byte)(square + BoardUtils.DirectionSEE);
            byte validSquare1 = (byte)(square + BoardUtils.DirectionSSE);
            byte validSquare2 = (byte)(square + BoardUtils.DirectionSSW);
            byte invalidSquare4 = (byte)(square + BoardUtils.DirectionNNW);
            byte invalidSquare5 = (byte)(square + BoardUtils.DirectionNWW);
            
            // assert invalid squares
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare1));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare2));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare3));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare4));
            Assert.IsFalse(BoardUtils.IsSquareValid(invalidSquare5));
            
            // assert valid squares
            Assert.IsTrue(BoardUtils.IsSquareValid(validSquare1));
            Assert.IsTrue(BoardUtils.IsSquareValid(validSquare2));
            
            Assert.AreEqual("h6", BoardUtils.Square0X88ToAlgebraic(validSquare1));
            Assert.AreEqual("f6", BoardUtils.Square0X88ToAlgebraic(validSquare2));
        }
    }
    
    [Test]
    public void QuietMoveConstructDeconstructTest()
    {
        Move move = MoveUtils.ConstructQuietMove(BoardUtils.SquareAlgebraicTo0X88("e2"), BoardUtils.SquareAlgebraicTo0X88("e3"));
        {
            MoveUtils.DeconstructMove(move, out byte from, out byte to);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("e2"), from);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("e3"), to);
        }
        
        move = MoveUtils.ConstructQuietMove(BoardUtils.SquareAlgebraicTo0X88("e7"), BoardUtils.SquareAlgebraicTo0X88("e5"));
        {
            MoveUtils.DeconstructMove(move, out byte from, out byte to);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("e7"), from);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("e5"), to);
        }
        
        move = MoveUtils.ConstructQuietMove(BoardUtils.SquareAlgebraicTo0X88("a1"), BoardUtils.SquareAlgebraicTo0X88("h8"));
        {
            MoveUtils.DeconstructMove(move, out byte from, out byte to);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("a1"), from);
            Assert.AreEqual(BoardUtils.SquareAlgebraicTo0X88("h8"), to);
        }
    }

    [Test]
    public void MoveGenTest()
    {
        Board b = new Board(startingFen);
        int numMoves = 256;
        Move* moves = stackalloc Move[numMoves];
        MoveList moveList = new MoveList();
        int count = b.GenerateMoves(&moveList);
        Console.WriteLine(count);
        
        byte from = BoardUtils.SquareAlgebraicTo0X88("a7");
        byte to = BoardUtils.SquareAlgebraicTo0X88("h3");
        int manhattanDistance = BoardUtils.GetManhattanDistance(from, to);
        Console.WriteLine(manhattanDistance);
    }
    
    private const string castleOnlyFen = "r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1";
    
    [Test]
    public void CastleTest()
    {
        Board b = new Board(castleOnlyFen);
        MoveList moveList = new MoveList();
        int count = b.GenerateMoves(&moveList);
        Console.WriteLine(count);
    }
}