using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase
    {
		public RepositorioContrato(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Contrato c)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Contratos (InmuebleId, InquilinoId, FechaInicio, FechaFin, Estado, PrecioInmueble) " +
					$"VALUES (@inmuebleid, @inquilinoid, @fechainicio, @fechafin, @estado, @precioinmueble);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@inmuebleid", c.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoid", c.InquilinoId);
					command.Parameters.AddWithValue("@fechainicio", c.FechaInicio);
					command.Parameters.AddWithValue("@fechafin", c.FechaFin);
					command.Parameters.AddWithValue("@precioinmueble", c.PrecioInmueble);
					command.Parameters.AddWithValue("@estado", 1);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					c.Id = res;
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
				string sql = $"UPDATE Contratos SET Estado = 0 WHERE Id = @id";
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
		public int Renovar(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Contratos SET Estado = 2 WHERE Id = @id";
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
		public int Modificacion(Contrato c)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Contratos SET InmuebleId=@inmuebleid, InquilinoId=@inquilinoid, FechaInicio=@fechainicio, FechaFin=@fechafin " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@inmuebleid", c.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoid", c.InquilinoId);
					command.Parameters.AddWithValue("@fechainicio", c.FechaInicio);
					command.Parameters.AddWithValue("@fechafin", c.FechaFin);
					command.Parameters.AddWithValue("@id", c.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			//string sql = $"SELECT Contratos.Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Contratos.Estado, inm.Id, inm.Direccion, inm.Uso, inq.Id ,inq.Nombre, inq.Apellido, pro.Id, pro.Nombre, pro.Apellido "  Contratos JOIN Inmuebles AS inm ON Contratos.InmuebleId = inm.Id JOIN Inquilinos AS inq ON Contratos.InquilinoId = inq.Id JOIN Propietarios AS pro ON inm.PropietarioId = pro.Id WHERE Contratos.Estado = 1";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Contratos.Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Contratos.Estado, inm.Id, inm.Direccion, inm.Uso, inq.Id ,inq.Nombre, inq.Apellido, pro.Id, pro.Nombre, pro.Apellido, inm.Estado, inq.Estado, pro.Estado, PrecioInmueble " +
					$" FROM Contratos JOIN Inmuebles AS inm ON Contratos.InmuebleId = inm.Id JOIN Inquilinos AS inq ON Contratos.InquilinoId = inq.Id JOIN Propietarios AS pro ON inm.PropietarioId = pro.Id WHERE Contratos.Estado != 0 ORDER BY Contratos.Id DESC";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
							InmuebleId = reader.GetInt32(1),
							InquilinoId = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),
							FechaFin = reader.GetDateTime(4),
							Estado = reader.GetInt32(5),
							PrecioInmueble = reader.GetInt32(18),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion = reader.GetString(7),
								Uso = reader.GetString(8),
								Estado = reader.GetInt32(15),
							},
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(9),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),
								Estado = reader.GetInt32(16),
							},
							Propietario = new Propietario
							{
								Id = reader.GetInt32(12),
								Nombre = reader.GetString(13),
								Apellido = reader.GetString(14),
								Estado = reader.GetInt32(17),
							}
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Contrato ObtenerPorId(int id)
		{
			Contrato c = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				//string sql = $"SELECT Id, InmuebleId, InquilinoId, FechaInicio, FechaFin FROM Contratos" +
				//	$" WHERE Id = @id";
				string sql = $"SELECT Contratos.Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Contratos.Estado, inm.Id, inm.Direccion, inm.Uso, inq.Id ,inq.Nombre, inq.Apellido, pro.Id, pro.Nombre, pro.Apellido, inm.Estado, inq.Estado, pro.Estado, inm.Precio, PrecioInmueble " +
					$" FROM Contratos JOIN Inmuebles AS inm ON Contratos.InmuebleId = inm.Id JOIN Inquilinos AS inq ON Contratos.InquilinoId = inq.Id JOIN Propietarios AS pro ON inm.PropietarioId = pro.Id WHERE Contratos.Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						c = new Contrato
						{
							Id = reader.GetInt32(0),
							InmuebleId = reader.GetInt32(1),
							InquilinoId = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),
							FechaFin = reader.GetDateTime(4),
							Estado = reader.GetInt32(5),
							PrecioInmueble = reader.GetInt32(19),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion = reader.GetString(7),
								Uso = reader.GetString(8),
								Estado = reader.GetInt32(15),
								Precio = reader.GetInt32(18),
							},
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(9),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),
								Estado = reader.GetInt32(16),
							},
							Propietario = new Propietario
							{
								Id = reader.GetInt32(12),
								Nombre = reader.GetString(13),
								Apellido = reader.GetString(14),
								Estado = reader.GetInt32(17),
							}
						};
					}
					connection.Close();
				}
			}
			return c;
		}

		public bool ComprobarPorInmuebleYFechas(int Id, DateTime Inicio, DateTime Fin, int Ignorar = 0) //Usar el parámetro ignorar para la edición de contratos
        {
			bool res = true;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id FROM Contratos WHERE Estado != 0 AND InmuebleId=@InmuebleId AND ((FechaFin >= @Inicio AND FechaFin <= @Fin) OR (FechaInicio <= @Fin AND FechaInicio >= @Inicio) OR (FechaInicio <= @Inicio AND FechaFin >= @Fin)) AND Id != @Ignorar";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@InmuebleId", SqlDbType.Int).Value = Id;
					command.Parameters.Add("@Ignorar", SqlDbType.Int).Value = Ignorar;
					command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = Inicio;
					command.Parameters.Add("@Fin", SqlDbType.DateTime).Value = Fin;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						res = false;
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public int TotalPagos(int id)
		{
			int TotalPagos = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT COUNT(Id) FROM Pagos WHERE ContratoId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						TotalPagos = reader.GetInt32(0);
					}
					connection.Close();
				}
			}
			return TotalPagos;
		}

		public int Cancelar(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Contratos SET Estado=@Estado, FechaFin=@FechaFin " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Estado", 0);
					command.Parameters.AddWithValue("@FechaFin", DateTime.Today);
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Contrato> ObtenerPorFiltro(string sqlWhere)
		{
			IList<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Contratos.Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Contratos.Estado, inm.Id, inm.Direccion, inm.Uso, inq.Id ,inq.Nombre, inq.Apellido, pro.Id, pro.Nombre, pro.Apellido, inm.Estado, inq.Estado, pro.Estado " +
					$" FROM Contratos JOIN Inmuebles AS inm ON Contratos.InmuebleId = inm.Id JOIN Inquilinos AS inq ON Contratos.InquilinoId = inq.Id JOIN Propietarios AS pro ON inm.PropietarioId = pro.Id " + sqlWhere;
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
							InmuebleId = reader.GetInt32(1),
							InquilinoId = reader.GetInt32(2),
							FechaInicio = reader.GetDateTime(3),
							FechaFin = reader.GetDateTime(4),
							Estado = reader.GetInt32(5),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion = reader.GetString(7),
								Uso = reader.GetString(8),
								Estado = reader.GetInt32(15),
							},
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(9),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),
								Estado = reader.GetInt32(16),
							},
							Propietario = new Propietario
							{
								Id = reader.GetInt32(12),
								Nombre = reader.GetString(13),
								Apellido = reader.GetString(14),
								Estado = reader.GetInt32(17),
							}
						};
						res.Add(c);
					}
					connection.Close();
				}
				return res;
			}
		}

	}
}