using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class RepositorioPago : RepositorioBase
    {
		public RepositorioPago(IConfiguration configuration) : base(configuration)
		{

		}
		public int Alta(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pagos (ContratoId, Fecha, Estado) VALUES (@ContratoId, @Fecha, @Estado);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@ContratoId", p.ContratoId);
					command.Parameters.AddWithValue("@Fecha", DateTime.Today);
					command.Parameters.AddWithValue("@Estado", 1);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.Id = res;
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
				string sql = $"UPDATE Pagos SET Estado = 0 WHERE Id = @id;";

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

		public int Modificacion(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Pagos SET ContratoId=@ContratoId, Fecha=@Fecha WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@ContratoId", p.ContratoId);
					command.Parameters.AddWithValue("@Fecha", p.Fecha);
					command.Parameters.AddWithValue("@id", p.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, ContratoId, Fecha FROM Pagos WHERE Estado = 1 ORDER BY Id DESC";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							Id = reader.GetInt32(0),
							ContratoId = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Pago ObtenerPorId(int id)
		{
			RepositorioInmueble ri = new RepositorioInmueble(configuration);
			RepositorioInquilino rinq = new RepositorioInquilino(configuration);
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				//string sql = $"SELECT Id, ContratoId, Fecha FROM Pagos WHERE Estado = 1 AND Id = @id";
				string sql = $"SELECT Pagos.Id, ContratoId, Fecha, cn.Id, cn.InmuebleId, cn.InquilinoId, cn.FechaInicio, cn.FechaFin FROM Pagos JOIN Contratos AS cn ON Pagos.ContratoId = cn.Id WHERE Pagos.Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							Id = reader.GetInt32(0),
							ContratoId = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Contrato = new Contrato
							{
								Id = reader.GetInt32(3),
								InmuebleId = reader.GetInt32(4),
								InquilinoId = reader.GetInt32(5),
								FechaInicio = reader.GetDateTime(6),
								FechaFin = reader.GetDateTime(7),
								Inmueble = ri.ObtenerPorId(reader.GetInt32(4)),
								Inquilino = rinq.ObtenerPorId(reader.GetInt32(5)),
							}
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public IList<Pago> ObtenerPorContrato(int idC)
		{
			RepositorioInmueble ri = new RepositorioInmueble(configuration);
			RepositorioInquilino rinq = new RepositorioInquilino(configuration);
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Pagos.Id, ContratoId, Fecha, cn.Id, cn.InmuebleId, cn.InquilinoId, cn.FechaInicio, cn.FechaFin FROM Pagos JOIN Contratos AS cn ON Pagos.ContratoId = cn.Id WHERE ContratoId = @idC ORDER BY Pagos.Id DESC";
				//string sql = $"SELECT Id, ContratoId, Fecha FROM Pagos WHERE Estado = 1 AND ContratoId = @idC";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					//command.Parameters.Add("@idC", SqlDbType.Int).Value = idC;
					command.Parameters.AddWithValue("idC", idC);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						
						Pago p = new Pago
						{
							Id = reader.GetInt32(0),
							ContratoId = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Contrato = new Contrato
							{
								Id = reader.GetInt32(3),
								InmuebleId = reader.GetInt32(4),
								InquilinoId = reader.GetInt32(5),
								FechaInicio = reader.GetDateTime(6),
								FechaFin = reader.GetDateTime(7),
								Inmueble = ri.ObtenerPorId(reader.GetInt32(4)),
								Inquilino = rinq.ObtenerPorId(reader.GetInt32(5)),
							}
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
