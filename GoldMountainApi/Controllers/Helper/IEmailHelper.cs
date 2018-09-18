using GoldMountainShared.Storage.Documents;

namespace GoldMountainApi.Controllers.Helper
{
    public interface IEmailHelper
    {
        void SendMessage(ContactMessage message);
    }
}
