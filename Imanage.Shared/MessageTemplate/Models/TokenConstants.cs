using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.MessageTemplate
{
    public class TokenConstant
    {
        public TokenConstant(string Name)
        {
            Key = Name;
        }
        public string Key { get; set; }

        public string Tag => $"%{Key}%";

    }

    

}
