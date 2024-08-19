using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeirotexApp.UI
{
    public class ForegroundTextController
    {
        /// <summary>
        /// меняет цвет строки в зависимости от типа параметра
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static IBrush? GetBrushForMessageType(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Error => Brushes.Red,
                MessageType.Warning => Brushes.Orange,
                MessageType.Info => new SolidColorBrush(Color.Parse("#4C6A7A")),
                _ => new SolidColorBrush(Color.Parse("#4C6A7A"))
            };
        }

    }
    public enum MessageType
    {
        Error,
        Warning,
        Info,
    }
}
