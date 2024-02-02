using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Game
{
    private Board* m_Boards;
    private int m_BoardsCount;
    public const string StartingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    public Game(string fen)
    {
        m_Boards = (Board*)Marshal.AllocHGlobal(sizeof(Board) * 1024);
        m_BoardsCount = 0;
        Board initBoard = new Board(fen);
        PushBoard(&initBoard);
    }
    
    public Board* currentBoard => &m_Boards[m_BoardsCount - 1];

    private void PushBoard(Board* b)
    {
        m_Boards[m_BoardsCount++] = *b;
    }

    private void PushBoard()
    {
        m_Boards[m_BoardsCount] = m_Boards[m_BoardsCount - 1];
        m_BoardsCount++;
    }
    
    private void PopBoard()
    {
        m_BoardsCount--;
    }
    
    public void MakeMove(Move move)
    {
        PushBoard();
        currentBoard->MakeMove(move);
    }
    
    public void UnmakeMove()
    {
        PopBoard();
    }
    
    public string Fen => currentBoard->Fen;
}