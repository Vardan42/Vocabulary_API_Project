using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using Vocabulary_API_Project.Data;
using Vocabulary_API_Project.DTO;
using Vocabulary_API_Project.Model;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordBController : ControllerBase
    {
        private readonly IWordBRepository   wordBRepository;
        private readonly ILogger<WordBController> logger;
        private IMapper mapper;
        protected ResponseDTO responseDTO;
        public WordBController(ILogger<WordBController> logger,IMapper mapper, IWordBRepository wordBRepository)
        {
            this.logger = logger;
            this.wordBRepository = wordBRepository;
            this.mapper=mapper;
            responseDTO = new ResponseDTO();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetAllAsync()
        {
            try
            {
                IEnumerable<WordB> result = await wordBRepository.GetAll();
                responseDTO.Result = mapper.Map<IEnumerable<WordB_DTO>>(result);
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

        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetFindByNameAsync(string name)
        {
            try
            {
                var result=await wordBRepository.Get(n=>n.Word.ToLower()==name.ToLower());
                responseDTO.Result = mapper.Map<WordB>(result);
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
        public async Task<ActionResult<ResponseDTO>> PostAsync([FromBody]WordB_DTO wordB_DTO )
        {
            try
            {
                var result = mapper.Map<WordB>(wordB_DTO);
                if (await wordBRepository.Get(u => u.Word.ToLower() == wordB_DTO.Word.ToLower()) != null)
                {
                    logger.LogInformation("This word already have a database");
                    return BadRequest();
                }
                await wordBRepository.Create(result);
                responseDTO.Result = result;
                responseDTO.IsSuccess = true;
                responseDTO.StatusCode = System.Net.HttpStatusCode.OK;
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
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> DeleteAsync(string name)
        {
            try
            {
                await wordBRepository.Delete(name);
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
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(string name, [FromBody] WordB_DTO wordB_DTO)
        {
            var result = await wordBRepository.Get(i=>i.Word.ToLower()==name.ToLower());
            result.Word= wordB_DTO.Word;
            result.Translate= wordB_DTO.Translate;
            if (await wordBRepository.Get(u=>u.Word.ToLower()== wordB_DTO.Word.ToLower())!=null)
            {
                logger.LogInformation("This word already have a database");
                return BadRequest();
            }
            await wordBRepository.Save();
            return Ok();
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateParialWord(string name, JsonPatchDocument<WordB> jsonPatchDocument)
        {
            if (jsonPatchDocument == null || name == null)
            {
                return BadRequest();
            }
            var word = await wordBRepository.Get(u => u.Word.ToLower() == name.ToLower());
            if (word == null)
            {
                return BadRequest();
            }
            jsonPatchDocument.ApplyTo(word, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
