using AutoMapper;

namespace Books.CommandHandlers.Commands.Books
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBook, Book>();
        }
    }
}
