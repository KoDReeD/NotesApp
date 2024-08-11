using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Models;
using NotesApi.Models.Notes.Request;

namespace NotesApi.Services;

public interface INoteService
{
    public Task<Page<Note>> GetAllByUserAsync(RequestFilter filter, int id);
    public Task<Note> GetByIdAsync(int id);
    public Task<Note> CreateAsync(NoteRequest model);
    public Task<bool> DeleteAsync(int id);
    public Task<Note> UpdateAsync(NoteRequest model);
}

public class NoteService : INoteService
{
    private readonly ApplicatonDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Account _currentUser;

    public NoteService(ApplicatonDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _currentUser = (Account)_httpContextAccessor.HttpContext.Items["Account"];
    }

    public async Task<Page<Note>> GetAllByUserAsync(RequestFilter filter, int id)
    {
        var query = _context.Notes
            .Include(x => x.AccountCreated)
            .Where(x => x.AccountCreatedId == _currentUser.Id);
        return new Page<Note>(filter)
        {
            Objects = await query.ToListAsync(),
            ObjectsCount = await query.CountAsync()
        };
    }

    public async Task<Note> GetByIdAsync(int id)
    {
        return await _context.Notes
                   .Include(x => x.AccountCreated)
                   .FirstOrDefaultAsync(x => x.Id == id && x.AccountCreatedId == _currentUser.Id)
               ?? throw new KeyNotFoundException();
    }

    public async Task<Note> CreateAsync(NoteRequest model)
    {
        var dbNote = _mapper.Map<Note>(model);
        dbNote.CreatedDate = DateTime.Now;
        dbNote.AccountCreatedId = _currentUser.Id;

        var allDbTags = _context.Tags
            .Where(x => x.AccountCreatedId == _currentUser.Id)
            .ToList();

        await _context.Notes.AddAsync(dbNote);
        await _context.SaveChangesAsync();

        foreach (var tag in dbNote.NoteTags)
        {
            var existingTag = allDbTags.Any(x => x.Id == tag.Id);
            if (!existingTag) throw new KeyNotFoundException($"Тег с ID[{tag.Id}] не найден");
            var noteTag = new NoteTags()
            {
                NoteId = dbNote.Id,
                TagId = tag.Id,
                AccountCreatedId = _currentUser.Id
            };
            await _context.NoteTags.AddAsync(noteTag);
        }

        return dbNote;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var noteToDelete = await GetByIdAsync(id);
        _context.Notes.Remove(noteToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Note> UpdateAsync(NoteRequest model)
    {
        var dbModel = _mapper.Map<Note>(model);
        dbModel.UpdatedDate = DateTime.Now;
        _context.Notes.Update(dbModel);
        await _context.SaveChangesAsync();
        return dbModel;
    }
}