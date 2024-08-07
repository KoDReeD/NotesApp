using AutoMapper;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;

namespace NotesApi;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterRequest, Account>();
    }
}