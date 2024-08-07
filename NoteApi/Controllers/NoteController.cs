using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NotesApi.DbModels;
using NotesApi.Models;
using NotesApi.Models.Notes.Request;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NoteController : ControllerBase
{
    private NoteService _notesService;

    public NoteController(NoteService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    public async Task<ActionResult<Page<Note>>> GetAllByUser(RequestFilter filter, int id)
    {
        return Ok(await _notesService.GetAllByUserAsync(filter, id));
    }
    
    [HttpGet]
    public async Task<ActionResult<Note>> GetNoteById(int id)
    {
        return Ok(await _notesService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<Note>> Create(NoteRequest model)
    {
        return Ok(await _notesService.CreateAsync(model));
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> Create(int id)
    {
        return Ok(await _notesService.DeleteAsync(id));
    }
    
    [HttpPut]
    public async Task<ActionResult<bool>> Update(NoteRequest model)
    {
        return Ok(await _notesService.UpdateAsync(model));
    }
}