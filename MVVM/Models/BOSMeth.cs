using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeirotexApp.MVVM.Models
{

    [XmlRoot("BOSMeth")]
    public class BOSMeth
    {
        [XmlAttribute("TemplateGUID")]
        public string TemplateGUID { get; set; }

        [XmlElement("Channels")]
        public Channels Channels { get; set; }
    }

    public class Channels
    {
        [XmlElement("Channel")]
        public List<Channel> ChannelList { get; set; }
    }

    public class Channel
    {
        [XmlAttribute("UnicNumber")]
        public int UnicNumber { get; set; }

        [XmlAttribute("SignalFileName")]
        public string SignalFileName { get; set; }

        [XmlAttribute("Type")]
        public int Type { get; set; }

        [XmlAttribute("EffectiveFd")]
        public int EffectiveFd { get; set; }
    }
}
