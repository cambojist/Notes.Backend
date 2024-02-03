using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistence;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteListQueryHandlerTests(QueryTestFixture fixture) : TestCommandBase
{
    private readonly NotesDbContext Context = fixture.Context;
    private readonly IMapper Mapper = fixture.Mapper;

    [Fact]
    public async void GetNoteListQueryHandler_Success()
    {
        var handler = new GetNoteListQueryHandler(Context, Mapper);

        var result = await handler.Handle(
            new GetNoteListQuery
            {
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

        Assert.IsType<NoteListVm>(result);
        Assert.Equal(2, result.Notes.Count);
    }
}