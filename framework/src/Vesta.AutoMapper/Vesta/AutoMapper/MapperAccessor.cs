using AutoMapper;

namespace Vesta.AutoMapper
{
    public class MapperAccessor : IMapperAccessor
    {
        public IMapper Mapper { get; set; }

    }
}
