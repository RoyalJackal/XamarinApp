using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string UserName { get; set; }
    }
}
