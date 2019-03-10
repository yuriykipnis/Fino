using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IInstitutionRepository
    {
        Task<IEnumerable<InstitutionDoc>> GetInstitutions();

        Task AddInstitution(InstitutionDoc item);

        Task<bool> RemoveAllInstitutions();
    }
}
