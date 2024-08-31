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
    
    public Weight(){}
    
    public Weight(long weightId, string dateTime, float weight, float stddev, long boxNumber, long userId)
    {
        this.weightId = weightId;
        this.dateTime = dateTime;
        this.boxNumber = boxNumber;
        this.weight = weight;
        this.stddev = stddev;
        this.userId = userId;
    }

    public GetWeightDto ToDto()
    {
        GetWeightDto getWeightDto = new GetWeightDto();
        getWeightDto.weight = this.weight;
        getWeightDto.stddev = this.stddev;
        getWeightDto.dateTime = this.dateTime;
        getWeightDto.boxNumber = this.boxNumber;
        getWeightDto.userId = this.userId;
        return getWeightDto;
    }
}