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
				string sql = $"INSERT INTO Contratos (InmuebleId, InquilinoId, FechaInicio, FechaFin) " +
					$"VALUES (@inmuebleid, @inquilinoid, @fechainicio, @fechafin);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@inmuebleid", c.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoid", c.InquilinoId);
					command.Parameters.AddWithValue("@fechainicio", c.FechaInicio);
					command.Parameters.AddWithValue("@fechafin", c.FechaFin);
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
				string sql = $"DELETE FROM Contratos WHERE Id = @id";
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
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, InmuebleId, InquilinoId, FechaInicio, FechaFin " +
					$" FROM Contratos";
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
				string sql = $"SELECT Id, InmuebleId, InquilinoId, FechaInicio, FechaFin FROM Contratos" +
					$" WHERE Id = @id";
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
						};
					}
					connection.Close();
				}
			}
			return c;
		}
	}
}