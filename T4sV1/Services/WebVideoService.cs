using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Services
{
    public class WebVideoService
    {
        public void OpenVideo(string url)
        {
            Launcher.Default.OpenAsync(new Uri(url));
        }
    }

}
