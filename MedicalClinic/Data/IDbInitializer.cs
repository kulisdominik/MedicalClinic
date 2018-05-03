using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Data
{
    public interface IDbInitializer
    {
        void Initialize(IConfiguration configuration);
    }
}
