
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PiggyScaleApi.Models;

namespace PiggyScaleApi.DTOs;

public class WeightDto
{
    [Required]
    public string weightName { get; set; }
    [Required]
    public string weightPassword { get; set; }
    
    [JsonConstructor]
    public WeightDto(){}

    public WeightDto(Weight weight)
    {
        weightName = weight.weightName;
        weightPassword = weight.weightPassword;
    }
    
    public WeightDto(string weightName, string weightPassword)
    {
        this.weightName = weightName;
        this.weightPassword = weightPassword;
    }
    
    public Weight toEntity()
    {
        Weight weight = new Weight();
        weight.weightPassword = weightPassword;
        weight.weightName = weightName;
        
        return weight;
    }
}