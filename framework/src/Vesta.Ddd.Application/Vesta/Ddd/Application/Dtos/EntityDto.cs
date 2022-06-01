﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Ddd.Application.Dtos
{

    public class EntityDto<TKey> : IEntityDto<TKey>
    {
        public TKey Id { get; set; }
    }

    public class EntityDto : IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
