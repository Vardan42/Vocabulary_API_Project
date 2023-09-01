using System.Linq.Expressions;
using Vocabulary_API_Project.Model;

namespace Vocabulary_API_Project.Repository.Interfaces
{
    public interface IWordBRepository
    {
        //Task<WordB> GetFindasync(string name);
        Task<WordB> Get(Expression<Func<WordB, bool>>? predicate = null, bool trucked = true);
        Task<List<WordB>> GetAll(Expression<Func<WordB, bool>> filter = null);
        Task Create(WordB wordA);
        Task Update(WordB wordA);
        Task<WordB> Delete(string name);
        Task Save();
    }
}
