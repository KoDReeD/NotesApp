using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Models;

namespace NotesApi.Services;

public class NoteService
{
    private readonly ApplicatonDbContext _context;

    public NoteService(ApplicatonDbContext context)
    {
        _context = context;
    }

    public async Task<Page<Note>> GetAllByUser(RequestFilter filter, int id)
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
    
}