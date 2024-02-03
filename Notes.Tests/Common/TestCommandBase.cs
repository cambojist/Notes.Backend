using Notes.Persistence;

namespace Notes.Tests.Common;

public abstract class TestCommandBase : IDisposable
{
    protected readonly NotesDbContext Context = NotesContextFactory.Create();

    public void Dispose()
    {
        NotesContextFactory.Destroy(Context);
    }
}