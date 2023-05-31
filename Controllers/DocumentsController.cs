using Lab1.Models;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentsService documentsService;

        public DocumentsController()
        {
            documentsService = new DocumentsService();
        }

        [HttpGet]
        public List<Document> Get()
        {
            return documentsService.Get();
        }

        [HttpGet("{id:int}")]
        public Document GetId(int id)
        {
            return documentsService.Get(id);
        }

        [HttpGet("author/{author}")]
        public List<Document> GetAuthor(string author)
        {
            return documentsService.Get(author);
        }

        [HttpGet("author/{author}/signed/{signed}")]
        public List<Document> Get(string author, bool signed)
        {
            return documentsService.Get(author, signed);
        }

        [HttpGet("from/{from}/to/{to}")]
        public List<Document> Get(DateTime from, DateTime to)
        {
            return documentsService.Get(from, to);
        }

        [HttpPost]
        public Document Post([FromBody] Document newDocument)
        {
            documentsService.Create(newDocument);

            return newDocument;
        }

        [HttpPut]
        public Document Put([FromBody] Document updatedDocument)
        {
            var document = documentsService.Get(updatedDocument.Id);
            if (document == null)
            {
                return null;
            }

            updatedDocument.Id = document.Id;

            documentsService.Update(updatedDocument.Id, updatedDocument);

            return updatedDocument;
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var employee = documentsService.Get(id);
            if (employee == null)
            {
                return;
            }

            documentsService.Remove(id);
        }
    }
}
