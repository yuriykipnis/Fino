using System;

namespace GoldMountainShared.Dto.Shared
{
    public class ContactMessageDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        public String Username { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String Phone { get; set; } = String.Empty;
        public String Subject { get; set; } = String.Empty;
        public String Message { get; set; } = String.Empty;
    }
}
