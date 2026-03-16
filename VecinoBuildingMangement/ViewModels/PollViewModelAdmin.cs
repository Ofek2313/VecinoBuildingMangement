using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.ViewModels
{
    public class PollViewModelAdmin : PollViewModel
    {
        public int TotalVotes { get; set; } = 0;

        public int ParticipationRate { get; set; }

        public int DaysLeft
        {
            get
            {
                if (DateTime.TryParse(poll.PollDate, out DateTime end))
                {
                    var span = end - DateTime.Now;
                    return (int)Math.Max(0, Math.Ceiling(span.TotalDays));
                }
                return 0;
            }

        }
    }
}
