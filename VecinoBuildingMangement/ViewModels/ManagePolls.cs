﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManagePolls
    {
        public int PollNumbers { get; set; }
        public List<Poll> polls { get; set; }
        public List<Vote> votes { get; set; }
    }
}
