using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TransacaoBancaria.Models;

namespace TransacaoBancaria.Data
{
    public class TransacaoDao
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ToString();

        public IEnumerable<Transacao> Selecionar(int idConta)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM transacao WHERE IdConta = @IdConta";
                return connection.Query<Transacao>(sql, new { IdConta = idConta }).AsEnumerable();
            }
        }

        public IEnumerable<Transacao> SelecionarPeriodo(UriTransacaoPeriodo model)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM transacao WHERE DataTransacao BETWEEN @DataInicial AND @DataFinal AND IdConta = @IdConta ";
                return connection.Query<Transacao>(sql, model).AsEnumerable();
            }
        }

    }
}