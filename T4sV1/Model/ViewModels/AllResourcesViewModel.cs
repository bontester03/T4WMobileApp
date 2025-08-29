using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.ViewModels
{
    public class AllResourcesViewModel
    {
        public List<ResourceViewModel> WellbeingResources { get; set; }
        public List<ResourceViewModel> ChallengeResources { get; set; }
        public List<ResourceViewModel> PhysicalActivityResources { get; set; }
        public List<ResourceViewModel> FatsAndSugarsResources { get; set; }
        public List<ResourceViewModel> PortionSizesResources { get; set; }
        public List<ResourceViewModel> WhatIsATriggerResources { get; set; }
        public List<ResourceViewModel> UnhealthySnacksResources { get; set; }
        public List<ResourceViewModel> ScreenTimeResources { get; set; }
        public List<ResourceViewModel> BreakfastResources { get; set; }
        public List<ResourceViewModel> HealthySnacksResources { get; set; }
        public List<ResourceViewModel> OtherResources { get; set; }
    }
}
