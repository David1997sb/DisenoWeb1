using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {

            Usuario usuario = new Usuario();

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(
                        System.Configuration.ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("SELECT USU_CODIGO, USU_IDENTIFICACION, " +
                        "USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, USU_FECHA " +
                        "FROM USUARIO WHERE USU_CODIGO = @USU_CODIGO", connection);

                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", id);

                    connection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        usuario.USU_CODIGO = sqlDataReader.GetInt32(0);
                        usuario.USU_IDENTIFICACION = sqlDataReader.GetString(1);
                        usuario.USU_NOMBRE = sqlDataReader.GetString(2);
                        usuario.USU_PASSWORD = sqlDataReader.GetString(3);
                        usuario.USU_EMAIL = sqlDataReader.GetString(4);
                        usuario.USU_ESTADO = sqlDataReader.GetString(5);
                        usuario.USU_FECHA = sqlDataReader.GetDateTime(6);
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Ok(usuario);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(
                        System.Configuration.ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("SELECT USU_CODIGO, USU_IDENTIFICACION, " +
                        "USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, USU_FECHA " +
                        "FROM USUARIO ORDER BY USU_CODIGO", connection);

                    connection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            USU_CODIGO = sqlDataReader.GetInt32(0),
                            USU_IDENTIFICACION = sqlDataReader.GetString(1),
                            USU_NOMBRE = sqlDataReader.GetString(2),
                            USU_PASSWORD = sqlDataReader.GetString(3),
                            USU_EMAIL = sqlDataReader.GetString(4),
                            USU_ESTADO = sqlDataReader.GetString(5),
                            USU_FECHA = sqlDataReader.GetDateTime(6)
                        };
                        usuarios.Add(usuario);
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Ok(usuarios);
        }

        public IHttpActionResult Ingresar (Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();

            if (RegistraUsuario(usuario))
                return Ok(usuario);
            else
                return InternalServerError();
        }

        private bool RegistraUsuario(Usuario usuario)
        {
            bool resultado = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO USUARIO (USU_IDENTIFICACION, USU_NOMBRE, USU_PASSWORD, USU_EMAIL,USU_ESTADO, USU_FECHA) VALUES (@USU_IDENTIFICACION, @USU_NOMBRE, @USU_PASSWORD, @USU_EMAIL, @USU_ESTADO, @USU_FECHA)", connection);
                    command.Parameters.AddWithValue("@USU_IDENTIFICACION", usuario.USU_IDENTIFICACION);
                    command.Parameters.AddWithValue("@USU_NOMBRE", usuario.USU_NOMBRE);
                    command.Parameters.AddWithValue("@USU_PASSWORD", usuario.USU_PASSWORD);
                    command.Parameters.AddWithValue("@USU_EMAIL", usuario.USU_EMAIL);
                    command.Parameters.AddWithValue("@USU_ESTADO", usuario.USU_ESTADO);
                    command.Parameters.AddWithValue("@USU_FECHA", usuario.USU_FECHA);

                    connection.Open();

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                        resultado = true;
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }
    }
}
