using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Middleware;
using NotesApi.Models;
using NotesApi.Models.Tag;
using NotesApi.Models.Tag.Response;

namespace NotesApi.Services;

public interface ITagService
{
    public Task<Page<TagResponse>> GetAllByUserAsync(RequestFilter filter, int id);
    public Task<TagResponse> GetByIdAsync(int id);
    public Task<Tag> GetByIdDbAsync(int id);
    public Task<TagResponse> CreateAsync(TagRequest model);
    public Task<bool> DeleteAsync(int id);
    public Task<TagResponse> UpdateAsync(int id, TagRequest model);
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

    public async Task<Page<TagResponse>> GetAllByUserAsync(RequestFilter filter, int id)
    {
        var query = _context.Tags
            .Include(x => x.AccountCreated)
            .Where(x => x.AccountCreatedId == _currentUser.Id)
            .Select(tag => new TagResponse()
            {
                Id = tag.Id,
                Title = tag.Title,
                CreatedDate = tag.CreatedDate,
                WhoCreatedId = tag.AccountCreatedId,
                Color = tag.Color
            })
            .TagsFilter(filter);
        
        return new Page<TagResponse>(filter)
        {
            Objects = await query.Pagination(filter).ToListAsync(),
            ObjectsCount = await query.CountAsync()
        };
    }

    public async Task<TagResponse> GetByIdAsync(int id)
    {
        var dbTag = await _context.Tags
                        .Include(x => x.NoteTags)
                   .FirstOrDefaultAsync(x => x.Id == id
                                             && x.AccountCreatedId == _currentUser.Id) ??
               throw new KeyNotFoundException($"Тэг с ID[{id}] не найден");
        return _mapper.Map<TagResponse>(dbTag);
    }
    
    public async Task<Tag> GetByIdDbAsync(int id)
    {
        var dbTag = await _context.Tags
                        .Include(x => x.NoteTags)
                        .FirstOrDefaultAsync(x => x.Id == id
                                                  && x.AccountCreatedId == _currentUser.Id) ??
                    throw new KeyNotFoundException($"Тэг с ID[{id}] не найден");
        return dbTag;
    }

    public async Task<TagResponse> CreateAsync(TagRequest model)
    {
        var currentTitle = model.Title.Trim().ToLower();
        var isExsistTag = await _context.Tags.AnyAsync(x => x.AccountCreatedId == _currentUser.Id &&
                                                            x.Title.Trim().ToLower() == currentTitle);
        if (isExsistTag) throw new AppException($"Тэг с названием [{model.Title}] уже существует");
        
        var dbTag = _mapper.Map<Tag>(model);
        dbTag.CreatedDate = DateTime.Now;
        dbTag.AccountCreatedId = _currentUser.Id;
        
        await _context.Tags.AddAsync(dbTag);
        await _context.SaveChangesAsync();

        return _mapper.Map<TagResponse>(dbTag);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tagToDelete = await GetByIdDbAsync(id);
        if (tagToDelete.NoteTags.Count > 0)
            throw new AppException($"Тэг с [{id}] нельзя удалить, он используется в заметках");
        _context.Tags.Remove(tagToDelete);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TagResponse> UpdateAsync(int id, TagRequest model)
    {
        var dbTag = await GetByIdDbAsync(id);
        _mapper.Map(model, dbTag);
        
        _context.Tags.Update(dbTag);
        await _context.SaveChangesAsync();
        return _mapper.Map<TagResponse>(dbTag);
    }
}