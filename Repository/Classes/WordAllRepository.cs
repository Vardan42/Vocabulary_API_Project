using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Vocabulary_API_Project.Data;
using Vocabulary_API_Project.Model;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Repository.Classes
{
	public class WordAllRepository : IWordAllRepository
	{
		private readonly ApplicationDbContext _context;
		public WordAllRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task Create(AllWord wordA)
		{
			wordA.Word = wordA.Word.ToLower();
			wordA.Translate = wordA.Translate.ToLower();
			await _context.AllWords.AddAsync(wordA);
			await Save();
		}
		public async Task<AllWord> Delete(string name)
		{
			name.ToLower();
			var word = await _context.AllWords.FirstOrDefaultAsync(n => n.Word == name);
			_context.AllWords.Remove(word);
			await Save();
			return word;
		}
		public async Task<AllWord> Get(Expression<Func<AllWord, bool>>? predicate = null, bool trucked = true)
		{
			IQueryable<AllWord> query = _context.AllWords;
			if (!trucked)
			{
				query = query.AsNoTracking();
			}
			if (predicate != null)
			{
				query = query.Where(predicate);
			}
			return await query.FirstOrDefaultAsync();
		}
		public async Task<List<AllWord>> GetAll(Expression<Func<AllWord, bool>> filter = null)
		{
			IQueryable<AllWord> query = _context.AllWords;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			return await query.ToListAsync();
		}
		public async Task Save()
		{
			await _context.SaveChangesAsync();
		}
		public async Task Update(AllWord wordA)
		{
			_context.AllWords.Update(wordA);
			await Save();
		}
	}
}
