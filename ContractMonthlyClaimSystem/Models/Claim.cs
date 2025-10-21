#nullable enable
using System.ComponentModel.DataAnnotations;

namespace ContractMonthlyClaimSystem.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public string Module { get; set; }
        public int HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? statusCoord { get; set; }
        public string? statusManager { get; set; }

        [Display (Name = "Submitted Document")]
        public string? DocumentPath { get; set; }
    }
}
