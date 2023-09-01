using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Vocabulary_API_Project.Data;
using Vocabulary_API_Project.Model;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Repository.Classes
{
    public class WordARepository : IWordARepository
	{
		private readonly ApplicationDbContext _context;
		public WordARepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task Create(WordA wordA)
		{
			wordA.Word = wordA.Word.ToLower();
			wordA.Translate = wordA.Translate.ToLower();
			await _context.WordA.AddAsync(wordA);
			await Save();	
		}
		public async Task<WordA> Delete(string name)
		{
			var item=await Get(s=>s.Word.ToLower()==name.ToLower());
			_context.WordA.Remove(item);
			await Save();
			return item;
		}
        public async Task<WordA> Get(Expression<Func<WordA, bool>>? predicate = null, bool trucked = true)
		{
			IQueryable<WordA> query = _context.WordA;
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
		public async Task<List<WordA>> GetAll(Expression<Func<WordA, bool>> filter = null)
		{
			IQueryable<WordA> query = _context.WordA;
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
	}
}
