using NeirotexApp.MVVM.Models;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.ResourcesLang;
using NeirotexApp.UI;
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
        public static  BOSMeth LoadBOSMethFromXml(string filePath)
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
                MainWindowViewModel.InformationStringAction?.DynamicInvoke(LangController.InfoMessageType.ErrorMessage, MessageType.Error, ex.Message);
                throw ex;
            }
        }
    }
}
