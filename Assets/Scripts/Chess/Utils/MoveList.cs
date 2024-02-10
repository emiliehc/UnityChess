using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public unsafe ref struct MoveList
{
    public const int MaxCount = 256;
    public const int ForcedPseudoLegalMovePriority = 0;
    public const int ForcedMovePriority = 0;
    public const int MatePriority = 0;
    public const int PawnPushPriority = 5;
    public const int PromotionPriority = 2;
    public const int PawnCapturePriority = 5;
    public const int CapturePromotionPriority = 1;
    public const int DoublePawnPushPriority = 3;
    public const int KnightMovePriority = 6;
    public const int KnightCapturePriority = 4;
    public const int KingMovePriority = 4;
    public const int KingCapturePriority = 3;
    public const int BishopMovePriority = 7;
    public const int BishopCapturePriority = 5;
    public const int RookMovePriority = 3;
    public const int RookCapturePriority = 2;
    public const int QueenMovePriority = 2;
    public const int QueenCapturePriority = 1;
    public const int CastlePriority = 3;
    
    
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    private struct MoveQueueItem
    {
        [FieldOffset(0)]
        public Move Move;
        [FieldOffset(2)]
        public ushort Priority; // 0 is the highest priority
    }
    
    private fixed uint MoveQueueItems[MaxCount];
    private int m_Count;
    public int Count => m_Count;

    public void Clear()
    {
        m_Count = 0;
    }
    
    public void Add(Move move, ushort priority)
    {
        if (m_Count == MaxCount)
        {
            return;
        }
        
        fixed (uint* moveQueueItems = MoveQueueItems)
        {
            int i = m_Count;
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (priority >= ((MoveQueueItem*)moveQueueItems)[parent].Priority)
                {
                    break;
                }
                ((MoveQueueItem*)moveQueueItems)[i] = ((MoveQueueItem*)moveQueueItems)[parent];
                i = parent;
            }
            ((MoveQueueItem*)moveQueueItems)[i].Move = move;
            ((MoveQueueItem*)moveQueueItems)[i].Priority = priority;
        }
        m_Count++;
    }
    
    private void MinHeapify(int i)
    {
        fixed (uint* moveQueueItems = MoveQueueItems)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;
            if (left < m_Count && ((MoveQueueItem*)moveQueueItems)[left].Priority < ((MoveQueueItem*)moveQueueItems)[i].Priority)
            {
                smallest = left;
            }
            if (right < m_Count && ((MoveQueueItem*)moveQueueItems)[right].Priority < ((MoveQueueItem*)moveQueueItems)[smallest].Priority)
            {
                smallest = right;
            }
            if (smallest != i)
            {
                (((MoveQueueItem*)moveQueueItems)[i], ((MoveQueueItem*)moveQueueItems)[smallest]) = (((MoveQueueItem*)moveQueueItems)[smallest], ((MoveQueueItem*)moveQueueItems)[i]);
                MinHeapify(smallest);
            }
        }
    }
    
    public Move Take()
    {
        if (m_Count == 0)
        {
            return 0;
        }
        
        fixed (uint* moveQueueItems = MoveQueueItems)
        {
            Move move = ((MoveQueueItem*)moveQueueItems)[0].Move;
            m_Count--;
            ((MoveQueueItem*)moveQueueItems)[0] = ((MoveQueueItem*)moveQueueItems)[m_Count];
            MinHeapify(0);
            return move;
        }
    }
}