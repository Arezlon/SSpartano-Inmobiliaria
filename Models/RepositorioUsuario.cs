using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class RepositorioUsuario : RepositorioBase
    {
		public RepositorioUsuario(IConfiguration configuration) : base(configuration)
		{

		}
		public int Alta(Usuario u)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuarios (Nombre, Apellido, Email, Clave, TipoCuenta, Estado) " +
					$"VALUES (@Nombre, @Apellido, @Email, @Clave, @TipoCuenta, @Estado);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Nombre", u.Nombre);
					command.Parameters.AddWithValue("@Apellido", u.Apellido);
					command.Parameters.AddWithValue("@Email", u.Email);
					command.Parameters.AddWithValue("@Clave", u.Clave);
					command.Parameters.AddWithValue("@TipoCuenta", 1);
					command.Parameters.AddWithValue("@Estado", 1);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					u.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Estado = 0 WHERE Id = @Id;";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Restaurar(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Estado = 1 WHERE Id = @Id;";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Usuario u)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email, TipoCuenta=@TipoCuenta WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Nombre", u.Nombre);
					command.Parameters.AddWithValue("@Apellido", u.Apellido);
					command.Parameters.AddWithValue("@Email", u.Email);
					command.Parameters.AddWithValue("TipoCuenta", u.TipoCuenta);
					//command.Parameters.AddWithValue("@Clave", u.Clave);
					command.Parameters.AddWithValue("@id", u.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Email, TipoCuenta, Estado FROM Usuarios ORDER BY Id DESC";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario u = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Email = reader.GetString(3),
							TipoCuenta = reader.GetInt32(4),
							Estado = reader.GetInt32(5),
						};
						res.Add(u);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Usuario ObtenerPorId(int id)
		{
			Usuario u = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Email, TipoCuenta, Estado FROM Usuarios WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						u = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Email = reader.GetString(3),
							TipoCuenta = reader.GetInt32(4),
							Estado = reader.GetInt32(5),
						};
					}
					connection.Close();
				}
			}
			return u;
		}

		virtual public Usuario ObtenerPorEmail(string email)
		{
			Usuario u = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Email, TipoCuenta, Estado, Clave FROM Usuarios WHERE Email = @Email";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						u = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Email = reader.GetString(3),
							TipoCuenta = reader.GetInt32(4),
							Estado = reader.GetInt32(5),
							Clave = reader.GetString(6),
						};
					}
					connection.Close();
				}
			}
			return u;
		}
	}
}