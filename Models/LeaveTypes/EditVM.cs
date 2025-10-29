using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class EditVM : BaseLeaveTypeVM
    {
        [Required]
        [Length(4, 150, ErrorMessage = "Name length does not meet Requirements")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 90)]
        public int DaysAllocated { get; set; }
    }
}
