using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using T4sV1.Services;

namespace T4sV1.Model.ViewModels
{
    public class ResourceViewModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public ICommand PlayCommand { get; }

        public ResourceViewModel(string url, string title, string image, Action<string> onPlayCallback)
        {
            Title = title;
            Url = url;
            Image = image;

            PlayCommand = new Command(() =>
            {
                onPlayCallback?.Invoke(Url);
            });
        }

    }

}
