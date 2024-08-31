using System.Globalization;

namespace PiggyScaleApi.DTOs;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PiggyScaleApi.Models;


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
    
    [Required]
    [Column("userid")]
    public long userId { get; set; }
    
    public PostWeightDto(){}
    
    public Weight ToEntity(string _dateTime, long _weightId)
    {
        Weight weight = new Weight();
        weight.dateTime = _dateTime;
        weight.weight = this.weight;
        weight.stddev = this.stddev;
        weight.boxNumber = this.boxNumber;
        weight.userId = this.userId;
        weight.weightId = _weightId;
        return weight;
    }
    
    public PostWeightDto(float weight, float stddev, long boxNumber, long userId)
    {
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
        this.userId = userId;
    }
}