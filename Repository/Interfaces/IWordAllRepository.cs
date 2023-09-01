using System.Linq.Expressions;
using Vocabulary_API_Project.Model;

namespace Vocabulary_API_Project.Repository.Interfaces
{
	public interface IWordAllRepository
	{
		Task<AllWord> Get(Expression<Func<AllWord, bool>>? predicate = null, bool trucked = true);
		Task<List<AllWord>> GetAll(Expression<Func<AllWord, bool>> filter = null);
		Task Create(AllWord wordA);
		Task Update(AllWord wordA);
		Task<AllWord> Delete(string name);
		Task Save();
	}
}
