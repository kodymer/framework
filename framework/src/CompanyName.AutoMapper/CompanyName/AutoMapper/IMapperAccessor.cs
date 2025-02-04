using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.AutoMapper
{
    public interface IMapperAccessor
    {
        IMapper Mapper { get; }
    }
}
