using NeirotexApp.App;
using NeirotexApp.MVVM.Models;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.UI;
using System;
using System.IO;
using System.Xml.Serialization;

namespace NeirotexApp.Services
{
    public class XMLService
    {

        private  static  string _xmlAppSettings =>  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.xml");
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
                MainWindowViewModel.InformationStringAction?.DynamicInvoke(LangController.InfoMessageType.ErrorMessage, MessageType.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Загружает настройки приложения
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static AppSettings LoadAppSettingsXml()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(AppSettings));
                using (var reader = new StreamReader(_xmlAppSettings))
                {
                    return (AppSettings)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Сохраняет настройки приложения
        /// </summary>
        public static void SaveAppSettingsXml(AppSettings settings)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(AppSettings));
                using (var writer = new StreamWriter(_xmlAppSettings))
                {
                    serializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                throw ex;
            }
        }



    }
}
