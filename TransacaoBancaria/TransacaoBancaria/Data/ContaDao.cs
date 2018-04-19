using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Web;
using System.Configuration;
using TransacaoBancaria.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace TransacaoBancaria.Data
{
    public class ContaDao
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ToString();

        public IEnumerable<Conta> SelecionarTodos()
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM conta WHERE FlagAtivo = true";
                return connection.Query<Conta>(sql).ToList();
            }
        }

        public Conta Selecionar(int id)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM conta WHERE IdConta = @Id";
                return connection.Query<Conta>(sql, new { Id = id }).FirstOrDefault();
            }
        }

        public void CriarConta(Conta model)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "INSERT INTO `conta` (`IdPessoa`, Saldo, `LimiteSaqueDiario`, `FlagAtivo`, TipoConta, DataCriacao) VALUES" +
                          " (@IdPessoa, @Saldo, @LimiteSaqueDiario, @FlagAtivo, @TipoConta, @DataCriacao )";

                connection.Execute(sql, model);
            }
        }

        public void Depositar(Conta model, decimal valorTransacao)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var sqlConta = "UPDATE conta SET " +
                              "Saldo = @Saldo " +
                              "WHERE IdConta = @IdConta";
                    connection.Execute(sqlConta, model, transaction: transaction);

                    var sqlTransacao = "INSERT INTO transacao (IdConta, Valor, DataTransacao) VALUES " +
                                       "(" + model.IdConta + ", @Valor, @DataTransacao)";
                    connection.Execute(sqlTransacao, new
                    {
                        Valor = valorTransacao,
                        DataTransacao = DateTime.Now.ToString("yyyy-MM-dd")
                    }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }

        public void Sacar(Conta model, double valorTransacao)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var sqlConta = "UPDATE conta SET " +
                              "Saldo = @Saldo " +
                              "WHERE IdConta = @IdConta";
                    connection.Execute(sqlConta, model, transaction: transaction);

                    var sqlTransacao = "INSERT INTO transacao (IdConta, Valor, DataTransacao) VALUES " +
                                       "(" + model.IdConta + ", @Valor, @DataTransacao)";
                    connection.Execute(sqlTransacao, new
                    {
                        Valor = valorTransacao,
                        DataTransacao = DateTime.Now.ToString("yyyy-MM-dd")
                    }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }

        public void BloquearConta(int id)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var sql = "UPDATE conta SET " +
                          "FlagAtivo = false " +
                          "WHERE IdConta = @IdConta";

                connection.Execute(sql, new { IdConta = id });
            }
        }

    }
}