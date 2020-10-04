using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Data
{
    public interface IPaisRepository 
    {
        IEnumerable<Pais> BuscarPaises();
        Pais ObterPais(string id);
        bool AdicionarPais(Pais pais);
        bool EditarPais(Pais pais);
        bool ExcluirPais(string id);
    }
    public class PaisRepository : IPaisRepository
    {
        public IConfiguration Configuration { get; }

        public PaisRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IEnumerable<Pais> BuscarPaises()
        {
            var paises = new List<Pais>();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]))
            {
                var procedureName = "BuscarPaises";
                var sqlCommand = new SqlCommand(procedureName, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();

                    using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        var pais = new Pais
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            FotoBandeira = reader["fotobandeira"].ToString()
                        };

                        paises.Add(pais);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return paises;
        }

        public Pais ObterPais(string id)
        {
            var pais = new Pais();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]))
            {
                var procedureName = "ObterPais";
                var sqlCommand = new SqlCommand(procedureName, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();

                    using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        pais = new Pais
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            FotoBandeira = reader["fotobandeira"].ToString()
                        };
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return pais;
        }

        public bool AdicionarPais(Pais pais)
        {

            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "AdicionarPais";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", pais.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", pais.Nome);
            sqlCommand.Parameters.AddWithValue("@FotoBandeira", pais.FotoBandeira);

            try
            {
                connection.Open();

                using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                return reader.Read();
            }
            finally
            {
                connection.Close();
            }
        }

        public bool EditarPais(Pais pais)
        {

            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "EditarPais";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", pais.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", pais.Nome);
            sqlCommand.Parameters.AddWithValue("@FotoBandeira", pais.FotoBandeira);

            try
            {
                connection.Open();

                using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                return reader.Read();
            }
            finally
            {
                connection.Close();
            }
        }

        public bool ExcluirPais(string id)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "ExcluirPais";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", id);

            try
            {
                connection.Open();

                using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                return reader.Read();
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
