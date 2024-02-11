// ulong for bitboards

using System;
using System.Numerics;
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long CountBits(long i)
    {
        i = i - ((i >> 1) & 0x5555555555555555);
        i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
        return (((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
    }
}
