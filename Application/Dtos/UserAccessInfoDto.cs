using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserAccessInfoDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
    }
}
