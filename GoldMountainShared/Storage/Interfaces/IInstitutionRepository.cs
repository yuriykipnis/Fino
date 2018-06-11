using System.Collections.Generic;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface IInstitutionRepository
    {
        Task<IEnumerable<Institution>> GetInstitutions();

        Task AddInstitution(Institution item);

        Task<bool> RemoveAllInstitutions();
    }
}
