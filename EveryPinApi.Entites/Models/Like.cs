   using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class Like
{
    [Key]
    public int LikeId { get; set; }
    public int PostId { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey("PostId")]
    public virtual Post? Post { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
    
}
