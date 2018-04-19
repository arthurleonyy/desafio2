using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransacaoBancaria.Data;
using TransacaoBancaria.Models;

namespace TransacaoBancaria.Controllers
{
    public class ContaController : ApiController
    {
        private ContaDao contaDao = new ContaDao();

        [HttpGet]
        [Route("api/conta")]
        public IEnumerable<Conta> SelecionarTodos()
        {
            return contaDao.SelecionarTodos();
        }

        [HttpGet]
        [Route("api/conta/{id}")]
        public IHttpActionResult Selecionar(int id)
        {
            var conta = contaDao.Selecionar(id);

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));

            else
                return ResponseMessage(Request.CreateResponse<Conta>(HttpStatusCode.OK, conta));
        }

        [HttpGet]
        [Route("api/conta/{id}/saldo")]
        public IHttpActionResult PegarSaldo(int id)
        {
            var conta = contaDao.Selecionar(id);

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));

            else
                return ResponseMessage(Request.CreateResponse<object>(HttpStatusCode.OK, new { saldo = conta.Saldo }));
        }

        [HttpPost]
        [Route("api/conta")]
        public IHttpActionResult CriarConta([FromBody]Conta conta)
        {
            if (contaDao.Selecionar(conta.IdPessoa) == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "IdPessoa não existe"));

            if (conta.LimiteSaqueDiario < 100)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Limite de saque diario inferior a 100."));

            else if (conta.TipoConta < 1 || conta.TipoConta > 2)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "TipoConta inválido."));

            else
            {
                conta.FlagAtivo = true;
                conta.DataCriacao = DateTime.Now.Date;
                conta.Saldo = 0;

                contaDao.CriarConta(conta);
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.Created, "Conta criada com sucesso."));
            }
        }

        [HttpPut]
        [Route("api/conta/depositar")]
        public IHttpActionResult Depositar([FromBody]Conta model)
        {
            var conta = contaDao.Selecionar(model.IdConta);

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));

            else if (model.Saldo < 1)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Valor a ser depositado inferior a 1."));

            else
            {
                conta.Saldo = conta.Saldo + model.Saldo;
                contaDao.Depositar(conta, model.Saldo);
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.OK, "Depósito realizado com sucesso."));
            }
        }

        [HttpPut]
        [Route("api/conta/sacar")]
        public IHttpActionResult Sacar([FromBody]Conta model)
        {
            var conta = contaDao.Selecionar(model.IdConta);

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));

            else if (model.Saldo < 1)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Valor a ser sacado inferior a 1."));

            else if ((conta.Saldo - model.Saldo) < 0)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Saldo insulficiete"));

            else
            {
                conta.Saldo = (conta.Saldo - model.Saldo);
                contaDao.Depositar(conta, model.Saldo * (-1));
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.OK, "Saque realizado com sucesso."));
            }
        }

        [HttpDelete]
        [Route("api/conta/{id}")]
        public IHttpActionResult BloquearConta(int id)
        {
            var conta = contaDao.Selecionar(id);

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (conta.FlagAtivo == false)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta já se encontra desativada."));

            else
            {
                contaDao.BloquearConta(id);
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.OK, "Conta bloqueada com sucesso."));
            }
        }
    }
}
