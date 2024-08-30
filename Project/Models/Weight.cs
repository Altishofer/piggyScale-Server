using PiggyScaleApi.DTOs;

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
    
    public Weight(){}
    
    public Weight(long weightId, string dateTime, float weight, float stddev, uint boxNumber)
    {
        this.weightId = weightId;
        this.dateTime = dateTime;
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
    }

    public WeightDto ToDto(Weight weightObj)
    {
        WeightDto weightDto = new WeightDto();
        weightDto.weight = weightObj.weight;
        weightDto.stddev = weightObj.stddev;
        weightDto.dateTime = weightObj.dateTime;
        weightDto.boxNumber = weightObj.boxNumber;
        return weightDto;
    }
}