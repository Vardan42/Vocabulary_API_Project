using System.ComponentModel.DataAnnotations;

namespace Vocabulary_API_Project.Model
{
	public class AllWord
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Word { get; set; }

		[Required]
		public string Translate { get; set; }
	}
}
