using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vocabulary_API_Project.Data;
using Vocabulary_API_Project.Model;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Repository.Classes
{
    public class WordBRepository : IWordBRepository
    {
        private readonly ApplicationDbContext _context;
        public WordBRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(WordB wordA)
        {
            wordA.Word = wordA.Word.ToLower();
            wordA.Translate = wordA.Translate.ToLower();
            await _context.WordB.AddAsync(wordA);
            await Save();
        }
        public async Task<WordB> Delete(string name)
        {
            name.ToLower();
            var word = await _context.WordB.FirstOrDefaultAsync(n => n.Word == name);
            _context.WordB.Remove(word);
            await Save();
            return word;
        }
        public async Task<WordB> Get(Expression<Func<WordB, bool>>? predicate = null, bool trucked = true)
        {
            IQueryable<WordB> query = _context.WordB;
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
        public async Task<List<WordB>> GetAll(Expression<Func<WordB, bool>> filter = null)
        {
            IQueryable<WordB> query = _context.WordB;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.AsNoTracking().ToListAsync();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task Update(WordB wordA)
        {
            _context.WordB.Update(wordA);
            await Save();
        }
    }
}
