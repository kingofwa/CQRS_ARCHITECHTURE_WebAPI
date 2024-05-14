using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserInfoDto
    {
        public string Name { get; set; }
        public string UserRole { get; set; }
        public List<string> Permissions { get; set; }
    }
}
