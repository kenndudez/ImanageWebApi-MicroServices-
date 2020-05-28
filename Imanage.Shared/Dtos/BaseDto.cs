using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dtos
{
    public class BaseDto
    {
        public Guid Id { get; set;  }
        public int TotalCount { get; set; } 
    }
}
