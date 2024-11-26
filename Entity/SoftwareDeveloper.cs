using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BugandFixSoftwareCompany.Entity
{
    public class SoftwareDeveloper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Specialization { get; set; }
        public string? Title { get; set; } = "Unknown";
        public int Experience { get; set; }
    }
}
