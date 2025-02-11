using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class PostPhoto
{
    [Key]
    public int PostPhotoSeq { get; set; }
    
    [ForeignKey(nameof(Post))]
    public int PostSeq { get; set; }

    public string? PhotoUrl { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public Post? Post { get; set; }
}

