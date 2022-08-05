using AutoMapper;
using IdentityServer.DbContext;
using MeetupAPI.Model;

namespace MvcMeetupClient.ViewModels.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FullEventViewModel, Event>()
            .ForMember(d => d.Theme,
                o => o.MapFrom(s => new Theme()
                {
                    Value = s.Theme
                }));

        CreateMap<Event, FullEventViewModel>()
            .ForMember(d => d.Theme,
                o => o.MapFrom(s => s.Theme.Value));
    }
}