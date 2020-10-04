using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebApiAmigo.Models;

namespace WebApiAmigo.Data
{
    public interface IAmigoRepository 
    {
        IEnumerable<Amigo> BuscarAmigos();
        Amigo ObterAmigo(string id);
        bool AdicionarAmigo(Amigo amigo);
        bool EditarAmigo(Amigo amigo);
        bool ExcluirAmigo(string id);
        AmigosRelacionados ObterAmigosRelacionados(string id);
        bool AdicionarAmigosRelacionados(AmigosRelacionados amigosRelacionados);
    }
    public class AmigoRepository : IAmigoRepository
    {
        public IConfiguration Configuration { get; }

        public AmigoRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IEnumerable<Amigo> BuscarAmigos()
        {
            var amigos = new List<Amigo>();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]))
            {
                var procedureName = "BuscarAmigos";
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
                        var amigo = new Amigo
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            Sobrenome = reader["sobrenome"].ToString(),
                            Email = reader["email"].ToString(),
                            Telefone = reader["telefone"].ToString(),
                            DataNascimento = DateTime.Parse(reader["datanascimento"].ToString()),
                            Foto = reader["foto"].ToString(),
                            PaisId = reader["paisid"].ToString(),
                            EstadoId = reader["estadoid"].ToString()
                        };

                        amigos.Add(amigo);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return amigos;
        }

        public Amigo ObterAmigo(string id)
        {
            var amigo = new Amigo();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]))
            {
                var procedureName = "ObterAmigo";
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
                        amigo = new Amigo
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            Sobrenome = reader["sobrenome"].ToString(),
                            Email = reader["email"].ToString(),
                            Telefone = reader["telefone"].ToString(),
                            DataNascimento = DateTime.Parse(reader["datanascimento"].ToString()),
                            Foto = reader["foto"].ToString(),
                            PaisId = reader["paisid"].ToString(),
                            EstadoId = reader["estadoid"].ToString()
                        };
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return amigo;
        }

        public bool AdicionarAmigo(Amigo amigo)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]);
            var procedureName = "AdicionarAmigo";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", amigo.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", amigo.Nome);
            sqlCommand.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
            sqlCommand.Parameters.AddWithValue("@Email", amigo.Email);
            sqlCommand.Parameters.AddWithValue("@Telefone", amigo.Telefone);
            sqlCommand.Parameters.AddWithValue("@DataNascimento", amigo.DataNascimento);
            sqlCommand.Parameters.AddWithValue("@Foto", amigo.Foto);
            sqlCommand.Parameters.AddWithValue("@PaisId", amigo.PaisId);
            sqlCommand.Parameters.AddWithValue("@EstadoId", amigo.EstadoId);

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

        public bool EditarAmigo(Amigo amigo)
        {

            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]);
            var procedureName = "EditarAmigo";
            var sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            sqlCommand.Parameters.AddWithValue("@Id", amigo.Id);
            sqlCommand.Parameters.AddWithValue("@Nome", amigo.Nome);
            sqlCommand.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
            sqlCommand.Parameters.AddWithValue("@Email", amigo.Email);
            sqlCommand.Parameters.AddWithValue("@Telefone", amigo.Telefone);
            sqlCommand.Parameters.AddWithValue("@DataNascimento", amigo.DataNascimento);
            sqlCommand.Parameters.AddWithValue("@Foto", amigo.Foto);
            sqlCommand.Parameters.AddWithValue("@PaisId", amigo.PaisId);
            sqlCommand.Parameters.AddWithValue("@EstadoId", amigo.EstadoId);

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

        public bool ExcluirAmigo(string id)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]);
            var procedureName = "ExcluirAmigo";
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

        public AmigosRelacionados ObterAmigosRelacionados(string id)
        {
            var amigosRelacionados = new AmigosRelacionados();
            var amigos = new List<Amigo>();

            using (var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]))
            {
                var procedureName = "ObterAmigosRelacionados";
                var sqlCommand = new SqlCommand(procedureName, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();

                    using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        var amigo = new Amigo
                        {
                            Id = reader["id"].ToString(),
                            Nome = reader["nome"].ToString(),
                            Sobrenome = reader["sobrenome"].ToString(),
                            Email = reader["email"].ToString(),
                            Telefone = reader["telefone"].ToString(),
                            DataNascimento = DateTime.Parse(reader["datanascimento"].ToString()),
                            Foto = reader["foto"].ToString(),
                            PaisId = reader["paisid"].ToString(),
                            EstadoId = reader["estadoid"].ToString()
                        };

                        amigos.Add(amigo);
                    }

                    amigosRelacionados = new AmigosRelacionados
                    {
                        Amigo = amigos.Where(x => x.Id == id).FirstOrDefault(),
                        TodosAmigos = amigos.Where(x => x.Id != id).ToList()
                    };
                    amigosRelacionados.AmigosRelacionadosIds = amigosRelacionados.Amigo.AmigosRelacionados.Select(x => x.Id).ToList();

                }
                finally
                {
                    connection.Close();
                }
            }
            return amigosRelacionados;
        }

        public bool AdicionarAmigosRelacionados(AmigosRelacionados amigosRelacionados)
        {
            using var connection = new SqlConnection(Configuration["ConnectionStrings:WebApiAmigo"]);
            var procedureName = "AdicionarAmigosRelacionados";


            foreach(var ar in amigosRelacionados.AmigosRelacionadosIds)
            {
                var sqlCommand = new SqlCommand(procedureName, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Id", amigosRelacionados.Amigo.Id);
                sqlCommand.Parameters.AddWithValue("@AmigoId", ar);

                try
                {
                    connection.Open();

                    using var reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    reader.Read();
                }
                finally
                {
                    connection.Close();
                }
            }
            return true;
            
        }



    }
}
