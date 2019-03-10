using System;
using System.Collections.Generic;

namespace GoldMountainShared.Storage.Documents
{
    public class UserDoc
    {
        public String Id { get; set; } = Guid.NewGuid().ToString();
        public String Name { get; set; }
        public String Email { get; set; }
        public IEnumerable<String> Accounts { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
