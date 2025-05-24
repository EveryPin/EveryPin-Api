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
    public int PostPhotoId { get; set; }
    
    [ForeignKey(nameof(Post))]
    public int PostId { get; set; }
    public string? PhotoFileName { get; set; }

    public string? PhotoUrl { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public Post? Post { get; set; }
}

