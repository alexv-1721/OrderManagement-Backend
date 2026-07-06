using Microsoft.EntityFrameworkCore;
using OrderManagement.API.DataContext;
using OrderManagement.API.DTOs;
using OrderManagement.API.Model;

namespace OrderManagement.API.Repositories
{
    public class UserRepository
    {
        private readonly AppDBContext _context;
        public UserRepository(AppDBContext dBContext) { _context = dBContext; }

        public async Task<UserModel> RegisterUser(UserModel user)
        {
            try
            {
                await _context.Users.AddAsync(user);
               await _context.SaveChangesAsync();
                return user;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return user;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<UserModel?> FindByEmail(string email)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}
