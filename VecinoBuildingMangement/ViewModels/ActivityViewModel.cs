using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.ViewModels
{
    public class ActivityViewModel
    {

        public string ActivityTitle { get; set; }

        public string ActivityDate { get; set; }

        public string ActivityType { get; set; }

        public string ResidentName { get; set; }

        public string ActivityDescription {
            get
            {
                switch (ActivityType)
                {
                    case "Fee":
                        return $"{ResidentName} Has Paid For {ActivityTitle} ";
                    case "Vote":
                        return $"{ResidentName} Has Voted On A Poll";
                    case "Request":
                        return $"{ResidentName} Has Opened A Service Request: {ActivityTitle}";
                    default:
                        return "Recent activity recorded";
                       
                };

            }
            
        }
    }
}
