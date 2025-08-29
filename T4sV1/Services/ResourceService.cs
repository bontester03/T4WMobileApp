using System;
using System.Collections.Generic;
using T4sV1.Model.ViewModels;

namespace T4sV1.Services
{
    public class ResourceService
    {
        private readonly AllResourcesViewModel allResources;

        public List<ResourceViewModel> WellbeingResources { get; }
        public List<ResourceViewModel> ChallengeResources { get; }
        public List<ResourceViewModel> PhysicalActivityResources { get; }
        public List<ResourceViewModel> FatsAndSugarsResources { get; }
        public List<ResourceViewModel> PortionSizesResources { get; }
        public List<ResourceViewModel> WhatIsATriggerResources { get; }
        public List<ResourceViewModel> UnhealthySnacksResources { get; }
        public List<ResourceViewModel> ScreenTimeResources { get; }
        public List<ResourceViewModel> BreakfastResources { get; }
        public List<ResourceViewModel> HealthySnacksResources { get; }
        public List<ResourceViewModel> OtherResources { get; }

        public ResourceService(Action<string> playVideo)
        {
            WellbeingResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("5 Ways to Wellbeing", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=WHcLb0h9FmI"), playVideo),
                new ResourceViewModel("Overcoming Barriers Part 1", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=acxBR2K6dhs"), playVideo),
                new ResourceViewModel("Overcoming Barriers Part 2", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=KUHWjclsHjo"), playVideo),
            };

            ChallengeResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Folding Towel Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=N8xhLIXOaFQ"), playVideo),
                new ResourceViewModel("Bottle Flip Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=KV681XB1DOo"), playVideo),
                new ResourceViewModel("Launch Pad Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=cgZvgESF3c8"), playVideo),
                new ResourceViewModel("Paper in a Cup Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=yeCfbqKHxcg"), playVideo),
                new ResourceViewModel("Sit Up Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=b73F9uRzSEY"), playVideo),
                new ResourceViewModel("Indoor Golf Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=MyoIqVxdNO0"), playVideo),
                new ResourceViewModel("Juggling Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=7ZprS1uFrO4"), playVideo),
                new ResourceViewModel("The Teddy Bear Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=2kuzrHS5Isc"), playVideo),
                new ResourceViewModel("ABC Pushup Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=mPK6N15gXRs"), playVideo),
                new ResourceViewModel("Blind Throw Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=g31NUIYfj6s"), playVideo),
                new ResourceViewModel("The Valentines Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=fhp3LaQt8nw"), playVideo),
                new ResourceViewModel("The Book Balance Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=p6fmtNgNJC8"), playVideo),
                new ResourceViewModel("Kick The Cup Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=r4peK3gNK3s"), playVideo),
                new ResourceViewModel("Take The Bins Out Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=o8jRsCBFpkE"), playVideo),
                new ResourceViewModel("Catch The Paper Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=cOYAf_4u7lU"), playVideo),
                new ResourceViewModel("Skater Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=aJO-DrPnJDg"), playVideo),
                new ResourceViewModel("The Snowman Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=oUIDkEY_OwI"), playVideo),
                new ResourceViewModel("Unlock Your Phone Challenge", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=8oA_uDdH5EA"), playVideo),
            };

            PhysicalActivityResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("How Active Have You Been?", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=nTrfGo3sIiU"), playVideo),
            };

            FatsAndSugarsResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Fats and Sugars Part 1", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=R0O51IUtmH4"), playVideo),
                new ResourceViewModel("Fats and Sugars Part 2", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=LheFxCohPdc"), playVideo),
                new ResourceViewModel("Fats and Sugars Part 3", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=4hMzjDuodG8"), playVideo),
                new ResourceViewModel("Fats and Sugars Part 4", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=fEr6ce9gHNs"), playVideo),
            };

            PortionSizesResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Portion Sizes Part 1", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=s2_qjyVwE8U"), playVideo),
                new ResourceViewModel("Portion Sizes Part 2", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=9eTx9gpQNEU"), playVideo),
            };

            WhatIsATriggerResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("What is a Trigger", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=QSHFBKcrdcw"), playVideo),
            };

            UnhealthySnacksResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Reduce Unhealthy Snacks", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=-rTCF4tGBzQ"), playVideo),
                new ResourceViewModel("Dealing With Cravings", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=mdCIuvNCTWQ"), playVideo),
            };

            ScreenTimeResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Reduce Screen Time", "wellbeingways.png", GetEmbedUrl("https://youtu.be/ZtLi13JczUU"), playVideo),
            };

            BreakfastResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Breakfast Days", "wellbeingways.png", GetEmbedUrl("https://youtu.be/7JYkJ-rGxiQ"), playVideo),
            };

            HealthySnacksResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Barriers Rewards", "wellbeingways.png", GetEmbedUrl("https://youtu.be/6oUm7PlRgck"), playVideo),
                new ResourceViewModel("Healthy Eating On A Budget Part 1", "wellbeingways.png", GetEmbedUrl("https://youtu.be/Ti6A09ZkDRM"), playVideo),
                new ResourceViewModel("Healthy Eating On A Budget Part 2", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=A4KOUxcp3Xg"), playVideo),
                new ResourceViewModel("Health By Numbers", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=4rAsZgtm7Ik"), playVideo),
                new ResourceViewModel("Healthy Eating Recipes Dinner", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=569DMNEpHOk"), playVideo),
                new ResourceViewModel("Healthy Eating Recipes Lunch", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=j4RFxo1sXbU"), playVideo),
                new ResourceViewModel("Healthy Eating Recipes Breakfast", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=xc_PQU3bu60"), playVideo),
            };

            OtherResources = new List<ResourceViewModel>
            {
                new ResourceViewModel("Making Sense Of Labels", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=NRkmaL7mULE"), playVideo),
                new ResourceViewModel("Slow and Fast Release Foods", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=i5MYGMQMtHw"), playVideo),
                new ResourceViewModel("Party Time and Takeouts", "wellbeingways.png", GetEmbedUrl("https://www.youtube.com/watch?v=2eM9QIGtWQo"), playVideo),
            };

            allResources = new AllResourcesViewModel
            {
                WellbeingResources = WellbeingResources,
                ChallengeResources = ChallengeResources,
                PhysicalActivityResources = PhysicalActivityResources,
                FatsAndSugarsResources = FatsAndSugarsResources,
                PortionSizesResources = PortionSizesResources,
                WhatIsATriggerResources = WhatIsATriggerResources,
                UnhealthySnacksResources = UnhealthySnacksResources,
                ScreenTimeResources = ScreenTimeResources,
                BreakfastResources = BreakfastResources,
                HealthySnacksResources = HealthySnacksResources,
                OtherResources = OtherResources
            };
        }

        private string GetEmbedUrl(string originalUrl)
        {
            if (originalUrl.Contains("watch?v="))
                return originalUrl.Replace("watch?v=", "embed/");
            if (originalUrl.Contains("youtu.be/"))
                return originalUrl.Replace("youtu.be/", "www.youtube.com/embed/");
            return originalUrl;
        }

        public AllResourcesViewModel GetAllResources() => allResources;
    }
}
