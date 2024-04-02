using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ServiceExceptions
{
    public class ContentServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) { }
}
