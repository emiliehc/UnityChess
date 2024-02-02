using System;
using System.Runtime.InteropServices;

public struct Any
{
}

public unsafe class Ref<T> : IDisposable where T : unmanaged
{
    private T* m_Value;
    
    public Ref()
    {
        // malloc
        m_Value = (T*)Marshal.AllocHGlobal(sizeof(T));
    }

    public Ref(nint size)
    {
        m_Value = (T*)Marshal.AllocHGlobal(size);
    }
    
    ~Ref()
    {
        Dispose();
    }

    public void Dispose()
    {
        // free
        Marshal.FreeHGlobal((IntPtr)m_Value);
        m_Value = null;
        GC.SuppressFinalize(this);
    }
    
    public T* Value => m_Value;
}