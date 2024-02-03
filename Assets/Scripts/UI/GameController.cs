using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Camera m_Camera;
    private Color m_DarkColor;
    private Color m_LightColor;
    private List<GameObject> m_OnBoardPieces = new List<GameObject>();
    
    private void Awake()
    {
        m_Camera = Camera.main;
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

        m_DarkColor = A1.GetComponent<SpriteRenderer>().color;
        m_LightColor = A2.GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        RenderBoard();
    }

    private void RenderBoard()
    {
        foreach (GameObject piece in m_OnBoardPieces)
        {
            Destroy(piece);
        }
        m_OnBoardPieces.Clear();
        Board* board = s_Game.currentBoard;
        for (byte i = 0; i < 128; i++)
        {
            if (!BoardUtils.IsSquareValid(i)) continue;
            GameObject square = m_Squares[i];
            if ((((Piece)board->m_Pieces[i]) & Piece.PieceColorMask) == Piece.Empty) continue;
            GameObject piece = Instantiate(m_PiecePrefabs[(int)((Piece)board->m_Pieces[i] & Piece.PieceColorMask)], square.transform, false);
            piece.transform.position = square.transform.position - new Vector3(0, 0, 2.0f);
            m_OnBoardPieces.Add(piece);
        }
    }
    
    private static byte Get0X88SquareFromMousePos(Vector2 mousePos)
    {
        byte file = (byte)((mousePos.x + 4f));
        byte rank = (byte)((mousePos.y + 4f));
        return (byte)(rank * 16 + file);
    }
    
    private bool m_Dragging = false;
    private GameObject m_DraggingPiece;
    private Piece m_DraggingPieceAbstract;

    private Move[] m_Moves = null;
    private Dictionary<(byte, byte), List<Move>> m_MoveMap = new Dictionary<(byte, byte), List<Move>>();

    private void ResetColors()
    {
        // reset colours
        for (byte i = 0; i < 128; i++)
        {
            if (!BoardUtils.IsSquareValid(i)) continue;
            GameObject squareObject = m_Squares[i];
            if (squareObject)
            {
                squareObject.GetComponent<SpriteRenderer>().color = (i % 2 == 0) ^ (i / 16 % 2 == 0) ? m_LightColor : m_DarkColor;
            }
        }
    }
    
    private void Update()
    {
        Board* board = s_Game.currentBoard;
        
        if (m_Moves == null)
        {
            MoveList list;
            int count = board->GenerateMoves(&list);
            m_Moves = new Move[count];
            for (int i = 0; i < count; i++)
            {
                m_Moves[i] = list.Take();
            }
            
            // set up piece to legal squares
            foreach (Move move in m_Moves)
            {
                byte from, to;
                MoveUtils.DeconstructMove(move, out from, out to);
                Piece piece = (Piece)board->m_Pieces[from];
                if (m_MoveMap.ContainsKey((from, to)))
                {
                    m_MoveMap[(from, to)].Add(move);
                }
                else
                {
                    m_MoveMap[(from, to)] = new List<Move> {move};
                }
            }
        }
        
        
        
        // input
        Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        // inside the bounds of the board ?
        Rect boardBounds = new Rect(-4, -4, 8, 8);
        if (boardBounds.Contains(mousePos))
        {
            // get square
            byte square = Get0X88SquareFromMousePos(mousePos);
            // color the square
            try
            {
                GameObject squareObj = m_Squares[square];
                if (squareObj)
                {
                    // allow for dragging
                    if (Input.GetMouseButtonDown(0))
                    {
                        // move piece
                        if (((Piece)board->m_Pieces[square]) != Piece.Empty)
                        {
                            GameObject pieceObj = squareObj.transform.GetChild(0).gameObject;
                            if (pieceObj)
                            {
                                // does this piece even have legal moves ?
                                // get coordinates
                                bool hasLegalMoves = false;
                                foreach ((byte from, byte to) in m_MoveMap.Keys)
                                {
                                    if (from == square)
                                    {
                                        hasLegalMoves = true;
                                        break;
                                    }
                                }
                                
                                if (hasLegalMoves)
                                {
                                    m_DraggingPiece = pieceObj;
                                    m_DraggingPieceAbstract = (Piece)board->m_Pieces[square];
                                    m_Dragging = true;
                                    
                                    // highlight legal squares
                                    List<byte> legalSquares = new List<byte>();
                                    
                                    foreach ((byte from, byte to) in m_MoveMap.Keys)
                                    {
                                        if (from == square)
                                        {
                                            legalSquares.Add(to);
                                        }
                                    }
                                    
                                    foreach (GameObject legalSquareObj in legalSquares.Select(legalSquare => m_Squares[legalSquare]).Where(legalSquareObj => legalSquareObj))
                                    {
                                        legalSquareObj.GetComponent<SpriteRenderer>().color *= new Color(1.0f, 1.2f, 1.0f, 1.0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // ignore
            }
        }
        
        if (m_Dragging)
        {
            m_DraggingPiece.transform.position = new Vector3(mousePos.x, mousePos.y, -10.0f);
            if (Input.GetMouseButtonUp(0))
            {
                // drop piece
                // query for legal squares
                List<byte> legalSquares = new List<byte>();
                {
                    foreach ((byte from, byte to) in m_MoveMap.Keys)
                    {
                        if (from == Get0X88SquareFromMousePos(m_DraggingPiece.transform.parent.position))
                        {
                            legalSquares.Add(to);
                        }
                    }
                }
                {
                    byte to = Get0X88SquareFromMousePos(mousePos);
                    if (legalSquares.Contains(to))
                    {
                        // find the move in the move list
                        List<Move> moves = m_MoveMap[(Get0X88SquareFromMousePos(m_DraggingPiece.transform.parent.position), to)];
                        if (moves.Count == 1)
                        {
                            Move move = moves[0];
                            s_Game.MakeMove(move);
                            // reset
                            m_Dragging = false;
                            m_MoveMap.Clear();
                            m_Moves = null;

                            RenderBoard();
                            ResetColors();
                        }
                        else
                        {
                            // promotion
                            // TODO
                            // for now, autopromote to queen
                            Move move = moves.FirstOrDefault(m => (m & Move.MoveTypeMask) == Move.QueenPromotionCapture || (m & Move.MoveTypeMask) == Move.QueenPromotion);
                            s_Game.MakeMove(move);
                            
                            // reset
                            m_Dragging = false;
                            m_MoveMap.Clear();
                            m_Moves = null;

                            RenderBoard();
                            ResetColors();
                        }
                    }
                }
            }
        }
    }
}
