using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Ddd.Application.Dtos
{
    public interface IEntityDto<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IEntityDto : IEntityDto<int>
    {

    }
}
