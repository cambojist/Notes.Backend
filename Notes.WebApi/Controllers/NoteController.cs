using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers;

[Authorize]
[Route("Api/[controller]")]
public class NoteController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<NoteListVm>> GetAll()
    {
        var query = new GetNoteListQuery
        {
            UserId = UserId
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
    {
        var query = new GetNoteDetailsQuery
        {
            UserId = UserId,
            Id = id
        };

        var vm = await Mediator.Send(query);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;

        var noteId = await Mediator.Send(command);
        return Created(noteId.ToString(), noteId);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteNoteCommand
        {
            UserId = UserId,
            Id = id
        };

        await Mediator.Send(command);
        return NoContent();
    }
}