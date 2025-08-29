using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model
{
    public class HealthScoreDto
    {
        public int Id { get; set; }
        public int PhysicalActivityScore { get; set; }
        public int BreakfastScore { get; set; }
        public int FruitVegScore { get; set; }
        public int SweetSnacksScore { get; set; }
        public int FattyFoodsScore { get; set; }
        public int TotalScore { get; set; }
        public string HealthClassification { get; set; }
        public DateTime DateRecorded { get; set; }

        // Computed properties
        public string PhysicalActivityDescription => PhysicalActivityScore switch
        {
            0 => "Less than 60 mins",
            1 => "60-120 mins",
            2 => "180-240 mins",
            3 => "300-360 mins",
            4 => "420+ mins",
            _ => "Unknown"
        };

        public string BreakfastDescription => BreakfastScore switch
        {
            0 => "0 days",
            1 => "1-2 days",
            2 => "3-4 days",
            3 => "5-6 days",
            4 => "7 days",
            _ => "Unknown"
        };

        public string FruitVegDescription => FruitVegScore switch
        {
            0 => "0 portions",
            1 => "1-2 portions",
            2 => "3 portions",
            3 => "4 portions",
            4 => "5+ portions",
            _ => "Unknown"
        };

        public string SweetSnacksDescription => SweetSnacksScore switch
        {
            0 => "6+ times a week",
            1 => "3-5 times a week",
            2 => "1-2 times a week",
            3 => "Less than 1 a week",
            4 => "Never",
            _ => "Unknown"
        };

        public string FattyFoodsDescription => FattyFoodsScore switch
        {
            0 => "6+ times a week",
            1 => "3-5 times a week",
            2 => "1-2 times a week",
            3 => "Less than 1 a week",
            4 => "Never",
            _ => "Unknown"
        };
    }
}
