﻿namespace Vesta.Core
{ 
    public interface ISoftDelete
    {
         bool IsDeleted { get; set;  }
    }
}