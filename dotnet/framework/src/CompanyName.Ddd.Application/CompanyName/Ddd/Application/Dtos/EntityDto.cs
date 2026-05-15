using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Ddd.Application.Dtos
{

    [Serializable]
    public record class EntityDto<TKey> : IEntityDto<TKey>
    {
        public TKey Id { get; set; }
    }

    [Serializable]
    public record class EntityDto : IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
