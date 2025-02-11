using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class Profile
{
    [Key]
    public int ProfileSeq { get; set; }
    public required string UserId { get; set; }
    public required string ProfileTag { get; set; }
    public required string ProfileName { get; set; }
    public string? SelfIntroduction { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public required DateTime CreatedDate { get; set; }

    [ForeignKey("UserId")]
    public virtual required User User { get; set; }
}
