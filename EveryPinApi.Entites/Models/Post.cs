using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class Post
{
    [Key]
    public int PostSeq { get; set; }
    public required string UserId { get; set; }
    public string? PostContent { get; set; }
    public string? Address { get; set; }
    public float? X { get; set; }
    public float? Y { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey("UserId")]
    public virtual required User User { get; set; }
    public virtual ICollection<PostPhoto> PostPhotos { get; set; } = new List<PostPhoto>();
    public virtual ICollection<Like> Likes { get; } = new List<Like>();    
    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
}
