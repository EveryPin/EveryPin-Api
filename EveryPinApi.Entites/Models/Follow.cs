using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;
public class Follow
{
    [Key]
    public int FollowSeq { get; set; }
    public required string FollowerId { get; set; }
    public required string FollowingId { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey("FollowerId")]
    public virtual required User Follower { get; set; }
    [ForeignKey("FollowingId")]
    public virtual required User Following { get; set; }
}
