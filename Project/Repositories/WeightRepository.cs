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
    
    public void DeleteWeight(Weight weight)
    {
        _context.Remove(weight);
    }

    public async Task<List<Weight>> GetWeightsByBoxNumber(uint boxNumber)
    {
        return await _context.Weight.Where(h => h.boxNumber == boxNumber).ToListAsync();
    }
    
    public async Task<List<Weight>> GetAllWeights()
    {
        return await _context.Weight.ToListAsync();
    }
    
    public async Task<long> NextWeightId()
    {
        return await _context.Weight.AnyAsync() ? (await _context.Weight.MaxAsync(u => u.weightId)) + 1 : 0;
    }

    public async Task<Weight> SaveWeight(Weight weight)
    {
        _context.Weight.Add(weight);
        await _context.SaveChangesAsync();
        return weight;
    }
}