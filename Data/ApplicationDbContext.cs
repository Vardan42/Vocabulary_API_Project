using Microsoft.EntityFrameworkCore;
using Vocabulary_API_Project.Model;
namespace Vocabulary_API_Project.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			:base(options)
		{


		}
		public DbSet<WordA> WordA { get; set; }
		public DbSet<AllWord> AllWords { get; set; }
		public DbSet<WordB>	WordB { get; set; }
    }
}
