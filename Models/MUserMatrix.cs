namespace BrazingControlAPI.Models
{
    public class MUserMatrix
    {
        public int index {  get; set; }
        public string? empcode { get; set; }
        public string? fullName { get; set; }
        public string? type { get; set; }   
        public Dictionary<string, bool> course { get; set; }
    }
}
