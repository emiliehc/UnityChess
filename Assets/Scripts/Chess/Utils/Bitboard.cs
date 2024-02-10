// ulong for bitboards

using System;
using System.Runtime.CompilerServices;

[Flags]
public enum Bitboard : ulong
{
    Empty = 0UL,
}

public static unsafe class BitboardUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetBit(this Bitboard bb, byte square)
    {
        int square8X8 = BoardUtils.Get8X8Square(square);
        return (bb & ((Bitboard)(1UL << square8X8))) != Bitboard.Empty;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetBitTrue(Bitboard* bb, byte square)
    {
        int square8X8 = BoardUtils.Get8X8Square(square);
        *bb |= (Bitboard)(1UL << square8X8);
    }
}
