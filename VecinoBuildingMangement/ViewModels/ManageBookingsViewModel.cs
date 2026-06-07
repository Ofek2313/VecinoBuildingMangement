using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageBookingsViewModel
    {
        public List<BookingResidentViewModel> PastBookings { get; set; } = new List<BookingResidentViewModel>();

        public List<BookingResidentViewModel> UpComingBookings { get; set; } = new List<BookingResidentViewModel>();

        public int AwaitingApproval { get; set; }

        public int AwaitingPayment { get; set; }

        public int Confirmed { get; set; }

        public int Rejected { get; set; }

    }
}
