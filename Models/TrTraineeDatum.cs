using System;
using System.Collections.Generic;

namespace BrazingControlAPI.Models
{
    public partial class TrTraineeDatum
    {
        public int ScheduleDetCode { get; set; }
        public string? ScheduleCode { get; set; }
        public string? CourseCode { get; set; }
        public string? Empcode { get; set; }
        public string? ExamSetCode { get; set; }
        public string? PreTestResult { get; set; }
        public string? PostTestResult { get; set; }
        public string? EvaluateResult { get; set; }
        public string? Cby { get; set; }
        public DateTime? Cdate { get; set; }
        public string? Uby { get; set; }
        public DateTime? Udate { get; set; }
        public int? Recordstatus { get; set; }
        public DateTime? Expire { get; set; }
    }
}
