//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Maui.Controls;
//using T4sV1.Model.Dashboard;
//using T4sV1.Model.ViewModels;
//using T4sV1.Services;
//// Alias to ensure correct DashboardService
//using IHttpDashboardService = T4sV1.Services.Http.IDashboardService;

//namespace T4sV1.Views
//{
//    [QueryProperty(nameof(ChildId), "childId")]
//    public partial class DashboardPage : ContentPage
//    {
//        private readonly IHttpDashboardService _dashboard;
//        private readonly ISessionService _session;
//        private readonly DashboardViewModel _vm;

//        public int? ChildId { get; set; }

//        public DashboardPage(IHttpDashboardService dashboard, ISessionService session, DashboardViewModel vm)
//        {
//            InitializeComponent();
//            _dashboard = dashboard;
//            _session = session;
//            _vm = vm; // ✅ Injected via DI
//            BindingContext = _vm;
//        }

//        protected override async void OnAppearing()
//        {
//            base.OnAppearing();

//            // ✅ Read childId (from Login or session)
//            int? selectedChildId = ChildId;

//            if (selectedChildId is null || selectedChildId <= 0)
//            {
//                _session.LoadFromPreferences();
//                selectedChildId = _session.ActiveChildId;
//            }

//            if (selectedChildId is int cid && cid > 0)
//            {
//                // ✅ Tell ViewModel which child to load
//                _vm.SelectedChild = new ChildSummaryLiteDto { Id = cid };
//                await _vm.InitialiseAsync(); // triggers data fetch
//            }
//            else
//            {
//                // No child found → take user to child setup page
//                await Shell.Current.GoToAsync("//ChildCreate");
//            }
//        }
//    }
//}
