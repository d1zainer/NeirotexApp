using NeirotexApp.MVVM.ViewModels;
using NeirotexApp.Services;
using NeirotexApp.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NeirotexApp.App
{
    public class ThreadController
    {
        private static readonly Lazy<ThreadController> _threadControllerInstance = new Lazy<ThreadController>(() => new ThreadController(new List<SignalViewModel>()));
        public static ThreadController Instance => _threadControllerInstance.Value;



        public readonly ConcurrentDictionary<SignalViewModel, (double, double, double)> _results = new ConcurrentDictionary<SignalViewModel, (double, double, double)>();

        private readonly List<SignalViewModel> _signals; //список вьюмоделей сигналов


        public ThreadController(List<SignalViewModel> signals)
        {
            _signals = signals;
            _results = new ConcurrentDictionary<SignalViewModel, (double, double, double)>(); ;
        }

        public void StartProcessing()
        {
            Thread mainThread = new Thread(ProcessFiles);
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
            var filePath = Path.Combine(FileDialog.folderPath, signal.SignalFileName);
            await calculator.ProcessFileAsync(filePath);
            _results[signal] = calculator.GetResults();
        }
    }
}
