using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeShare.Domain;

namespace WeShare.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IStudentServices _studentServices;

        public ValuesController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }

        public int Get()
        {
            return _studentServices.GetId();
        }
    }
}
