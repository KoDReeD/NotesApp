using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Models;
using NotesApi.Models.Notes.Request;

namespace NotesApi.Services;

public class NoteService
{
    private readonly ApplicatonDbContext _context;
    private readonly IMapper _mapper;

    public NoteService(ApplicatonDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Page<Note>> GetAllByUserAsync(RequestFilter filter, int id)
    {
        var query = _context.Notes
            .Include(x => x.WhoCreated)
            .Include(x => x.WhoUpdated)
            .Where(x => x.WhoCreatedId == id);
        return new Page<Note>(filter)
        {
            Objects = await query.ToListAsync(),
            ObjectsCount = await query.CountAsync()
        };
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Note> CreateAsync(NoteRequest model)
    {
        var dbModel = _mapper.Map<Note>(model);
        dbModel.CreatedDate = DateTime.Now;
        await _context.Notes.AddAsync(dbModel);
        await _context.SaveChangesAsync();
        return dbModel;
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