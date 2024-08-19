using NeirotexApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeirotexApp.Services.Interfaces
{
    public interface ILanguageSetting
    {
        public void SetCulture(string cultureCode);
        public void SaveCulture(string cultureCode);
        public void GetCulture(Settings settings);
    }
}
