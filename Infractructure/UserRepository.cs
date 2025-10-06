using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Interfaces;

namespace SportsNewsAPI.Infractructure;

public class UserRepository : IUserRepository
{
    private readonly SportsNewsContext _context;
    private readonly IMapper _mapper;

    public UserRepository(SportsNewsContext context)
    {
        _context = context;
    }
    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.User.AsNoTracking().FirstOrDefaultAsync(e => e.Email == email) ??
                         throw new Exception();

        return _mapper.Map<User>(userEntity);
    }
}