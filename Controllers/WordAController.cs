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
using Vocabulary_API_Project.Repository.Classes;
using Vocabulary_API_Project.Repository.Interfaces;

namespace Vocabulary_API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordAController : ControllerBase
    {
        private readonly IWordARepository  wordARepository;
        private readonly ILogger<WordAController> logger;
        private IMapper mapper;
        protected ResponseDTO responseDTO;
        public WordAController(ILogger<WordAController> logger,IMapper mapper, IWordARepository wordARepository)
        {
            this.logger = logger;
            this.wordARepository = wordARepository;
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
                IEnumerable<WordA> result = await wordARepository.GetAll();
                responseDTO.Result = mapper.Map<IEnumerable<WordA_DTO>>(result);
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
                var result=await wordARepository.Get(n=>n.Word.ToLower()==name.ToLower());
                responseDTO.Result = mapper.Map<WordA>(result);
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
        public async Task<ActionResult<ResponseDTO>> PostAsync([FromBody]WordA_DTO wordA)
        {
            try
            {
                var result = mapper.Map<WordA>(wordA);
                if (await wordARepository.Get(u => u.Word.ToLower() == wordA.Word.ToLower()) != null)
                {
                    logger.LogInformation("This word already have a database");
                    return BadRequest();
                }
                await wordARepository.Create(result);
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
                await wordARepository.Delete(name);
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
        public async Task<ActionResult> Update(string name, [FromBody] WordA_DTO wordA)
        {
            var result = await wordARepository.Get(i=>i.Word.ToLower()==name.ToLower());
            result.Word=wordA.Word;
            result.Translate=wordA.Translate;
            if (await wordARepository.Get(u=>u.Word.ToLower()==wordA.Word.ToLower())!=null)
            {
                logger.LogInformation("This word already have a database");
                return BadRequest();
            }
            await wordARepository.Save();
            return Ok();
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateParialWord(string name, JsonPatchDocument<WordA> jsonPatchDocument)
        {
            if (jsonPatchDocument == null || name == null)
            {
                return BadRequest();
            }
            var word = await wordARepository.Get(u => u.Word.ToLower() == name.ToLower());
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
