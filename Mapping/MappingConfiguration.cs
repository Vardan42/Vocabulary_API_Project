using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;
using Vocabulary_API_Project.DTO;
using Vocabulary_API_Project.Model;
namespace Vocabulary_API_Project.Mapping
{
	public class MappingConfiguration:Profile
	{
		public MappingConfiguration()
		{
			CreateMap<WordA, WordA_DTO>().ReverseMap();
			CreateMap<AllWord, AllWord_DTO>().ReverseMap();
			CreateMap<WordB,WordB_DTO>().ReverseMap();
		}
	}
}
