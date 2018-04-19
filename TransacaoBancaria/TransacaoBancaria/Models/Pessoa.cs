using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransacaoBancaria.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}