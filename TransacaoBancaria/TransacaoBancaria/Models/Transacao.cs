using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransacaoBancaria.Models
{
    public class Transacao
    {
        public int IdTransacao { get; set; }
        public int IdConta { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; }
    }
}