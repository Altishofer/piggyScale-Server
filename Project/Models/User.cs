using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PiggyScaleApi.Models;

[Index(nameof(User.userId), IsUnique = true)]
public class User
{
    [Required]
    [Column("userid")]
    public long userId { get; set; }
    [Required]
    [Column("username")]
    public string userName { get; set; }
    [Required]
    [Column("userpassword")]
    public string userPassword { get; set; }
    
    public User(){}
    
    public User(long userId, string userName, string userPassword)
    {
        this.userId = userId;
        this.userName = userName;
        this.userPassword = userPassword;
    }
}