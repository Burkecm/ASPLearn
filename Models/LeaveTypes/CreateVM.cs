using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class CreateVM
    {
        [Required]
        [Length(4, 150,  ErrorMessage = "Name length does not meet Requirements")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 90)]
        [Display(Name="Number of Days")]
        public int DaysAllocated { get; set; }
    }
}
