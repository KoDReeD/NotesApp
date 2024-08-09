using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Middleware;
using NotesApi.Models;
using NotesApi.Models.Notes.Request;
using NotesApi.Models.Tag;

namespace NotesApi.Services;

public interface ITagService
{
    public Task<Page<Tag>> GetAllByUserAsync(RequestFilter filter, int id);
    public Task<Tag> GetByIdAsync(int id);
    public Task<Tag> CreateAsync(TagRequest model);
    public Task<bool> DeleteAsync(int id);
    public Task<Tag> UpdateAsync(TagRequest model);
}

public class TagServices : ITagService
{
    private readonly ApplicatonDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Account _currentUser;
    private readonly IMapper _mapper;

    public TagServices(ApplicatonDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _currentUser = (Account)_httpContextAccessor.HttpContext.Items["Account"];
    }

    public async Task<Page<Tag>> GetAllByUserAsync(RequestFilter filter, int id)
    {
        var query = _context.Tags
            .Include(x => x.WhoCreated)
            .Where(x => x.WhoCreatedId == _currentUser.Id);
        return new Page<Tag>(filter)
        {
            Objects = await query.ToListAsync(),
            ObjectsCount = await query.CountAsync()
        };
    }

    public async Task<Tag> GetByIdAsync(int id)
    {
        return await _context.Tags
                   .Include(x => x.Notes)
                   .FirstOrDefaultAsync(x => x.Id == id
                                             && x.WhoCreatedId == _currentUser.Id) ??
               throw new KeyNotFoundException($"Тэг с ID[{id}] не найден");
    }

    public async Task<Tag> CreateAsync(TagRequest model)
    {
        var dbNote = _mapper.Map<Tag>(model);
        dbNote.CreatedDate = DateTime.Now;
        dbNote.WhoCreatedId = _currentUser.Id;

        var allDbTags = _context.Tags
            .Where(x => x.WhoCreatedId == _currentUser.Id)
            .ToList();

        await _context.Tags.AddAsync(dbNote);
        await _context.SaveChangesAsync();

        return dbNote;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tagToDelete = await GetByIdAsync(id);
        if (tagToDelete.Notes.Count > 0)
            throw new AppException($"Тэг с [{id}] нельзя удалить, он используется в заметках");
        
        _context.Tags.Remove(tagToDelete);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Tag> UpdateAsync(TagRequest model)
    {
        var dbModel = _mapper.Map<Tag>(model);
        _context.Tags.Update(dbModel);
        await _context.SaveChangesAsync();
        return dbModel;
    }
}