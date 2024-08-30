using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyScaleApi.Models;
using Weight = PiggyScaleApi.Models.Weight;

namespace PiggyScaleApi.Repositories;

public class WeightRepository
{
    private readonly ApplicationContext _context;

    public WeightRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public void DeleteWeight(Weight Weight)
    {
        _context.Remove(Weight);
    }

    public async Task<List<Weight>> GetAllWeights()
    {
        return _context.Weight.ToList();
    }
    
    public async Task<Weight?> GetWeightByWeightName(string WeightName)
    {
        return await _context.Weight.FirstOrDefaultAsync(h => h.weightName == WeightName);
    }
    
    public async Task<bool> WeightExistsByWeightName(string WeightName)
    {
        return await _context.Weight.AnyAsync(h => h.weightName == WeightName);
    }

    public async Task<long> NextWeightId()
    {
        return await _context.Weight.AnyAsync() ? (await _context.Weight.MaxAsync(u => u.weightId)) + 1 : 0;
    }

    public async Task<Weight> SaveWeight(Weight Weight)
    {
        _context.Weight.Add(Weight);
        await _context.SaveChangesAsync();
        return Weight;
    }
}