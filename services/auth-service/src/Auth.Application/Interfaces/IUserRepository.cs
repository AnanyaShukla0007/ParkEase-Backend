using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);

    Task<User?> FindByIdAsync(int userId);

    Task<User?> FindByPhoneAsync(string phone);

    Task<User?> FindByVehiclePlateAsync(string plate);

    Task<List<User>> FindAllByRoleAsync(string role);

    Task<bool> ExistsByEmailAsync(string email);

    Task<bool> ExistsByPhoneAsync(string phone);

    Task UpdateAsync(User user);

    Task DeleteAsync(int userId);

    Task<List<User>> GetAllAsync();
}