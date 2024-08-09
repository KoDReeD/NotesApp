using AutoMapper;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Accounts.Response;
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

        CreateMap<Account, AuthorizeResponse>()
            .ForMember(auth => auth.Id, account => account.MapFrom(srs => srs.Id));
        CreateMap<AccessTokenModel, AuthorizeResponse>();
        CreateMap<RefreshToken, AuthorizeResponse>()
            .ForMember(auth => auth.RefreshToken, refresh => refresh.MapFrom(srs => srs.Token))
            .ForMember(auth => auth.RefreshTokenExpirationDate, refresh => refresh.MapFrom(srs => srs.ExpiresDate))
            .ForMember(auth => auth.CreateRefreshTokenDate, refresh => refresh.MapFrom(srs => srs.CreatedDate))
            .ForMember(auth => auth.Id, opt => opt.Ignore());
        
        
        CreateMap<TagRequest, Tag>();
    }
}