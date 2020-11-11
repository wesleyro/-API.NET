using System;
using Api.Models;
using Api.Data.Collections;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class infectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;

        IMongoCollection<infectado> _infectadosCollection;

        public infectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection =_mongoDB.DB.GetCollection<infectado>(typeof(infectado).Name.ToLower());
        }
        [HttpPost]

        public ActionResult SalvarInfectado([FromBody] infectadoDto dto)
        {
            var infectado = new infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionando com  sucesso");        
        }
        [HttpGet]

        public ActionResult ObterInferctados()
        {
            var infectados = _infectadosCollection.Find(Builders<infectado>.Filter.Empty).ToList();

            return Ok(infectados);

        }
        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody]infectadoDto dto) 
        {
            _infectadosCollection.UpdateOne(Builders<infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento),Builders<infectado>.Update.Set("sexo",dto.Sexo));

            return Ok("Atualizando com sucesso");
        }      

        [HttpDelete("{dataNasc}")]
        public ActionResult Delete(DateTime dataNasc) 
        {
            _infectadosCollection.DeleteOne(Builders<infectado>.Filter.Where(_ => _.DataNascimento == dataNasc));

            return Ok("Atualizando com sucesso");
        }  

        
    }
}