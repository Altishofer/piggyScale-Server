
namespace PiggyScaleApi.DTOs;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models;


public class PostWeightDto
{
    
    [Required]
    [Column("box")]
    public long boxNumber { get; set; }
    
    [Required]
    [Column("weight")]
    public float weight { get; set; }
    
    [Required]
    [Column("stddev")]
    public float stddev { get; set; }
    
    
    public PostWeightDto(){}
    
    public Weight ToEntity(string _dateTime, long _weightId, long _userId)
    {
        Weight weight = new Weight();
        weight.dateTime = _dateTime;
        weight.weight = this.weight;
        weight.stddev = this.stddev;
        weight.boxNumber = this.boxNumber;
        weight.weightId = _weightId;
        weight.userId = _userId;
        return weight;
    }
    
    public PostWeightDto(float weight, float stddev, long boxNumber)
    {
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
    }
}