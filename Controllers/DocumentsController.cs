using Lab1.Models;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentsService documentsService;

        public DocumentsController()
        {
            documentsService = new DocumentsService();
        }

        [HttpGet]
        public async Task<List<Document>> Get()
        {
            return await documentsService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Document> Get(int id)
        {
            return await documentsService.GetAsync(id);
        }

        [HttpGet("{author}")]
        public async Task<Document> Get(string author)
        {
            return await documentsService.GetAsync(author);
        }

        [HttpGet("{author}/{signed}")]
        public async Task<Document> Get(string author, bool signed)
        {
            return await documentsService.GetAsync(author, signed);
        }

        [HttpGet("{from}/{to}")]
        public async Task<Document> Get(DateTime from, DateTime to)
        {
            return await documentsService.GetAsync(from, to);
        }

        [HttpPost]
        public async Task<Document> Post([FromBody] Document newDocument)
        {
            await documentsService.CreateAsync(newDocument);

            return newDocument;
        }

        [HttpPut]
        public async Task<Document> Put(int id, [FromBody] Document updatedDocument)
        {
            var document = await documentsService.GetAsync(id);
            if (document == null)
            {
                return null;
            }

            updatedDocument.Id = document.Id;

            await documentsService.UpdateAsync(id, updatedDocument);

            return updatedDocument;
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            var employee = await documentsService.GetAsync(id);
            if (employee == null)
            {
                return;
            }

            await documentsService.RemoveAsync(id);
        }
    }
}
