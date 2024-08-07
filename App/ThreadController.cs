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

    public void StartProcessing()
    {
        var mainThread = new Thread(ProcessFiles);
        mainThread.Start();
        mainThread.Join();
    }

    private void ProcessFiles()
    {
        var tasks = _signals.Select(signal =>
            Task.Run(() => ReadFromFile(signal))).ToArray();
        Task.WaitAll(tasks);
    }

    private async Task ReadFromFile(SignalViewModel signal)
    {
        var calculator = new SignalValueService(signal.EffectiveFd);
        var filePath = Path.Combine(FileDialog.FolderPath, signal.SignalFileName);
        await calculator.ProcessFileAsync(filePath);
        Results[signal] = calculator.GetResults();
    }
}