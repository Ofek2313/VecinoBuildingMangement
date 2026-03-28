using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.DTO;

namespace VecinoWpfApp
{
    public static class Session
    {

        public static string BuildingId { get; set; } = "";

        public static bool HasAccount { get; set; } = false;

        public static CreateBuildingRegister PendingRegistration { get; set; } = new CreateBuildingRegister();

        public static string PendingImagePath { get; set; } = "";

        public static void Clear()
        {
            BuildingId = "";
            HasAccount = false;

            PendingRegistration = new CreateBuildingRegister();
        }
    }
}
