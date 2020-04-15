using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Docway.API1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicosController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IEnumerable<Medicos> getAll()
        {
            var medico1 = new Medicos();
            medico1.Id = 1;
            medico1.Nome = "Dr. Dayan Siebra";
            medico1.Especialidade = "Ortomolecular";

            var medico2 = new Medicos();
            medico2.Id = 2;
            medico2.Nome = "Dr. Barakat";
            medico2.Especialidade = "Nutrólogo";

            var medico3 = new Medicos();
            medico3.Id = 3;
            medico3.Nome = "Dr. Vitor Azzini";
            medico3.Especialidade = "Nutriendocrinologia";

            var medicos = new List<Medicos>();
            medicos.Add(medico1);
            medicos.Add(medico2);
            medicos.Add(medico3);

            //return new string[] { "Dr. Drauzio", "Dr. Dayan Siebra", "Dr. Barakat"};
            return medicos;
        }

    }

    public class Medicos
    {
        public int Id  { get; set; }
        public string Nome { get; set; }
        public string Especialidade  { get; set; }
    }
}
