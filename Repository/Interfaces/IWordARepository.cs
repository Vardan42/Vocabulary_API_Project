using System.Linq.Expressions;
using Vocabulary_API_Project.Model;

namespace Vocabulary_API_Project.Repository.Interfaces
{
	public interface IWordARepository
	{
		//Task<IEnumerable<WordA>> GetAllAsync();
		//Task<WordA> GetFindasync(string name);
		Task<WordA> Get(Expression<Func<WordA, bool>>? predicate=null,bool trucked=true);
		Task<List<WordA>> GetAll(Expression<Func<WordA, bool>> filter = null);
		Task Create(WordA wordA);
		Task<WordA> Delete(string name);
		Task Save();
	}
}
