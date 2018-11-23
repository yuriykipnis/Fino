using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainApi.Controllers.Helper
{
    public interface IEmailHelper
    {
        Task SendMessage(ContactMessage message);
    }
}
