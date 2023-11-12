using System;
using System.Collections.Generic;

namespace BrazingControlAPI.Models
{
    public partial class TrCourse
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? CourseNameEn { get; set; }
        public string? CourseType { get; set; }
        public string? CourseVdoPath { get; set; }
        public decimal? CoursePerPerson { get; set; }
        public string? CourseExamSet { get; set; }
        public DateTime? Cdate { get; set; }
        public string? Cby { get; set; }
        public DateTime? Udate { get; set; }
        public string? Uby { get; set; }
        public string? CourseStatus { get; set; }
        public string? Group { get; set; }
        public string? Line { get; set; }
        public bool? ExpireStatus { get; set; }
    }
}
