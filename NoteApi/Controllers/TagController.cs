using Microsoft.AspNetCore.Mvc;
using NotesApi.DbModels;
using NotesApi.Models;
using NotesApi.Models.Notes.Request;
using NotesApi.Models.Tag;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<Page<Tag>>> GetAllByUser([FromQuery]RequestFilter filter, int id)
    {
        return Ok(await _tagService.GetAllByUserAsync(filter, id));
    }
    
    [HttpGet]
    public async Task<ActionResult<Tag>> GetNoteById(int id)
    {
        return Ok(await _tagService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> Create(TagRequest model)
    {
        return Ok(await _tagService.CreateAsync(model));
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        return Ok(await _tagService.DeleteAsync(id));
    }
    
    [HttpPut]
    public async Task<ActionResult<bool>> Update(int id, [FromBody]TagRequest model)
    {
        return Ok(await _tagService.UpdateAsync(id, model));
    }
}