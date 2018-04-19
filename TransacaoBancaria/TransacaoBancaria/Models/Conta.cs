using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransacaoBancaria.Models
{
    public class Conta
    {
        public int IdConta { get; set; }
        public int IdPessoa { get; set; }
        public decimal Saldo { get; set; }
        public decimal LimiteSaqueDiario { get; set; }
        public bool FlagAtivo { get; set; }
        public int TipoConta { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}