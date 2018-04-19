using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransacaoBancaria.Models
{
    public class UriTransacaoPeriodo
    {
        public int IdConta { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
    }
}