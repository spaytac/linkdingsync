using AutoMapper;
using Core.Entities.Linkding;

namespace Linkding.Client.Automapper;

public class LinkdingBookmarkProfile : Profile
{
    public LinkdingBookmarkProfile()
    {
        CreateMap<Bookmark, BookmarkUpdatePayload>();
        CreateMap<Bookmark, BookmarkCreatePayload>();
    }
}