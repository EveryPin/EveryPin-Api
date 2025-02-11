using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class Comment
{
    [Key]
    public int CommentSeq { get; set; }
    public required int PostSeq { get; set; }
    public required string UserId { get; set; }
    public string? CommentMessage { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey("PostSeq")]
    public virtual required Post Post { get; set; }
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }
}
