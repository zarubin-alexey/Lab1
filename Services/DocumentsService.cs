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

        public List<Document> Get()
        {
            var sql = "Select * from documents;";

            return ExecuteGet(sql);
        }

        public Document Get(int id)
        {
            var sql = $"Select * from documents where \"id\" = '{id}';";

            return ExecuteGet(sql).FirstOrDefault();
        }

        public List<Document> Get(string author)
        {
            var sql = $"Select * from documents where \"author\" = '{author}';";

            return ExecuteGet(sql);
        }

        public List<Document> Get(string author, bool signed)
        {
            var sql = $"Select * from documents where \"author\" = '{author}' and signed IS {(signed ? "NOT " : "")}NULL;";

            return ExecuteGet(sql);
        }

        public List<Document> Get(DateTime from, DateTime to)
        {
            var sql = $"Select * from documents where created BETWEEN '{from}' AND '{to}'";

            return ExecuteGet(sql);
        }

        public void Create(Document newDocument)
        {
            var sql = $"insert into documents (\"id\", \"type\", \"content\", \"created\", \"signed\", \"author\") values ('{newDocument.Id}', '{newDocument.Type}', '{newDocument.Content}', '{newDocument.Created}', '{newDocument.Signed}', '{newDocument.Author}');";

            Execute(sql);
        }

        public void Update(int id, Document updatedDocument)
        {
            var sql = $"update documents set \"type\" = '{updatedDocument.Type}', \"content\" = '{updatedDocument.Content}', \"created\" = '{updatedDocument.Created}', \"signed\" = '{updatedDocument.Signed}', \"author\" = '{updatedDocument.Author}' where \"id\" = '{id}';";

            Execute(sql);
        }

        public void Remove(int id)
        {
            var sql = $"delete from documents where \"id\" = '{id}';";

            Execute(sql);
        }

        private void Execute(string sql)
        {
            connection.Open();

            var cmd = new NpgsqlCommand(sql, connection);

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        private List<Document> ExecuteGet(string sql)
        {
            connection.Open();

            var cmd = new NpgsqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
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
