using System.ComponentModel.DataAnnotations;

namespace Vocabulary_API_Project.Model
{
	public class WordA
	{
		[Key]
		public int Id { get; set; } = 1;

		[Required]
		public string Word { get; set; }

		[Required]
		public string Translate { get; set; }
	}
}
