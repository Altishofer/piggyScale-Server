namespace PiggyScaleApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


[Index(nameof(Weight.weightId), IsUnique = true)]
public class Weight
{
    [Required]
    [Column("weightid")]
    public long weightId { get; set; }
    [Required]
    [Column("weightname")]
    public string weightName { get; set; }
    [Required]
    [Column("weightpassword")]
    public string weightPassword { get; set; }
    
    public Weight(){}
    
    public Weight(long weightId, string weightName, string weightPassword)
    {
        this.weightId = weightId;
        this.weightName = weightName;
        this.weightPassword = weightPassword;
    }
}