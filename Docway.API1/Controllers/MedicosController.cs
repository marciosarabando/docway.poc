using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable getAll()
        {
            return new string[] { "Dr. Drauzio", "Dr. Dayan Siebra", "Dr. Barakat"};
        }
    }
}
