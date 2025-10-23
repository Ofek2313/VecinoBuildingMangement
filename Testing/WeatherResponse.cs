using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.ViewModels;

namespace Testing
{
    public class WeatherResponse
    {
        public List<Weather> weather { get; set; }

        public Main main { get; set; }
    }
}
