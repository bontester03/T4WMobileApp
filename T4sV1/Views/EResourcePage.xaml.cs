using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using T4sV1.Services;
using T4sV1.Model.ViewModels;


namespace T4sV1.Views
{

  

    public partial class EResourcePage : ContentPage
    {
        public AllResourcesViewModel AllResources { get; set; }

        public ObservableCollection<string> ResourceCategories { get; set; }
        public ObservableCollection<ResourceViewModel> CurrentCategoryVideos { get; set; }

        public EResourcePage()
        {
            InitializeComponent();

            // 🔹 1. Create the resourceService with video playback callback
            var resourceService = new ResourceService(url =>
            {
                VideoPlayer.Source = url;
                VideoPlayer.IsVisible = true;
            });

            // 🔹 2. Define categories
            ResourceCategories = new ObservableCollection<string>
    {
        "Wellbeing",
        "Challenges",
        "Physical Activity",
        "Fats & Sugars",
        "Portion Sizes",
        "Triggers",
        "Unhealthy Snacks",
        "Screen Time",
        "Breakfast",
        "Healthy Snacks",
        "Other"
    };

            // 🔹 3. Get resources from service (after declaration!)
            AllResources = resourceService.GetAllResources();

            // 🔹 4. Set initial video list
            CurrentCategoryVideos = new ObservableCollection<ResourceViewModel>(AllResources.WellbeingResources);

            // 🔹 5. Set data context for binding
            BindingContext = this;
        }


        private void OnSegmentClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var category = button.CommandParameter?.ToString();

            var selectedList = category switch
            {
                "Wellbeing" => AllResources.WellbeingResources,
                "Challenges" => AllResources.ChallengeResources,
                "Physical Activity" => AllResources.PhysicalActivityResources,
                "Fats and Sugars" => AllResources.FatsAndSugarsResources,
                "Portion Sizes" => AllResources.PortionSizesResources,
                "Triggers" => AllResources.WhatIsATriggerResources,
                "Unhealthy Snacks" => AllResources.UnhealthySnacksResources,
                "Screen Time" => AllResources.ScreenTimeResources,
                "Breakfast" => AllResources.BreakfastResources,
                "Healthy Snacks" => AllResources.HealthySnacksResources,
                "Other" => AllResources.OtherResources,
                _ => new List<ResourceViewModel>()
            };

            CurrentCategoryVideos.Clear();
            foreach (var video in selectedList)
                CurrentCategoryVideos.Add(video);

            VideoPlayer.IsVisible = false; // Hide video when category changes
        }


        private void OnVideoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string url)
            {
                VideoPlayer.Source = url;
                VideoPlayer.IsVisible = true;
            }
        }

        private void OnExpanderCollapsing(object sender, EventArgs e)
        {
            VideoPlayer.IsVisible = false;
            VideoPlayer.Source = null;
        }




    }
}


