using AutoMapper;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Jwt.Response;
using NotesApi.Models.Notes.Request;
using NotesApi.Models.Notes.Response;
using NotesApi.Models.Tag;

namespace NotesApi;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterRequest, Account>();

        CreateMap<NoteRequest, Note>();
        
        CreateMap<Account, JwtResponse>();
        
        CreateMap<TagRequest, Tag>();
    }
}