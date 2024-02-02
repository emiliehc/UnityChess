using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public unsafe class GameController : MonoBehaviour
{
    private static Game s_Game;
    public GameObject A1, B1, C1, D1, E1, F1, G1, H1;
    public GameObject A2, B2, C2, D2, E2, F2, G2, H2;
    public GameObject A3, B3, C3, D3, E3, F3, G3, H3;
    public GameObject A4, B4, C4, D4, E4, F4, G4, H4;
    public GameObject A5, B5, C5, D5, E5, F5, G5, H5;
    public GameObject A6, B6, C6, D6, E6, F6, G6, H6;
    public GameObject A7, B7, C7, D7, E7, F7, G7, H7;
    public GameObject A8, B8, C8, D8, E8, F8, G8, H8;
    private GameObject[] m_Squares = new GameObject[128]; // 0x88 indexed
    public GameObject WhitePawnPrefab, WhiteKnightPrefab, WhiteBishopPrefab, WhiteRookPrefab, WhiteQueenPrefab, WhiteKingPrefab;
    public GameObject BlackPawnPrefab, BlackKnightPrefab, BlackBishopPrefab, BlackRookPrefab, BlackQueenPrefab, BlackKingPrefab;
    public GameObject[] m_PiecePrefabs = new GameObject[16];
    
    private void Awake()
    {
        // GC pin
        GC.KeepAlive(s_Game);
        GCHandle.Alloc(s_Game, GCHandleType.Pinned);
        
        s_Game = new Game(Game.StartingFen);
        
        // set up squares
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a1")] = A1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b1")] = B1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c1")] = C1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d1")] = D1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e1")] = E1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f1")] = F1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g1")] = G1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h1")] = H1;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a2")] = A2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b2")] = B2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c2")] = C2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d2")] = D2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e2")] = E2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f2")] = F2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g2")] = G2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h2")] = H2;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a3")] = A3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b3")] = B3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c3")] = C3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d3")] = D3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e3")] = E3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f3")] = F3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g3")] = G3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h3")] = H3;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a4")] = A4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b4")] = B4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c4")] = C4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d4")] = D4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e4")] = E4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f4")] = F4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g4")] = G4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h4")] = H4;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a5")] = A5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b5")] = B5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c5")] = C5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d5")] = D5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e5")] = E5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f5")] = F5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g5")] = G5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h5")] = H5;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a6")] = A6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b6")] = B6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c6")] = C6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d6")] = D6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e6")] = E6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f6")] = F6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g6")] = G6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h6")] = H6;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a7")] = A7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b7")] = B7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c7")] = C7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d7")] = D7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e7")] = E7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f7")] = F7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g7")] = G7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h7")] = H7;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("a8")] = A8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("b8")] = B8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("c8")] = C8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("d8")] = D8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("e8")] = E8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("f8")] = F8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("g8")] = G8;
        m_Squares[BoardUtils.SquareAlgebraicTo0X88("h8")] = H8;
        
        // set up piece prefabs
        m_PiecePrefabs[(int)PieceUtils.WhitePawn] = WhitePawnPrefab;
        m_PiecePrefabs[(int)PieceUtils.WhiteKnight] = WhiteKnightPrefab;
        m_PiecePrefabs[(int)PieceUtils.WhiteBishop] = WhiteBishopPrefab;
        m_PiecePrefabs[(int)PieceUtils.WhiteRook] = WhiteRookPrefab;
        m_PiecePrefabs[(int)PieceUtils.WhiteQueen] = WhiteQueenPrefab;
        m_PiecePrefabs[(int)PieceUtils.WhiteKing] = WhiteKingPrefab;
        
        m_PiecePrefabs[(int)PieceUtils.BlackPawn] = BlackPawnPrefab;
        m_PiecePrefabs[(int)PieceUtils.BlackKnight] = BlackKnightPrefab;
        m_PiecePrefabs[(int)PieceUtils.BlackBishop] = BlackBishopPrefab;
        m_PiecePrefabs[(int)PieceUtils.BlackRook] = BlackRookPrefab;
        m_PiecePrefabs[(int)PieceUtils.BlackQueen] = BlackQueenPrefab;
        m_PiecePrefabs[(int)PieceUtils.BlackKing] = BlackKingPrefab;
    }

    private void Start()
    {
        // set up board with game
        Board* board = s_Game.currentBoard;
        for (byte i = 0; i < 128; i++)
        {
            if (!BoardUtils.IsSquareValid(i)) continue;
            GameObject square = m_Squares[i];
            if ((((Piece)board->m_Pieces[i]) & Piece.PieceColorMask) == Piece.Empty) continue;
            GameObject piece = Instantiate(m_PiecePrefabs[(int)((Piece)board->m_Pieces[i] & Piece.PieceColorMask)]);
            piece.transform.position = square.transform.position - new Vector3(0, 0, 2.0f);
        }
    }

    private void Update()
    {
        Board* board = s_Game.currentBoard;
    }
}
