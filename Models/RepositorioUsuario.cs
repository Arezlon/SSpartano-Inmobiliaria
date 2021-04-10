using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class RepositorioUsuario : RepositorioBase
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {

        }
    }
}