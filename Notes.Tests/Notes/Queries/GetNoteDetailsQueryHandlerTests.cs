using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
{
    private readonly NotesDbContext Context = fixture.Context;
    private readonly IMapper Mapper = fixture.Mapper;

    [Fact]
    public async Task GetNoteDetailsQueryHandler_Success()
    {
        var handler = new GetNoteDetailsQueryHandler(Context, Mapper);

        var result = await handler.Handle(
            new GetNoteDetailsQuery
            {
                UserId = NotesContextFactory.UserBId,
                Id = Guid.Parse("4D4CC26F-C27D-47E4-971F-85963A685CC7")
            },
            CancellationToken.None);

        Assert.IsType<NoteDetailsVm>(result);
        Assert.Equal("Title2", result.Title);
        Assert.Equal(DateTime.Today, result.CreationDate);
    }
}