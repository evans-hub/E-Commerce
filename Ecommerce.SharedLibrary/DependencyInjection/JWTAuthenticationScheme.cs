using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.SharedLibrary.DependencyInjection
{
    public class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthentication(this ServiceCollection services, IConfiguration config)
        {//Add JWT Service
            services.AddAuthentication(JwtBearerDefaults)
        }
    }
}
