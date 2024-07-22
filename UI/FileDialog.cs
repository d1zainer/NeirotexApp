using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NeirotexApp.UI
{
    public class FileDialog
    {
        static  string appDirectory => AppDomain.CurrentDomain.BaseDirectory;
        static  string projectPath => appDirectory.Substring(0, appDirectory.IndexOf("\\bin"));

        public static readonly string folderPath = Path.Combine(projectPath, "Data");
        public async Task<string?> ShowOpenFileDialog(Window parentWindow)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите файл",
                AllowMultiple = false,
                Directory = folderPath,

                // Установите начальную директорию на папку Data
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "XML файлы", Extensions = new List<string> { "xml" } }
                }
            };

            var result = await dialog.ShowAsync(parentWindow);
            return result?.FirstOrDefault();
        }
    }
}
