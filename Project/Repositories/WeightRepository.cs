using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    
    public async Task<Weight> DeleteWeight(Weight weight)
    {
        _context.Remove(weight);
        await _context.SaveChangesAsync();
        return weight;
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
    
    public async Task<Weight> DeleteLastByUserId(uint userId)
    {
        Weight? weight = await _context.Weight.Where(w => w.userId == userId).OrderByDescending(w => w.weightId).FirstOrDefaultAsync();
        if (weight != null)
        {
            _context.Weight.Remove(weight);
            await _context.SaveChangesAsync();
            return weight;
        }
        throw new Exception("Weight with userId " + userId + " not found");
    }

    public async Task<List<Weight>> GetWeightsByBoxNumberAndDays(long boxId, uint days, long userId)
    {
        if (days == 0)
        {
            days = 1000;
        }

        DateTime currentDate = DateTime.Now;
        DateTime earlierDate = currentDate.AddDays(-days);

        List<Weight> weights = await _context.Weight
            .Where(w => w.boxNumber == boxId && w.userId == userId)
            .ToListAsync();

        return weights
            .Where(w => DateTime.ParseExact(w.dateTime, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture) >= earlierDate &&
                        DateTime.ParseExact(w.dateTime, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture) <= currentDate)
            .ToList();
    }
    
    public async Task<List<Weight>> GetWeightsByUserId(long userId)
    {
        return await _context.Weight.Where(w => w.userId == userId).ToListAsync();
    }
}