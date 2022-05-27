using AutoMapper;

namespace Vesta.AutoMapper
{
    internal class MapperAccessor : IMapperAccessor
    {

        public IMapper Mapper { get; set; }

        public MapperAccessor(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}
