using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class User : IdentityUser
{
    public override required string Id { get; set; }
    public int? PlatformCode { get; set; }
    public string? FcmToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastLoginDate { get; set; }
    public bool DeleteCheck { get; set; }

    [ForeignKey("PlatformCode")]
    public virtual required CodeOAuthPlatform CodeOAuthPlatform { get; set; }
    public virtual Profile? Profile { get; set; }
    public virtual ICollection<Like> Like { get; set; } = new List<Like>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Post> Post { get; set; } = new List<Post>();
    public virtual ICollection<Follow> FollowingList { get; set; } = new List<Follow>();
    public virtual ICollection<Follow> FollowerList { get; set; } = new List<Follow>();
}
