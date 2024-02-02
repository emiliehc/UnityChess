using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Flags]
public enum Piece : byte
{
    // piece flags
    Empty = 0b_0000_0000,
    Pawn = 0b_0000_0001,
    Knight = 0b_0000_0010,
    Bishop = 0b_0000_0011,
    Rook = 0b_0000_0100,
    Queen = 0b_0000_0101,
    King = 0b_0000_0110,
    
    // color flags
    White = 0b_0000_0000,
    Black = 0b_0000_1000,
    
    // piece and color masks
    PieceMask = 0b_0000_0111,
    ColorMask = 0b_0000_1000,
    
    PieceColorMask = PieceMask | ColorMask,
}

public static class PieceUtils
{
    public static readonly Piece WhitePawn = Piece.Pawn | Piece.White;
    public static readonly Piece WhiteKnight = Piece.Knight | Piece.White;
    public static readonly Piece WhiteBishop = Piece.Bishop | Piece.White;
    public static readonly Piece WhiteRook = Piece.Rook | Piece.White;
    public static readonly Piece WhiteQueen = Piece.Queen | Piece.White;
    public static readonly Piece WhiteKing = Piece.King | Piece.White;
    
    public static readonly Piece BlackPawn = Piece.Pawn | Piece.Black;
    public static readonly Piece BlackKnight = Piece.Knight | Piece.Black;
    public static readonly Piece BlackBishop = Piece.Bishop | Piece.Black;
    public static readonly Piece BlackRook = Piece.Rook | Piece.Black;
    public static readonly Piece BlackQueen = Piece.Queen | Piece.Black;
    public static readonly Piece BlackKing = Piece.King | Piece.Black;
    
    // look up table for piece char
    public static readonly char[] PieceToChars = new char[16];
    public static readonly Dictionary<char, Piece> CharToPieces = new();
    public static readonly Piece[] ValidPieces = new Piece[]
    {
        WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing,
        BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing,
    };

    static PieceUtils()
    {
        foreach (ref char pieceChar in PieceToChars.AsSpan())
        {
            pieceChar = '?';
        }
        PieceToChars[(int)Piece.Empty] = '.';
        PieceToChars[(int)WhitePawn] = 'P';
        PieceToChars[(int)WhiteKnight] = 'N';
        PieceToChars[(int)WhiteBishop] = 'B';
        PieceToChars[(int)WhiteRook] = 'R';
        PieceToChars[(int)WhiteQueen] = 'Q';
        PieceToChars[(int)WhiteKing] = 'K';
        PieceToChars[(int)BlackPawn] = 'p';
        PieceToChars[(int)BlackKnight] = 'n';
        PieceToChars[(int)BlackBishop] = 'b';
        PieceToChars[(int)BlackRook] = 'r';
        PieceToChars[(int)BlackQueen] = 'q';
        PieceToChars[(int)BlackKing] = 'k';
        
        foreach (var piece in ValidPieces)
        {
            CharToPieces[PieceToChars[(int)piece]] = piece;
        }
    }
    
    public static Piece CreatePiece(Piece piece, Piece color)
    {
        return piece | color;
    }
    
    public static Piece GetColor(Piece piece)
    {
        return piece & Piece.ColorMask;
    }
    
    public static Piece GetPiece(Piece piece)
    {
        return piece & Piece.PieceMask;
    }
}

// 0x88 board representation
public static class BoardUtils
{
    // DONT USE UNLESS NECESSARY
    public const byte FileA = 0;
    public const byte FileB = 1;
    public const byte FileC = 2;
    public const byte FileD = 3;
    public const byte FileE = 4;
    public const byte FileF = 5;
    public const byte FileG = 6;
    public const byte FileH = 7;
    
    public const byte Rank1 = 0;
    public const byte Rank2 = 1;
    public const byte Rank3 = 2;
    public const byte Rank4 = 3;
    public const byte Rank5 = 4;
    public const byte Rank6 = 5;
    public const byte Rank7 = 6;
    public const byte Rank8 = 7;
    
    // VECTORS
    public const int DirectionN = 16;
    public const int DirectionS = -16;
    public const int DirectionE = 1;
    public const int DirectionW = -1;
    public const int DirectionNE = 17;
    public const int DirectionNW = 15;
    public const int DirectionSE = -15;
    public const int DirectionSW = -17;
    
    public const int DirectionNN = DirectionN + DirectionN;
    public const int DirectionSS = DirectionS + DirectionS;
    
    // KNIGHT VECTORS
    public const int DirectionNNE = DirectionNN + DirectionE;
    public const int DirectionNEE = DirectionN + DirectionE + DirectionE;
    public const int DirectionSEE = DirectionS + DirectionE + DirectionE;
    public const int DirectionSSE = DirectionSS + DirectionE;
    public const int DirectionSSW = DirectionSS + DirectionW;
    public const int DirectionSWW = DirectionS + DirectionW + DirectionW;
    public const int DirectionNWW = DirectionN + DirectionW + DirectionW;
    public const int DirectionNNW = DirectionNN + DirectionW;
    
    // algebraic notation helpers
    private static readonly string[] Files = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
    private static readonly string[] Ranks = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
    public const byte InvalidSquare = 0xff;
    
    public static string Square0X88ToAlgebraic(byte square)
    {
        return $"{Files[GetFile(square)]}{Ranks[GetRank(square)]}";
    }
    
    public static byte SquareAlgebraicTo0X88(string square)
    {
        return GetSquare((byte)(square[0] - 'a'), (byte)(square[1] - '1'));
    }

    public static byte GetFile(byte square)
    {
        return (byte)(square & 7);
    }
    
    public static byte GetRank(byte square)
    {
        return (byte)(square >> 4);
    }
    
    public static byte GetSquare(byte file, byte rank)
    {
        unchecked
        {
            return (byte)((byte)(rank << 4) | file);
        }
    }
    
    public static bool IsSquareValid(byte square)
    {
        unchecked
        {
            return (square & 0x88) == 0;
        }
    }
    
    public static int Get8X8Square(byte square)
    {
        unchecked
        {
            return (square + (square & 7)) >> 1; 
        }
    }
    
    public static byte Get0X88Square(int square)
    {
        unchecked
        {
            return (byte)(square + (square & ~7));
        }
    }

    public static int Get0X88Diff(byte a, byte b)
    {
        unchecked
        {
            return 0x77b + a - b;
        }
    }

    public static byte AsByte(this Piece piece)
    {
        return (byte)piece;
    }
    
    public static Piece AsPiece(this byte piece)
    {
        return (Piece)piece;
    }
    
    public const byte Square0X88FileMask = 0b_0000_0111;
    public const byte Square0X88RankMask = 0b_0111_0000;
}

/// <summary>
/// move type flags
/// MSB to LSB:
/// 3 --------------- 2 --------------- 1 --------------- 0 -------
/// promotion ------- capture --------- special 1 ------- special 0
/// </summary>
[Flags]
public enum Move : ushort
{
    // from to position masks
    FromMask = 0b___000_000__111_111,
    ToMask = 0b___111_111__000_000,
    
    PositionMask = FromMask | ToMask,
    
    // move types
    QuietMove = 0b_0000___000_000__000_000,
    DoublePawnPush = 0b_0001___000_000__000_000,
    CastleOO = 0b_0010___000_000__000_000,
    CastleOOO = 0b_0011___000_000__000_000,
    Capture = 0b_0100___000_000__000_000,
    EnPassantCapture = 0b_0101___000_000__000_000,
    KnightPromotion = 0b_1000___000_000__000_000,
    BishopPromotion = 0b_1001___000_000__000_000,
    RookPromotion = 0b_1010___000_000__000_000,
    QueenPromotion = 0b_1011___000_000__000_000,
    KnightPromotionCapture = 0b_1100___000_000__000_000,
    BishopPromotionCapture = 0b_1101___000_000__000_000,
    RookPromotionCapture = 0b_1110___000_000__000_000,
    QueenPromotionCapture = 0b_1111___000_000__000_000,
    
    MoveTypeMask = 0b_1111___000_000__000_000,
}

public static class MoveUtils
{
    public static bool IsQuietMove(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.QuietMove;
    }
    
    public static bool IsDoublePawnPush(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.DoublePawnPush;
    }
    
    public static bool IsCastleOO(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.CastleOO;
    }
    
    public static bool IsCastleOOO(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.CastleOOO;
    }
    
    public static bool IsCapture(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.Capture;
    }
    
    public static bool IsEnPassantCapture(Move move)
    {
        return (move & Move.MoveTypeMask) == Move.EnPassantCapture;
    }
    
    public static Move ConstructQuietMove(byte from, byte to)
    {
        return (Move) (((to & BoardUtils.Square0X88RankMask) << 5) | ((to & BoardUtils.Square0X88FileMask) << 6) | ((from & BoardUtils.Square0X88RankMask) >> 1) | (from & BoardUtils.Square0X88FileMask));
    }
    
    public static void DeconstructMove(Move move, out byte from, out byte to)
    {
        byte moveFrom = (byte)(move & Move.FromMask);
        byte moveTo = (byte)((int)(move & Move.ToMask) >> 6);
        from = (byte)((moveFrom & BoardUtils.Square0X88FileMask) | ((moveFrom << 1) & BoardUtils.Square0X88RankMask));
        to = (byte)((moveTo & BoardUtils.Square0X88FileMask) | ((moveTo << 1) & BoardUtils.Square0X88RankMask));
    }
}

[StructLayout(LayoutKind.Auto)]
public unsafe struct Board
{
    private fixed byte m_Pieces[128];
    private SideToMove m_SideToMove;
    private byte m_EnPassantTargetSquare;
    
    private ushort m_HalfMoveClock;
    private ushort m_FullMoveNumber;
    
    private bool m_WhiteCanCastleOO;
    private bool m_WhiteCanCastleOOO;
    private bool m_BlackCanCastleOO;
    private bool m_BlackCanCastleOOO;

    public enum SideToMove : byte
    {
        White = 1,
        Black = 2
    }
    
    public Piece this[byte square0X88]
    {
        get => m_Pieces[square0X88].AsPiece();
        set => m_Pieces[square0X88] = value.AsByte();
    }
    
    public Board(string fen)
    {
        // parse fen
        string[] parts = fen.Split(' ');
        string[] ranks = parts[0].Split('/');
        byte square8X8 = 0;
        for (int rank = 7; rank >= 0; rank--)
        {
            foreach (char c in ranks[rank])
            {
                if (char.IsDigit(c))
                {
                    square8X8 += (byte)(c - '0');
                }
                else
                {
                    m_Pieces[BoardUtils.Get0X88Square(square8X8)] = PieceUtils.CharToPieces[c].AsByte();
                    square8X8++;
                }
            }
        }
        
        m_SideToMove = parts[1] == "w" ? SideToMove.White : SideToMove.Black;

        // castle flags
        m_WhiteCanCastleOO = false;
        m_WhiteCanCastleOOO = false;
        m_BlackCanCastleOO = false;
        m_BlackCanCastleOOO = false;
        foreach (char c in parts[2])
        {
            switch (c)
            {
                case 'K':
                    m_WhiteCanCastleOO = true;
                    break;
                case 'Q':
                    m_WhiteCanCastleOOO = true;
                    break;
                case 'k':
                    m_BlackCanCastleOO = true;
                    break;
                case 'q':
                    m_BlackCanCastleOOO = true;
                    break;
            }
        }
        
        m_EnPassantTargetSquare = parts[3] == "-" ? BoardUtils.InvalidSquare : BoardUtils.SquareAlgebraicTo0X88(parts[3]);
        
        m_HalfMoveClock = ushort.Parse(parts[4]);
        m_FullMoveNumber = ushort.Parse(parts[5]);
    }
    
    public int GeneratePawnMoves(Move* moves, int capacity)
    {
        int count = 0;
        // for current side to move
        if (m_SideToMove == SideToMove.White)
        {
            // white pawns
            for (byte square = 0; square < 128; square++)
            {
                if (!BoardUtils.IsSquareValid(square)) continue;
                Piece piece = m_Pieces[square].AsPiece();
                if ((piece & Piece.PieceColorMask) != PieceUtils.WhitePawn) continue;
                byte rank = BoardUtils.GetRank(square);
                
                // single push, only if target square is empty
                byte targetSquare = (byte)(square + BoardUtils.DirectionN);
                if ((m_Pieces[targetSquare].AsPiece() & Piece.PieceColorMask) == Piece.Empty)
                {
                    if (count < capacity)
                    {
                        moves[count] = MoveUtils.ConstructQuietMove(square, targetSquare);
                        count++;
                    }
                    else
                    {
                        return count;
                    }
                }
            }
        }
        else
        {

        }

        return count;
    }

    public int GenerateMoves(Move* moves, int capacity)
    {
        // for current side to move
        return 0;
    }

    public string Fen
    {
        get
        {
            string result = "";
            for (int rank = 7; rank >= 0; rank--)
            {
                int empty = 0;
                for (int file = 0; file < 8; file++)
                {
                    byte square = BoardUtils.GetSquare((byte)file, (byte)rank);
                    Piece piece = m_Pieces[square].AsPiece();
                    if (piece == Piece.Empty)
                    {
                        empty++;
                    }
                    else
                    {
                        if (empty > 0)
                        {
                            result += empty;
                            empty = 0;
                        }
                        result += PieceUtils.PieceToChars[(int)piece];
                    }
                }
                if (empty > 0)
                {
                    result += empty;
                }
                if (rank > 0)
                {
                    result += "/";
                }
            }
        
            result += " " + (m_SideToMove == SideToMove.White ? "w" : "b");
        
            string castleFlags = "";
            if (m_WhiteCanCastleOO)
            {
                castleFlags += "K";
            }
            if (m_WhiteCanCastleOOO)
            {
                castleFlags += "Q";
            }
            if (m_BlackCanCastleOO)
            {
                castleFlags += "k";
            }
            if (m_BlackCanCastleOOO)
            {
                castleFlags += "q";
            }
        
            result += " " + (castleFlags.Length == 0 ? "-" : castleFlags);
        
            result += " " + (m_EnPassantTargetSquare == BoardUtils.InvalidSquare ? "-" : BoardUtils.Square0X88ToAlgebraic(m_EnPassantTargetSquare));
        
            result += " " + m_HalfMoveClock;
            result += " " + m_FullMoveNumber;
        
            return result;
        }
    }

    public override string ToString()
    {
        return Fen;
    }
}


