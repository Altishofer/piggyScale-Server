using Microsoft.EntityFrameworkCore;
using PiggyScaleApi.Models;
using User = PiggyScaleApi.Models.User;

namespace PiggyScaleApi.Repositories;

public class UserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public void DeleteUser(User User)
    {
        _context.Remove(User);
    }
    
    public async Task<int> DeleteAll()
    {
        List<User> allUsers = await _context.User.ToListAsync();
        _context.User.RemoveRange(allUsers);
        return await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsers()
    {
        return _context.User.ToList();
    }
    
    public async Task<User?> GetUserByUserName(string UserName)
    {
        return await _context.User.FirstOrDefaultAsync(h => h.userName == UserName);
    }
    
    public async Task<bool> UserExistsByUserName(string UserName)
    {
        return await _context.User.AnyAsync(h => h.userName == UserName);
    }

    public async Task<long> NextUserId()
    {
        return await _context.User.AnyAsync() ? (await _context.User.MaxAsync(u => u.userId)) + 1 : 0;
    }

    public async Task<User> SaveUser(User User)
    {
        _context.User.Add(User);
        await _context.SaveChangesAsync();
        return User;
    }
}