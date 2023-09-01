using System.Net;

namespace Vocabulary_API_Project.Model
{
	public class ResponseDTO
	{
		public HttpStatusCode StatusCode { get; set; }
		public bool IsSuccess { get; set; }
		public List<string> ErrorMessages { get; set; }
		public object Result { get; set; }
	}
}
