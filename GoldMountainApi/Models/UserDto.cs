using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldMountainApi.Models
{
    public class UserDto
    {
        public String Id { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;

        public IEnumerable<Guid> Accounts { get; set; }
    }
}
