using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransacaoBancaria.Data;
using TransacaoBancaria.Models;

namespace TransacaoBancaria.Controllers
{
    public class TransacaoController : ApiController
    {
        private TransacaoDao transacaoDao = new TransacaoDao();
        private ContaDao contaDao = new ContaDao();

        [HttpGet]
        [Route("api/transacao/{idConta}")]
        public IHttpActionResult Selecionar(int idConta)
        {

            var conta = contaDao.Selecionar(idConta);

            if(conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Número de conta inexistente."));

            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));

            else
            {
                var transacoes = transacaoDao.Selecionar(idConta);
                return ResponseMessage(Request.CreateResponse<IEnumerable<Transacao>>(HttpStatusCode.OK, transacoes));
            }
                
        }

        [HttpGet]
        [Route("api/transacao")]
        public IHttpActionResult SelecionarPeriodo([FromUri]UriTransacaoPeriodo uri)
        {
            var conta = contaDao.Selecionar(uri.IdConta);
            DateTime d;

            if (conta == null)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "IdConta inexistente."));
                
            else if (!conta.FlagAtivo)
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "A conta se encontra desativada."));
                
            else if (!DateTime.TryParseExact(uri.DataInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d) ||
                !DateTime.TryParseExact(uri.DataFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Data inicial inválida."));

            else
            {
                var transacoes = transacaoDao.SelecionarPeriodo(uri);
                return ResponseMessage(Request.CreateResponse<IEnumerable<Transacao>>(HttpStatusCode.OK, transacoes));
            }
        }
    }
}
