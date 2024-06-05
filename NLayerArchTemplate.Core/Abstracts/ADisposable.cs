namespace NLayerArchTemplate.Core.Abstracts;

public abstract class ADisposable : IDisposable
{
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                DisposeManagedResources();
            }
            DisposeUnmanagedResources();
            _disposed = true;
        }
    }
    /// <summary>
    /// Free Any Managed Resources Here
    /// </summary>
    protected virtual void DisposeManagedResources() { }

    /// <summary>
    /// Free Any Unmanaged Resources Here
    /// </summary>
    protected virtual void DisposeUnmanagedResources() { }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~ADisposable()
    {
        Dispose(false);
    }
}