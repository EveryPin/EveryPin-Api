﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class Profile
{
    [Column("ProfileId")]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public required string UserId { get; set; }
    public User? User { get; set; }

    public string? TagId { get; set; } = null!;
    public string? Name { get; set; }
    public string? SelfIntroduction { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime? UpdatedDate { get; set; } = null;
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

}
