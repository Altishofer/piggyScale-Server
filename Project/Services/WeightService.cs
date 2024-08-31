using System.Globalization;
using Microsoft.AspNetCore.Mvc;
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
    
    public async Task<Weight> Save(PostWeightDto postWeightDto)
    {
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        long weightId = await _weightRepository.NextWeightId();
        Weight weight = postWeightDto.ToEntity(dateTime, weightId);
        return await _weightRepository.SaveWeight(weight);
    }
    
    public async Task<List<Weight>> GetAllWeights()
    {
        return await _weightRepository.GetAllWeights();
    }
    
    public async Task<Weight> Delete(Weight weight)
    {
        return await _weightRepository.DeleteWeight(weight);
    }
    
    
    public async Task<Weight> DeleteLastByUserId(uint userId)
    {
        return await _weightRepository.DeleteLastByUserId(userId);
    }
    
    public async Task<List<Weight>> GetWeightsByBoxNumberAndDays(long boxId, uint days, long userId)
    {
        return await _weightRepository.GetWeightsByBoxNumberAndDays(boxId, days, userId);
    }
    
    public async Task<List<Weight>> ExportAllByUserId(long userId)
    {
        return await _weightRepository.GetWeightsByUserId(userId);
    }
    
    public async Task GenerateTestData()
    {
        var random = new Random();
        var weights = new List<Weight>();

        for (long userId = 1; userId <= 10; userId++)
        {
            for (int delta = 0; delta < 60; delta++)
            {
                for (int box = 1; box <= 3; box++)
                {
                    int weightValue = 100 - delta - random.Next(-2, 3);
                    double stddev = random.Next(100, 501) / 100.0;
                    DateTime dateTime = DateTime.Now.AddDays(-delta);
                    string dateTimeStr = dateTime.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
                    long weightId = await _weightRepository.NextWeightId();
                    PostWeightDto postWeightDto = new PostWeightDto(weightValue, (float)stddev, box, userId);

                    await _weightRepository.SaveWeight(postWeightDto.ToEntity(dateTimeStr, weightId));
                }
            }
        }
    }
}