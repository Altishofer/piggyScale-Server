
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PiggyScaleApi.Models;

namespace PiggyScaleApi.DTOs;

public class GetWeightDto
{
    
    [Required]
    [Column("datetime")]
    public string dateTime { get; set; }
    
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
    
    public GetWeightDto(){}
    
    public Weight ToEntity()
    {
        Weight weight = new Weight();
        weight.dateTime = this.dateTime;
        weight.weight = this.weight;
        weight.stddev = this.stddev;
        weight.boxNumber = this.boxNumber;
        weight.userId = this.userId;
        return weight;
    }
    
    public GetWeightDto(string dateTime, float weight, float stddev, long boxNumber, long userId)
    {
        this.dateTime = dateTime;
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
        this.userId = userId;
    }
}