using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
	public class RepositorioInquilino : RepositorioBase
	{
		public RepositorioInquilino(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Inquilino i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Telefono, Email, Estado, LugarTrabajo, GaranteNombre, GaranteApellido, GaranteDni, GaranteTelefono, GaranteEmail) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email, @estado, @lugartrabajo, @garantenombre, @garanteapellido, @garantedni, @garantetelefono, @garanteemail);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", i.Nombre);
					command.Parameters.AddWithValue("@apellido", i.Apellido);
					command.Parameters.AddWithValue("@dni", i.Dni);
					command.Parameters.AddWithValue("@telefono", i.Telefono);
					command.Parameters.AddWithValue("@email", i.Email);
					command.Parameters.AddWithValue("@estado", 1);
					command.Parameters.AddWithValue("@lugartrabajo", i.LugarTrabajo);
					command.Parameters.AddWithValue("@garantenombre", i.GaranteNombre);
					command.Parameters.AddWithValue("@garanteapellido", i.GaranteApellido);
					command.Parameters.AddWithValue("@garantedni", i.GaranteDni);
					command.Parameters.AddWithValue("@garantetelefono", i.GaranteTelefono);
					command.Parameters.AddWithValue("@garanteemail", i.GaranteEmail);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.Id = res;
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
				string sql = $"UPDATE Inquilinos SET Estado = 0 WHERE Id = @id; UPDATE Contratos SET Estado = 0 WHERE InquilinoId = @id;";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilinos SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email, LugarTrabajo=@lugartrabajo, GaranteNombre=@garantenombre, GaranteApellido=@garanteapellido, GaranteDni=@garantedni, GaranteTelefono=@garantetelefono, GaranteEmail=@garanteemail" +
					$" WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", i.Nombre);
					command.Parameters.AddWithValue("@apellido", i.Apellido);
					command.Parameters.AddWithValue("@dni", i.Dni);
					command.Parameters.AddWithValue("@telefono", i.Telefono);
					command.Parameters.AddWithValue("@email", i.Email);
					command.Parameters.AddWithValue("@id", i.Id);
					command.Parameters.AddWithValue("@lugartrabajo", i.LugarTrabajo);
					command.Parameters.AddWithValue("@garantenombre", i.GaranteNombre);
					command.Parameters.AddWithValue("@garanteapellido", i.GaranteApellido);
					command.Parameters.AddWithValue("@garantedni", i.GaranteDni);
					command.Parameters.AddWithValue("@garantetelefono", i.GaranteTelefono);
					command.Parameters.AddWithValue("@garanteemail", i.GaranteEmail);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email, LugarTrabajo, GaranteNombre, GaranteApellido, GaranteDni, GaranteTelefono, GaranteEmail" +
					$" FROM Inquilinos WHERE Estado = 1";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							LugarTrabajo = reader.GetString(6),
							GaranteNombre = reader.GetString(7),
							GaranteApellido = reader.GetString(8),
							GaranteDni = reader.GetString(9),
							GaranteTelefono = reader.GetString(10),
							GaranteEmail = reader.GetString(11),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email, LugarTrabajo, GaranteNombre, GaranteApellido, GaranteDni, GaranteTelefono, GaranteEmail" +
					$" FROM Inquilinos WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							LugarTrabajo = reader.GetString(6),
							GaranteNombre = reader.GetString(7),
							GaranteApellido = reader.GetString(8),
							GaranteDni = reader.GetString(9),
							GaranteTelefono = reader.GetString(10),
							GaranteEmail = reader.GetString(11),
						};
					}
					connection.Close();
				}
			}
			return i;
		}
	}
}
