
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PiggyScaleApi.Models;

namespace PiggyScaleApi.DTOs;

public class WeightDto
{
    
    [Required]
    [Column("datetime")]
    public string dateTime { get; set; }
    
    [Required]
    [Column("box")]
    public uint boxNumber { get; set; }
    
    [Required]
    [Column("weight")]
    public float weight { get; set; }
    
    [Required]
    [Column("stddev")]
    public float stddev { get; set; }
    
    public WeightDto(){}
    
    public Weight ToEntity()
    {
        Weight weight = new Weight();
        weight.dateTime = this.dateTime;
        weight.weight = this.weight;
        weight.stddev = this.stddev;
        weight.boxNumber = this.boxNumber;
        return weight;
    }
    
    public WeightDto(string dateTime, float weight, float stddev, uint boxNumber)
    {
        this.dateTime = dateTime;
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
    }
}