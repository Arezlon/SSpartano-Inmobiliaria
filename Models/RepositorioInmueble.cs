using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase
    {
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{

		}
		public int Alta(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmuebles (PropietarioId, Direccion, Uso, Tipo, Ambientes, Precio, Estado) " +
					$"VALUES (@propietarioid, @direccion, @uso, @tipo, @ambientes, @precio, @estado);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@propietarioid", i.PropietarioId);
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@uso", i.Uso);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@precio", i.Precio);
					command.Parameters.AddWithValue("@estado", 1);

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
				//string sql = $"DELETE FROM Inmuebles WHERE Id = @id";
				string sql = $"UPDATE Inmuebles SET Estado = 0 WHERE Id = @id";
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
		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmuebles SET PropietarioId=@propietarioid, Direccion=@direccion Uso=@uso, Tipo=@tipo, Ambientes=@ambientes, Precio=@precio, Estado=@estado " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@propietarioid", i.PropietarioId);
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@uso", i.Uso);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@precio", i.Precio);
					command.Parameters.AddWithValue("@estado", i.Estado);
					command.Parameters.AddWithValue("@id", i.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Inmuebles.Id, PropietarioId, Direccion, Uso, Tipo, Ambientes, Precio, Inmuebles.Estado, " +
					$"p.Id, p.Nombre, p.Apellido, p.Dni, p.Telefono, p.Email, p.Clave, p.Estado" +
					$" FROM Inmuebles JOIN Propietarios AS p ON Inmuebles.PropietarioId = p.Id WHERE Inmuebles.Estado = 1";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							PropietarioId = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							Uso = reader.GetString(3),
							Tipo = reader.GetString(4),
							Ambientes = reader.GetInt32(5),
							Precio = reader.GetInt32(6),
							Estado = reader.GetInt32(7),
							Propietario = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
								Dni = reader.GetString(11),
								Telefono = reader["Telefono"].ToString(),
								Email = reader.GetString(13),
								Clave = reader.GetString(14),
							}
					};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, PropietarioId, Direccion, Uso, Tipo, Ambientes, Precio, Estado" +
					$" FROM Inmuebles WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							Id = reader.GetInt32(0),
							PropietarioId = reader.GetInt32(1),
							Direccion = reader.GetString(2),
							Uso = reader.GetString(3),
							Tipo = reader.GetString(4),
							Ambientes = reader.GetInt32(5),
							Precio = reader.GetInt32(6),
							Estado = reader.GetInt32(7),
						};
					}
					connection.Close();
				}
			}
			return i;
		}
	}
}