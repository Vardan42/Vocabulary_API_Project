using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using Vocabulary_API_Project.DTO;
using Vocabulary_API_Project.Model;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllWordController : ControllerBase
    {
        private readonly IWordAllRepository wordAllRepository;
        private readonly IWordARepository wordARepository;
        private readonly IWordBRepository wordBRepository;
        private readonly ILogger<AllWordController> logger;
        private readonly IMapper mapper;
        protected ResponseDTO responseDTO;
        public AllWordController(
            IWordARepository wordARepository,
            IWordAllRepository wordAllRepository,
            ILogger<AllWordController> logger,
            IMapper mapper,
            IWordBRepository wordBRepository)
        {
            this.wordARepository = wordARepository;
            this.wordAllRepository = wordAllRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.responseDTO = new ResponseDTO();
            this.wordBRepository = wordBRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetAllAsnc()
        {
            try
            {
                var item = await wordAllRepository.GetAll();
                responseDTO.Result = mapper.Map<IEnumerable<AllWord>>(item);
                responseDTO.IsSuccess = true;
                responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
                logger.LogInformation("This is all words in A Word Table");
            }
            catch (Exception exeption)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                logger.LogInformation("Running process is inccorect");
                responseDTO.ErrorMessages.Add(exeption.Message);

            }
            return responseDTO;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> Post([FromBody] AllWord_DTO allWord_DTO)
        {
            try
            {
                var allWord=mapper.Map<AllWord>(allWord_DTO);
                
                if (allWord_DTO.Word.ToLower().StartsWith('a'))
                {
                    WordA_DTO Word_A = new  WordA_DTO()
                    {
                        Translate=allWord_DTO.Translate,
                        Word=allWord_DTO.Word,
                    };
                    var item=mapper.Map<WordA>(Word_A);
                    if (await wordBRepository.Get(u => u.Word.ToLower() == allWord_DTO.Word.ToLower()) != null)
                    {
                        logger.LogInformation("This word already have a database");
                        return BadRequest();
                    }
                    await wordARepository.Create(item);
                    responseDTO.IsSuccess = true;
                    responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
                    logger.LogInformation("You are creating data in database");
                    responseDTO.Result= item;
                }
                else if (allWord_DTO.Word.ToLower().StartsWith("b"))
                {
                    WordB_DTO wordB_DTO = new WordB_DTO
                    {
                        Translate = allWord_DTO.Translate,
                        Word = allWord_DTO.Word,
                    };
                    var item =mapper.Map<WordB>(wordB_DTO);
                    if (await wordBRepository.Get(u => u.Word.ToLower() == allWord_DTO.Word.ToLower()) != null)
                    {
                        logger.LogInformation("This word already have a database");
                        return BadRequest();
                    }
                    await wordBRepository.Create(item);
                    responseDTO.IsSuccess = true;
                    responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
                    logger.LogInformation("You are creating data in database");
                    responseDTO.Result = item;
                }
                if (await wordBRepository.Get(u => u.Word.ToLower() == allWord_DTO.Word.ToLower()) != null)
                {
                    logger.LogInformation("This word already have a database");
                    return BadRequest();
                }
                else
                await wordAllRepository.Create(allWord);
                responseDTO.Result = allWord;
                responseDTO.IsSuccess = true;
                responseDTO.StatusCode = HttpStatusCode.OK;
                logger.LogInformation("You are creating data in database");
            }
            catch (Exception exeption)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                logger.LogInformation("Running process is inccorect");
                responseDTO.ErrorMessages.Add(exeption.Message);
            }
            return responseDTO;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> Update(string name, [FromBody] AllWord_DTO allWord_DTO  )
        {
            try
            {
                var result = await wordAllRepository.Get(i => i.Word.ToLower() == name.ToLower());
                result.Word = allWord_DTO.Word;
                result.Translate = allWord_DTO.Translate;
                if (await wordAllRepository.Get(u => u.Word.ToLower() == allWord_DTO.Word.ToLower()) != null)
                {
                    logger.LogInformation("This word already have a database");
                    return BadRequest();
                }
                responseDTO.Result = JsonConvert.DeserializeObject(name + " You are upted this data");
                responseDTO.IsSuccess = true;
                responseDTO.StatusCode = HttpStatusCode.OK;
                logger.LogInformation("You are Updated data in database");
                await wordAllRepository.Save();
           
            }
            catch (Exception exeption)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                logger.LogInformation("Running process is inccorect");
                responseDTO.ErrorMessages.Add(exeption.Message);
            }

            return Ok(responseDTO);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> DeleteAsync(string name)
        {
            try
            {
                await wordAllRepository.Delete(name);
                responseDTO.IsSuccess = true;
                responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception exeption)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                logger.LogInformation("Running process is inccorect");
                responseDTO.ErrorMessages.Add(exeption.Message); ;
            }
            return responseDTO;
        }
        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetFindByNameAsync(string name)
        {
            try
            {
                var result = await wordAllRepository.Get(n => n.Word.ToLower() == name.ToLower());
                responseDTO.Result = mapper.Map<AllWord>(result);
                responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
                logger.LogInformation("This is all words in A Word Table");
            }
            catch (Exception exeption)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                logger.LogInformation("Running process is inccorect");
                responseDTO.ErrorMessages.Add(exeption.Message);
            }
            return responseDTO;
        }
    }
}
