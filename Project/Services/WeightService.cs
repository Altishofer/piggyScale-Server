using PiggyScaleApi.DTOs;
using PiggyScaleApi.Repositories;

namespace PiggyScaleApi.Services;

using System.Threading.Tasks;
using Models;

public class WeightService
{
    private readonly WeightRepository _weightRepository;

    public WeightService(ApplicationContext context)
    {
        _weightRepository = new WeightRepository(context);
    }
    
    public async Task<Weight> Save(WeightDto weightDto)
    {
        Weight weight = weightDto.ToEntity();
        weight.weightId = await _weightRepository.NextWeightId();
        return await _weightRepository.SaveWeight(weight);
    }
    
    public async Task<List<Weight>> GetAllWeights()
    {
        return await _weightRepository.GetAllWeights();
    }
    
    public void Delete(Weight weight)
    {
        _weightRepository.DeleteWeight(weight);
    }
}