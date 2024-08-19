using NeirotexApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeirotexApp.Services.Interfaces
{
    public interface IThemeSetting
    {
        public void SetTheme(string themeName);
        public void SaveTheme(string themeName);
        public void GetTheme(Settings settings);
    }
}
