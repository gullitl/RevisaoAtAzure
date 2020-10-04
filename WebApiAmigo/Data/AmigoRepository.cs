using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Data
{
    public interface IEstadoRepository 
    {
        IEnumerable<Estado> BuscarEstados();
        Estado ObterEstado(string id);
        bool AdicionarEstado(Estado estado);
        bool EditarEstado(Estado estado);
        bool ExcluirEstado(string id);
    }
    public class EstadoRepository : IEstadoRepository
    {
        public IConfiguration Configuration { get; }

        public EstadoRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IEnumerable<Estado> BuscarEstados()
        {
            var estados = new List<Estado>();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]))
            {
                var procedureName = "BuscarEstados";
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
                        var estado = new Estado
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            FotoBandeira = reader["fotobandeira"].ToString(),
                            PaisId = reader["paisid"].ToString()
                        };

                        estados.Add(estado);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return estados;
        }

        public Estado ObterEstado(string id)
        {
            var estado = new Estado();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]))
            {
                var procedureName = "ObterEstado";
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
                        estado = new Estado
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            FotoBandeira = reader["fotobandeira"].ToString(),
                            PaisId = reader["paisid"].ToString()
                        };
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return estado;
        }

        public bool AdicionarEstado(Estado estado)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "AdicionarEstado";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", estado.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", estado.Nome);
            sqlCommand.Parameters.AddWithValue("@FotoBandeira", estado.FotoBandeira);
            sqlCommand.Parameters.AddWithValue("@PaisId", estado.PaisId);

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

        public bool EditarEstado(Estado estado)
        {

            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "EditarEstado";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", estado.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", estado.Nome);
            sqlCommand.Parameters.AddWithValue("@FotoBandeira", estado.FotoBandeira);
            sqlCommand.Parameters.AddWithValue("@PaisId", estado.PaisId);

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

        public bool ExcluirEstado(string id)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiPaisEstado"]);
            var procedureName = "ExcluirEstado";
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
