// ulong for bitboards

using System;

[Flags]
public enum Bitboard : ulong
{
    Empty = 0UL,
}

public static unsafe class BitboardUtils
{
    public static bool GetBit(this Bitboard bb, byte square)
    {
        int square8X8 = BoardUtils.Get8X8Square(square);
        return (bb & ((Bitboard)(1UL << square8X8))) != Bitboard.Empty;
    }
    
    public static void SetBitTrue(Bitboard* bb, byte square)
    {
        int square8X8 = BoardUtils.Get8X8Square(square);
        *bb |= (Bitboard)(1UL << square8X8);
    }
}
