using NeirotexApp.MVVM.Models;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.UI;
using NeirotexApp.UI.Managers;
using System;
using System.IO;
using System.Xml.Serialization;

namespace NeirotexApp.Services
{
    public class XMLService
    {


        /// <summary>
        /// загружает и парсит
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static  BOSMeth? LoadBosMethFromXml(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(BOSMeth));
                using (var reader = new StreamReader(filePath))
                {
                    return (BOSMeth)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MainWindowViewModel.InformationStringAction?.DynamicInvoke(LanguageManager.InfoMessageType.ErrorMessage, MessageType.Error, ex.Message);
                throw;
            }
        }
    }
}
