using System;
using System.Collections.Generic;

namespace LogAudit.Models
{
    public partial class Syslogd
    {

        public string? MsgDate { get; set; }
        public string? MsgTime { get; set; }
        public string? MsgPriority { get; set; }
        public string? MsgHostname { get; set; }
        public string? MsgText { get; set; }
    }
}
