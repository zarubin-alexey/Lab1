using Lab1.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class DocumentsService
    {
        private readonly NpgsqlConnection connection;

        public DocumentsService()
        {
            connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=1111;Database=Documents");
        }

        public async Task<List<Document>> GetAsync()
        {
            var sql = "Select * from documents;";

            return await ExecuteGet(sql);
        }

        public async Task<Document> GetAsync(string author)
        {
            var sql = $"Select * from documents where \"author\" = '{author}';";

            return (await ExecuteGet(sql)).FirstOrDefault();
        }

        public async Task<Document> GetAsync(string author, bool signed)
        {
            var sql = $"Select * from documents where \"author\" = '{author}' and signed IS {(signed ? "NOT " : "")}NULL;";

            return (await ExecuteGet(sql)).FirstOrDefault();
        }

        public async Task<Document> GetAsync(DateTime from, DateTime to)
        {
            var sql = $"Select * from documents where created BETWEEN '{from}' AND '{to}'";

            return (await ExecuteGet(sql)).FirstOrDefault();
        }

        public async Task<Document> GetAsync(int id)
        {
            var sql = $"Select * from documents where \"id\" = '{id}';";

            return (await ExecuteGet(sql)).FirstOrDefault();
        }

        public async Task CreateAsync(Document newDocument)
        {
            var sql = $"insert into documents (\"id\", \"type\", \"content\", \"created\", \"signed\", \"author\") values ('{newDocument.Id}', '{newDocument.Type}', '{newDocument.Content}', '{newDocument.Created}', '{newDocument.Signed}', '{newDocument.Author}');";

            await Execute(sql);
        }

        public async Task UpdateAsync(int id, Document updatedDocument)
        {
            var sql = $"update documents set \"type\" = '{updatedDocument.Type}', \"content\" = '{updatedDocument.Content}', \"created\" = '{updatedDocument.Created}', \"signed\" = '{updatedDocument.Signed}', \"author\" = '{updatedDocument.Author}' where \"id\" = '{id}';";

            await Execute(sql);
        }

        public async Task RemoveAsync(int id)
        {
            var sql = $"delete from documents where \"id\" = '{id}';";

            await Execute(sql);
        }

        private async Task Execute(string sql)
        {
            connection.Open();

            var cmd = new NpgsqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();

            connection.Close();
        }

        private async Task<List<Document>> ExecuteGet(string sql)
        {
            connection.Open();

            var cmd = new NpgsqlCommand(sql, connection);

            var reader = await cmd.ExecuteReaderAsync();
            var documents = GetDocuments(reader);

            connection.Close();

            return documents;
        }

        private static List<Document> GetDocuments(NpgsqlDataReader reader)
        {
            var dt = new DataTable();
            dt.Load(reader);

            var documents = new List<Document>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var doc = new Document
                {
                    Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                    Type = (DocumentType)Enum.Parse(typeof(DocumentType), dt.Rows[i]["Type"].ToString()),
                    Content = dt.Rows[i]["Content"].ToString(),
                    Created = (DateTime)dt.Rows[i]["Created"],
                    Author = dt.Rows[i]["Author"].ToString(),
                };

                if (dt.Rows[i]["Signed"] is DateTime date)
                {
                    doc.Signed = date;
                }

                documents.Add(doc);
            }

            return documents;
        }
    }
}
