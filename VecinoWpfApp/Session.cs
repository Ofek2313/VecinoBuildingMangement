using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;

namespace VecinoWpfApp
{
    public static class Session //Static Session Class To Store Info Across Pages.
    {

        public static Building CurrentBuilding { get; set; }

        public static string BuildingId { get; set; } = "";

        public static string ResidentId { get; set; } = string.Empty;

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
