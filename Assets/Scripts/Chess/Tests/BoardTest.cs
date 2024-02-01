using System;
using System.IO;
using NUnit.Framework;

public class BoardTest
{
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
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = new Board(fen);
        Assert.AreEqual(fen, board.Fen);
    }
}