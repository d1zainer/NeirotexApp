﻿using Avalonia.Controls;
using NeirotexApp.UI.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NeirotexApp.UI
{
    public class FileDialog
    {
        private static string _folderPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        public static string FolderPath => _folderPath;

        // Метод для проверки существования папки и создания её при необходимости
        private static void EnsureFolderExists()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
        }

        public async Task<string?> ShowOpenFileDialog(Window parentWindow)
        {
            // Убедитесь, что папка существует перед показом диалога
            EnsureFolderExists();
            var title = LanguageManager.Instance.GetTitleByCulture(LanguageManager.TitleType.FileDialog);
            var dialog = new OpenFileDialog
            {
                Title = title,
                AllowMultiple = false,
                Directory = FolderPath,

                // Установите начальную директорию на папку Data
                Filters = new List<FileDialogFilter>
                {
                    new () { Name = ".xml ", Extensions = new List<string> { "xml" } }
                }
            };

            var result = await dialog.ShowAsync(parentWindow);
            return result?.FirstOrDefault();
        }
    }
}
