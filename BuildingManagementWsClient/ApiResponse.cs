using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingManagementWsClient
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }   
        public T? Data { get; set; }
    }
}
