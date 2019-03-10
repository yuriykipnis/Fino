using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainShared.Storage.Interfaces
{
    public interface ICreditCardRepository
    {
        Task<IEnumerable<CreditCardDoc>> GetAllCards();

        Task<CreditCardDoc> GetCard(String id);

        Task<IEnumerable<CreditCardDoc>> GetCardsByUserId(String userId);

        Task<IEnumerable<CreditCardDoc>> GetCardsByProviderId(String providerId);

        Task AddCard(CreditCardDoc item);

        Task AddCards(IEnumerable<CreditCardDoc> items);

        Task<bool> RemoveCard(String id);

        Task<bool> UpdateCard(String id, CreditCardDoc account);

        Task<bool> RemoveAllCards();

        Task<CreditCardDoc> FindCardByCriteria(Expression<Func<CreditCardDoc, bool>> filter);
    }
}
