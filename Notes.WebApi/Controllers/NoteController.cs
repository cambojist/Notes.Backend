using Asp.Versioning;
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
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("Api/{version:ApiVersion}/[controller]")]
[Produces("application/json")]
public class NoteController(IMapper mapper) : BaseController
{
    /// <summary>
    ///     Gets the list of notes
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     GET /note
    /// </remarks>
    /// <returns>Return NoteListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteListVm>> GetAll()
    {
        var query = new GetNoteListQuery
        {
            UserId = UserId
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    ///     Gets the note by id
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     GET /note/D34D349E-43B8-429E-BCA4-793C932FD580
    /// </remarks>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns NoteDetailsVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user in unauthorized</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    ///     Creates the note
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     POST /note
    ///     {
    ///     title: "note title",
    ///     details: "note details"
    ///     }
    /// </remarks>
    /// <param name="createNoteDto">CreateNoteDto object</param>
    /// <returns>Returns id (guid)</returns>
    /// <response code="201">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;

        var noteId = await Mediator.Send(command);
        return Created(noteId.ToString(), noteId);
    }

    /// <summary>
    ///     Updates the note
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     PUT /note
    ///     {
    ///     title: "updated note title"
    ///     }
    /// </remarks>
    /// <param name="updateNoteDto">UpdateNoteDto object</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Deletes the note by id
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     DELETE /note/88DEB432-062F-43DE-8DCD-8B6EF79073D3
    /// </remarks>
    /// <param name="id">Id of the note (guid)</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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