using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int TotalChildren { get; set; }
        public int TotalHealthScores { get; set; }
        public int TotalWeightLogs { get; set; }

        public ObservableCollection<NotificationDto> Notifications { get; set; } = new();
        public ObservableCollection<TimelineActivityDto> Activities { get; set; } = new();
        public ObservableCollection<MeasurementDto> Measurements { get; set; } = new();
        public ObservableCollection<HealthScoreDto> HealthScores { get; set; } = new();
        public ObservableCollection<ChildDto> Children { get; set; } = new();


        public event PropertyChangedEventHandler PropertyChanged;
    }

}
