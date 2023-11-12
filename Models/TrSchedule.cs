using System;
using System.Collections.Generic;

namespace BrazingControlAPI.Models
{
    public partial class TrSchedule
    {
        public int ScheduleCode { get; set; }
        public string? CourseId { get; set; }
        public DateTime? ScheduleStart { get; set; }
        public DateTime? ScheduleEnd { get; set; }
        public string? ScheduleDetail { get; set; }
        public string? CourseExamSet { get; set; }
        public decimal? TrainDay { get; set; }
        public string? TrainerType { get; set; }
        public string? TrainerCompany { get; set; }
        public string? Trainer { get; set; }
        public string? Company { get; set; }
        public string? Indicator { get; set; }
        public string? InstructionMedia { get; set; }
        public string? FileName { get; set; }
        public string? Cby { get; set; }
        public DateTime? Cdate { get; set; }
        public string? Uby { get; set; }
        public DateTime? Udate { get; set; }
        public string? LocationType { get; set; }
        public string? Location { get; set; }
        public bool? Certificate { get; set; }
        public string? Status { get; set; }
        public int? Mark { get; set; }
    }
}
