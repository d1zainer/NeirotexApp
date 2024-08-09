using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.Services;
using NeirotexApp.UI;

namespace NeirotexApp.App;

public class ThreadController
{
    private static readonly Lazy<ThreadController> ThreadControllerInstance = new(() => new ThreadController(new List<SignalViewModel>()));

    private readonly List<SignalViewModel> _signals; //список вьюмоделей сигналов

    public readonly ConcurrentDictionary<SignalViewModel, (double, double, double)> Results;

    
    public ThreadController(List<SignalViewModel> signals)
    {
        _signals = signals;
        Results = new ConcurrentDictionary<SignalViewModel, (double, double, double)>();
    }

    public static ThreadController Instance => ThreadControllerInstance.Value;

    public async Task StartProcessingAsync()
    {
        var tasks = _signals.Select(signal => Task.Run(() => ReadFromFileAsync(signal))).ToArray();
        await Task.WhenAll(tasks);
    }

    private async Task ReadFromFileAsync(SignalViewModel signal)
    {
        var calculator = new SignalValueService(signal.EffectiveFd);
        var filePath = Path.Combine(FileDialog.FolderPath, signal.SignalFileName);

        // Обработка файла асинхронно
        await calculator.ProcessFileAsync(filePath);

        // Сохранение результатов в ConcurrentDictionary
        Results[signal] = calculator.GetResults();
    }

   
}