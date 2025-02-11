using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models;

public class CodeOAuthPlatform
{
    [Key]
    public int PlatformCode { get; set; }
    public required string PlatformName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
