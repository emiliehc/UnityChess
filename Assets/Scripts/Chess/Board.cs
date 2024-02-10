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
    
    public static readonly Piece[] SlidingPieces = new Piece[]
    {
        Piece.Bishop, Piece.Rook, Piece.Queen
    };

    public static readonly int[][] SlidingPieceDirections = new int[16][];
    
    static BoardUtils()
    {
        SlidingPieceDirections[(int) Piece.Bishop] = BishopSlideDirections;
        SlidingPieceDirections[(int) Piece.Rook] = RookSlideDirections;
        SlidingPieceDirections[(int) Piece.Queen] = QueenSlideDirections;
    }
    
    public static readonly int[] KnightMoves = new int[]
    {
        DirectionNNE, DirectionNEE, DirectionSEE, DirectionSSE, DirectionSSW, DirectionSWW, DirectionNWW, DirectionNNW
    };
    
    public static readonly int[] KingMoves = new int[]
    {
        DirectionN, DirectionNE, DirectionE, DirectionSE, DirectionS, DirectionSW, DirectionW, DirectionNW
    };
    
    public static readonly int[] WhitePawnAttackMoves = new int[]
    {
        DirectionNE, DirectionNW
    };
    
    public static readonly int[] BlackPawnAttackMoves = new int[]
    {
        DirectionSE, DirectionSW
    };
    
    public static readonly int[] BishopSlideDirections = new int[]
    {
        DirectionNE, DirectionSE, DirectionSW, DirectionNW
    };
    
    public static readonly int[] RookSlideDirections = new int[]
    {
        DirectionN, DirectionE, DirectionS, DirectionW
    };
    
    public static readonly int[] QueenSlideDirections = new int[]
    {
        DirectionNE, DirectionSE, DirectionSW, DirectionNW, DirectionN, DirectionE, DirectionS, DirectionW
    };  
    
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
    
    public static int GetManhattanDistance(byte a, byte b)
    {
        return Math.Abs(GetFile(a) - GetFile(b)) + Math.Abs(GetRank(a) - GetRank(b));
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
    
    public static string GetMoveDescription(Move move)
    {
        MoveUtils.DeconstructMove(move, out byte from, out byte to);
        string desc = $"{Square0X88ToAlgebraic(from)} -> {Square0X88ToAlgebraic(to)}";
        // promotion ?
        if ((move & Move.MoveTypeMask) >= Move.KnightPromotion)
        {
            desc += " promotion" + (move & Move.PromotionMask) switch
            {
                Move.KnightPromotion => " knight",
                Move.BishopPromotion => " bishop",
                Move.RookPromotion => " rook",
                Move.QueenPromotion => " queen",
                _ => ""
            };
        }
        
        // capture ?
        if ((move & Move.MoveTypeMask) == Move.Capture)
        {
            desc += " capture";
        }
        
        // en passant ?
        if ((move & Move.MoveTypeMask) == Move.EnPassantCapture)
        {
            desc += " en passant";
        }
        
        // castle ?
        if ((move & Move.MoveTypeMask) == Move.CastleOO)
        {
            desc += " O-O";
        }
        else if ((move & Move.MoveTypeMask) == Move.CastleOOO)
        {
            desc += " O-O-O";
        }
        
        return desc;
    }

    public static unsafe string GetMoveDescriptionWithBoard(in Board board, Move move)
    {
        MoveUtils.DeconstructMove(move, out byte from, out byte to);
        string desc = $"{PieceUtils.PieceToChars[board.m_Pieces[from]]}@{Square0X88ToAlgebraic(from)} -> {PieceUtils.PieceToChars[board.m_Pieces[to]]}@{Square0X88ToAlgebraic(to)}";
        // promotion ?
        if ((move & Move.MoveTypeMask) >= Move.KnightPromotion)
        {
            desc += " promotion" + (move & Move.PromotionMask) switch
            {
                Move.KnightPromotion => " knight",
                Move.BishopPromotion => " bishop",
                Move.RookPromotion => " rook",
                Move.QueenPromotion => " queen",
                _ => ""
            };
        }
        
        // capture ?
        if ((move & Move.Capture) != 0)
        {
            desc += " capture";
        }
        
        // en passant ?
        if ((move & Move.MoveTypeMask) == Move.EnPassantCapture)
        {
            desc += " en passant";
        }
        
        // castle ?
        if ((move & Move.MoveTypeMask) == Move.CastleOO)
        {
            desc += " O-O";
        }
        else if ((move & Move.MoveTypeMask) == Move.CastleOOO)
        {
            desc += " O-O-O";
        }
        
        return desc;
    }
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
    
    PromotionMask = 0b_1011___000_000__000_000,
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
        unchecked
        {
            return (Move) (((to & BoardUtils.Square0X88RankMask) << 5) | ((to & BoardUtils.Square0X88FileMask) << 6) | ((from & BoardUtils.Square0X88RankMask) >> 1) | (from & BoardUtils.Square0X88FileMask));
        }
    }
    
    public static void DeconstructMove(Move move, out byte from, out byte to)
    {
        unchecked
        {
            byte moveFrom = (byte)(move & Move.FromMask);
            byte moveTo = (byte)((int)(move & Move.ToMask) >> 6);
            from = (byte)((moveFrom & BoardUtils.Square0X88FileMask) | ((moveFrom << 1) & BoardUtils.Square0X88RankMask));
            to = (byte)((moveTo & BoardUtils.Square0X88FileMask) | ((moveTo << 1) & BoardUtils.Square0X88RankMask));
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Board
{
    public fixed byte m_Pieces[128];
    public Bitboard m_SquaresAttackedByWhite;
    public Bitboard m_SquaresAttackedByBlack;
    public SideToMove m_SideToMove;
    public byte m_EnPassantTargetSquare;
    
    public ushort m_HalfMoveClock;
    public ushort m_FullMoveNumber;
    
    public bool m_WhiteCanCastleOO;
    public bool m_WhiteCanCastleOOO;
    public bool m_BlackCanCastleOO;
    public bool m_BlackCanCastleOOO;

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
        
        m_SquaresAttackedByWhite = Bitboard.Empty;
        m_SquaresAttackedByBlack = Bitboard.Empty;
        
        m_BlackKingInCheck = false;
        m_WhiteKingInCheck = false;
        
        GenerateAttackMapForSide(m_SideToMove == SideToMove.White ? SideToMove.Black : SideToMove.White);
    }

    public Move GenerateSinglePawnPush(byte from, byte to)
    {
        return MoveUtils.ConstructQuietMove(from, to);
    }
    
    public Move GenerateDoublePawnPush(byte from, byte to)
    {
        return MoveUtils.ConstructQuietMove(from, to) | Move.DoublePawnPush;
    }

    public int GenerateMoves(MoveList* moves)
    {
        Piece currentColor = m_SideToMove == SideToMove.White ? Piece.White : Piece.Black;
        int pawnPushDirection = m_SideToMove == SideToMove.White ? BoardUtils.DirectionN : BoardUtils.DirectionS;
        byte pawnStartRank = m_SideToMove == SideToMove.White ? BoardUtils.Rank2 : BoardUtils.Rank7;
        byte pawnPromotionRank = m_SideToMove == SideToMove.White ? BoardUtils.Rank8 : BoardUtils.Rank1;
        int[] pawnAttackMoves = m_SideToMove == SideToMove.White ? BoardUtils.WhitePawnAttackMoves : BoardUtils.BlackPawnAttackMoves;
        byte enPassantRank = m_SideToMove == SideToMove.White ? BoardUtils.Rank5 : BoardUtils.Rank4;
        bool canCastleOO = m_SideToMove == SideToMove.White ? m_WhiteCanCastleOO : m_BlackCanCastleOO;
        bool canCastleOOO = m_SideToMove == SideToMove.White ? m_WhiteCanCastleOOO : m_BlackCanCastleOOO;
        
        // for current side to move
        // for each square
        for (byte sq = 0; sq < 128; sq++)
        {
            if (sq % 16 >= 8)
            {
                sq += 7;
                continue;
            }
            
            Piece piece = m_Pieces[sq].AsPiece();
            if (piece == Piece.Empty || (piece & Piece.ColorMask) != currentColor)
            {
                continue;
            }
            
            // pawn
            if ((piece & Piece.PieceColorMask) == (Piece.Pawn | currentColor))
            {
                // single push, quiet move, unless a step away from the pawn promotion rank, then promotion
                {
                    byte to = (byte)(sq + pawnPushDirection);
                    if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        if (BoardUtils.GetRank(to) == pawnPromotionRank)
                        {
                            moves->Add(GenerateSinglePawnPush(sq, to) | Move.QueenPromotion, MoveList.PromotionPriority);
                            moves->Add(GenerateSinglePawnPush(sq, to) | Move.RookPromotion, MoveList.PromotionPriority);
                            moves->Add(GenerateSinglePawnPush(sq, to) | Move.BishopPromotion, MoveList.PromotionPriority);
                            moves->Add(GenerateSinglePawnPush(sq, to) | Move.KnightPromotion, MoveList.PromotionPriority);
                        }
                        else
                        {
                            moves->Add(GenerateSinglePawnPush(sq, to), MoveList.PawnPushPriority);
                        }
                    }
                }
                
                // double push, special double pawn push move
                if (BoardUtils.GetRank(sq) == pawnStartRank)
                {
                    byte enPassantTarget = (byte)(sq + pawnPushDirection);
                    byte to = (byte)(enPassantTarget + pawnPushDirection);
                    if ((m_Pieces[enPassantTarget].AsPiece() & Piece.PieceMask) == Piece.Empty && (m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        moves->Add(GenerateDoublePawnPush(sq, to), MoveList.DoublePawnPushPriority);
                    }
                }
                
                // pawn captures, unless a step away from the pawn promotion rank, then promotion capture
                foreach (int pawnAttackMove in pawnAttackMoves)
                {
                    byte to = (byte)(sq + pawnAttackMove);
                    // en passant
                    if (to == m_EnPassantTargetSquare)
                    {
                        moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.EnPassantCapture, MoveList.PawnCapturePriority);
                        continue;
                    }
                    
                    if (BoardUtils.IsSquareValid(to))
                    {
                        if ((m_Pieces[to].AsPiece() & Piece.ColorMask) != currentColor &&
                            (m_Pieces[to].AsPiece() & Piece.PieceMask) != Piece.Empty)
                        {
                            // king
                            if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.King)
                            {
                                // forced to capture the king
                                moves->Clear();
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, 0);
                                return 1;
                            }
                        
                            if (BoardUtils.GetRank(to) == pawnPromotionRank)
                            {
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.QueenPromotionCapture, MoveList.CapturePromotionPriority);
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.RookPromotionCapture, MoveList.CapturePromotionPriority);
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.BishopPromotionCapture, MoveList.CapturePromotionPriority);
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.KnightPromotionCapture, MoveList.CapturePromotionPriority);
                            }
                            else
                            {
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, MoveList.PawnCapturePriority);
                            }
                        }
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.Knight | currentColor))
            {
                foreach (int knightMove in BoardUtils.KnightMoves)
                {
                    byte to = (byte)(sq + knightMove);
                    if (!BoardUtils.IsSquareValid(to)) continue;
                    // see if empty
                    if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        moves->Add(MoveUtils.ConstructQuietMove(sq, to), MoveList.KnightMovePriority);
                    }
                    else
                    {
                        // see if capture
                        if ((m_Pieces[to].AsPiece() & Piece.ColorMask) != currentColor)
                        {
                            // king ?
                            if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.King)
                            {
                                // forced to capture the king
                                moves->Clear();
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, 0);
                                return 1;
                            }
                                
                            moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, MoveList.KnightCapturePriority);
                        }
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.Bishop | currentColor) || (piece & Piece.PieceColorMask) == (Piece.Queen | currentColor) || (piece & Piece.PieceColorMask) == (Piece.Rook | currentColor))
            {
                Piece pieceCode = piece & Piece.PieceMask;
                
                int[] slidingDirections = BoardUtils.SlidingPieceDirections[(int)pieceCode];
                
                foreach (int direction in slidingDirections)
                {
                    int to = sq + direction;
                    while (BoardUtils.IsSquareValid((byte)to))
                    {
                        // see if empty
                        if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Empty)
                        {
                            moves->Add(MoveUtils.ConstructQuietMove(sq, (byte)to), 3);
                        }
                        else
                        {
                            // see if capture
                            if ((m_Pieces[to].AsPiece() & Piece.ColorMask) != currentColor)
                            {
                                // king ?
                                if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.King)
                                {
                                    // forced to capture the king
                                    moves->Clear();
                                    moves->Add(MoveUtils.ConstructQuietMove(sq, (byte)to) | Move.Capture, 0);
                                    return 1;
                                }
                                
                                moves->Add(MoveUtils.ConstructQuietMove(sq, (byte)to) | Move.Capture, 2);
                            }
                            break;
                        }
                        to += direction;
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.King | currentColor))
            {
                // king
                // movements
                foreach (int dir in BoardUtils.KingMoves)
                {
                    byte to = (byte)(sq + dir);
                    if (!BoardUtils.IsSquareValid(to)) continue;
                    // see if empty
                    if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        moves->Add(MoveUtils.ConstructQuietMove(sq, to), MoveList.KingMovePriority);
                    }
                    else
                    {
                        // see if capture
                        if ((m_Pieces[to].AsPiece() & Piece.ColorMask) != currentColor)
                        {
                            // king ?
                            if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.King)
                            {
                                // forced to capture the king
                                moves->Clear();
                                moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, 0);
                                return 1;
                            }
                                
                            moves->Add(MoveUtils.ConstructQuietMove(sq, to) | Move.Capture, MoveList.KingCapturePriority);
                        }
                    }
                }
                
                // O-O
                if (canCastleOO)
                {
                    byte rookTo = (byte)(sq + BoardUtils.DirectionE);
                    byte kingTo = (byte)(sq + BoardUtils.DirectionE + BoardUtils.DirectionE);
                    // check if both squares are empty
                    if ((m_Pieces[rookTo].AsPiece() & Piece.PieceMask) == Piece.Empty && (m_Pieces[kingTo].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        moves->Add(MoveUtils.ConstructQuietMove(sq, kingTo) | Move.CastleOO, MoveList.CastlePriority);
                    }
                }
                
                // O-O-O
                if (canCastleOOO)
                {
                    byte rookTo = (byte)(sq + BoardUtils.DirectionW);
                    byte kingTo = (byte)(sq + BoardUtils.DirectionW + BoardUtils.DirectionW);
                    byte intermediate = (byte)(sq + BoardUtils.DirectionW + BoardUtils.DirectionW + BoardUtils.DirectionW);
                    // check if all squares are empty
                    if ((m_Pieces[rookTo].AsPiece() & Piece.PieceMask) == Piece.Empty && (m_Pieces[kingTo].AsPiece() & Piece.PieceMask) == Piece.Empty && (m_Pieces[intermediate].AsPiece() & Piece.PieceMask) == Piece.Empty)
                    {
                        moves->Add(MoveUtils.ConstructQuietMove(sq, kingTo) | Move.CastleOOO, MoveList.CastlePriority);
                    }
                }
            }
        }
        
        return moves->Count;
    }

    private bool m_WhiteKingInCheck;
    private bool m_BlackKingInCheck;

    public bool WhiteKingInCheck => m_WhiteKingInCheck;
    public bool BlackKingInCheck => m_BlackKingInCheck;

    public void GenerateAttackMapForSide(SideToMove side)
    {
        Piece currentColor = side == SideToMove.White ? Piece.White : Piece.Black;
        int pawnPushDirection = side == SideToMove.White ? BoardUtils.DirectionN : BoardUtils.DirectionS;
        byte pawnStartRank = side == SideToMove.White ? BoardUtils.Rank2 : BoardUtils.Rank7;
        byte pawnPromotionRank = side == SideToMove.White ? BoardUtils.Rank8 : BoardUtils.Rank1;
        int[] pawnAttackMoves = side == SideToMove.White ? BoardUtils.WhitePawnAttackMoves : BoardUtils.BlackPawnAttackMoves;
        byte enPassantRank = side == SideToMove.White ? BoardUtils.Rank5 : BoardUtils.Rank4;
        bool canCastleOO = side == SideToMove.White ? m_WhiteCanCastleOO : m_BlackCanCastleOO;
        bool canCastleOOO = side == SideToMove.White ? m_WhiteCanCastleOOO : m_BlackCanCastleOOO;
        Bitboard squaresAttackedBySide = Bitboard.Empty;
        Piece enemyKing = side == SideToMove.White ? PieceUtils.BlackKing : PieceUtils.WhiteKing;
        
        bool oppositeKingInCheck = false;
        
        // for current side to move
        // for each square
        for (byte sq = 0; sq < 128; sq++)
        {
            if (sq % 16 >= 8)
            {
                sq += 7;
                continue;
            }
            
            Piece piece = m_Pieces[sq].AsPiece();
            if (piece == Piece.Empty || (piece & Piece.ColorMask) != currentColor)
            {
                continue;
            }
            
            // pawn
            if ((piece & Piece.PieceColorMask) == (Piece.Pawn | currentColor))
            {
                // pawn captures, unless a step away from the pawn promotion rank, then promotion capture
                foreach (int pawnAttackMove in pawnAttackMoves)
                {
                    byte to = (byte)(sq + pawnAttackMove);
                    if (BoardUtils.IsSquareValid(to))
                    {
                        BitboardUtils.SetBitTrue(&squaresAttackedBySide, to);
                        
                        // is it king
                        if (m_Pieces[to].AsPiece() == enemyKing)
                        {
                            oppositeKingInCheck = true;
                        }
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.Knight | currentColor))
            {
                foreach (int knightMove in BoardUtils.KnightMoves)
                {
                    byte to = (byte)(sq + knightMove);
                    if (!BoardUtils.IsSquareValid(to)) continue;
                    BitboardUtils.SetBitTrue(&squaresAttackedBySide, to);
                    // is it king
                    if (m_Pieces[to].AsPiece() == enemyKing)
                    {
                        oppositeKingInCheck = true;
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.Bishop | currentColor) || (piece & Piece.PieceColorMask) == (Piece.Queen | currentColor) || (piece & Piece.PieceColorMask) == (Piece.Rook | currentColor))
            {
                Piece pieceCode = piece & Piece.PieceMask;
                
                int[] slidingDirections = BoardUtils.SlidingPieceDirections[(int)pieceCode];
                
                foreach (int direction in slidingDirections)
                {
                    int to = sq + direction;
                    while (BoardUtils.IsSquareValid((byte)to))
                    {
                        BitboardUtils.SetBitTrue(&squaresAttackedBySide, (byte)to);
                        // is it king
                        if (m_Pieces[to].AsPiece() == enemyKing)
                        {
                            oppositeKingInCheck = true;
                        }
                        if ((m_Pieces[to].AsPiece() & Piece.PieceMask) != Piece.Empty)
                        {
                            break;
                        }
                        to += direction;
                    }
                }
            }
            else if ((piece & Piece.PieceColorMask) == (Piece.King | currentColor))
            {
                // king
                // movements
                foreach (int dir in BoardUtils.KingMoves)
                {
                    byte to = (byte)(sq + dir);
                    if (!BoardUtils.IsSquareValid(to)) continue;
                    BitboardUtils.SetBitTrue(&squaresAttackedBySide, to);
                    // is it king
                    if (m_Pieces[to].AsPiece() == enemyKing)
                    {
                        oppositeKingInCheck = true;
                    }
                }
            }
        }
        
        m_BlackKingInCheck = false;
        m_WhiteKingInCheck = false;
        
        if (side == SideToMove.White)
        {
            m_SquaresAttackedByWhite = squaresAttackedBySide;
            m_BlackKingInCheck = oppositeKingInCheck;
        }
        else
        {
            m_SquaresAttackedByBlack = squaresAttackedBySide;
            m_WhiteKingInCheck = oppositeKingInCheck;
        }
    }
    
    public void MakeMove(Move move)
    {
        // quiet move
        bool halfMoveClockReset = false;
        MoveUtils.DeconstructMove(move, out byte from, out byte to);
        Move moveType = move & Move.MoveTypeMask;
        int pawnPushDirection = m_SideToMove == SideToMove.White ? BoardUtils.DirectionN : BoardUtils.DirectionS;
        Piece sideToMoveFlag = m_SideToMove == SideToMove.White ? Piece.White : Piece.Black;
        
        switch (moveType)
        {
            case Move.QuietMove:
                // is it pawn push
                if ((m_Pieces[from].AsPiece() & Piece.PieceMask) == Piece.Pawn)
                {
                    halfMoveClockReset = true;
                }
                // no capture, no promotion, no en passant, no castle
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                break;
            case Move.DoublePawnPush:
                // double pawn push
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                m_EnPassantTargetSquare = (byte)(from + (to - from) / 2);
                halfMoveClockReset = true;
                break;
            case Move.CastleOO:
                // O-O
                // move king from from square to to square
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                // move rook to the west of the king
                m_Pieces[to + BoardUtils.DirectionW] = m_Pieces[to + BoardUtils.DirectionE];
                m_Pieces[to + BoardUtils.DirectionE] = Piece.Empty.AsByte();
                // remove castling rights
                if (m_SideToMove == SideToMove.White)
                {
                    m_WhiteCanCastleOO = false;
                    m_WhiteCanCastleOOO = false;
                }
                else
                {
                    m_BlackCanCastleOO = false;
                    m_BlackCanCastleOOO = false;
                }
                break;
            case Move.CastleOOO:
                // O-O-O
                // move king from from square to to square
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                // move rook to the east of the king
                m_Pieces[to + BoardUtils.DirectionE] = m_Pieces[to + BoardUtils.DirectionW + BoardUtils.DirectionW];
                m_Pieces[to + BoardUtils.DirectionW + BoardUtils.DirectionW] = Piece.Empty.AsByte();
                // remove castling rights
                if (m_SideToMove == SideToMove.White)
                {
                    m_WhiteCanCastleOO = false;
                    m_WhiteCanCastleOOO = false;
                }
                else
                {
                    m_BlackCanCastleOO = false;
                    m_BlackCanCastleOOO = false;
                }
                break;
            case Move.Capture:
                // capture
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                halfMoveClockReset = true;
                break;
            case Move.EnPassantCapture:
                // en passant capture
                m_Pieces[to] = m_Pieces[from];
                m_Pieces[from] = Piece.Empty.AsByte();
                m_Pieces[to - pawnPushDirection] = Piece.Empty.AsByte();
                m_EnPassantTargetSquare = BoardUtils.InvalidSquare;
                halfMoveClockReset = true;
                break;
            case Move.KnightPromotion:
                m_Pieces[to] = (Piece.Knight | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                break;
            case Move.BishopPromotion:
                m_Pieces[to] = (Piece.Bishop | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                break;
            case Move.RookPromotion:
                m_Pieces[to] = (Piece.Rook | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                break;
            case Move.QueenPromotion:
                m_Pieces[to] = (Piece.Queen | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                break;
            case Move.KnightPromotionCapture:
                m_Pieces[to] = (Piece.Knight | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                halfMoveClockReset = true;
                break;
            case Move.BishopPromotionCapture:
                m_Pieces[to] = (Piece.Bishop | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                halfMoveClockReset = true;
                break;
            case Move.RookPromotionCapture:
                m_Pieces[to] = (Piece.Rook | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                halfMoveClockReset = true;
                break;
            case Move.QueenPromotionCapture:
                m_Pieces[to] = (Piece.Queen | sideToMoveFlag).AsByte();
                m_Pieces[from] = Piece.Empty.AsByte();
                halfMoveClockReset = true;
                break;
        }
        
        // if king move
        if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.King)
        {
            if (m_SideToMove == SideToMove.White)
            {
                m_WhiteCanCastleOO = false;
                m_WhiteCanCastleOOO = false;
            }
            else
            {
                m_BlackCanCastleOO = false;
                m_BlackCanCastleOOO = false;
            }
        }
        
        // if caslting rights already lost, no need to check
        if (m_WhiteCanCastleOO || m_WhiteCanCastleOOO || m_BlackCanCastleOO || m_BlackCanCastleOOO)
        {
            // if rook move (si une tour bouge de sa case de départ)
            if ((m_Pieces[to].AsPiece() & Piece.PieceMask) == Piece.Rook)
            {
                if (m_SideToMove == SideToMove.White)
                {
                    if (from == BoardUtils.GetSquare(BoardUtils.FileA, BoardUtils.Rank1))
                    {
                        m_WhiteCanCastleOOO = false;
                    }
                    else if (from == BoardUtils.GetSquare(BoardUtils.FileH, BoardUtils.Rank1))
                    {
                        m_WhiteCanCastleOO = false;
                    }
                }
                else
                {
                    if (from == BoardUtils.GetSquare(BoardUtils.FileA, BoardUtils.Rank8))
                    {
                        m_BlackCanCastleOOO = false;
                    }
                    else if (from == BoardUtils.GetSquare(BoardUtils.FileH, BoardUtils.Rank8))
                    {
                        m_BlackCanCastleOO = false;
                    }
                }
            }
        
            if ((move & Move.Capture) != 0)
            {
                if (m_SideToMove == SideToMove.White)
                {
                    if (to == BoardUtils.GetSquare(BoardUtils.FileA, BoardUtils.Rank8))
                    {
                        m_BlackCanCastleOOO = false;
                    }
                    else if (to == BoardUtils.GetSquare(BoardUtils.FileH, BoardUtils.Rank8))
                    {
                        m_BlackCanCastleOO = false;
                    }
                }
                else
                {
                    if (to == BoardUtils.GetSquare(BoardUtils.FileA, BoardUtils.Rank1))
                    {
                        m_WhiteCanCastleOOO = false;
                    }
                    else if (to == BoardUtils.GetSquare(BoardUtils.FileH, BoardUtils.Rank1))
                    {
                        m_WhiteCanCastleOO = false;
                    }
                }
            }
        }


        
        
        
        if (moveType != Move.DoublePawnPush)
        {
            m_EnPassantTargetSquare = BoardUtils.InvalidSquare;
        }
        
        // move number and half move clock
        if (m_SideToMove == SideToMove.Black)
        {
            m_FullMoveNumber++;
        }

        if (halfMoveClockReset)
        {
            m_HalfMoveClock = 0;
        }
        else
        {
            m_HalfMoveClock++;
        }
        
        GenerateAttackMapForSide(m_SideToMove);
        
        // flip side to move
        m_SideToMove = m_SideToMove == SideToMove.White ? SideToMove.Black : SideToMove.White;
        GenerateAttackMapForSide(m_SideToMove);
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


