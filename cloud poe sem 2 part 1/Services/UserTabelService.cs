using Azure.Data.Tables;
using cloud_poe_sem_2_part_1.Models;

namespace cloud_poe_sem_2_part_1.Services;

public class UserTableService
{
    private readonly TableClient _tableClient;

    public UserTableService(string connectionString, string tableName = "UserProfiles")
    {
        var serviceClient = new TableServiceClient(connectionString);
        _tableClient = serviceClient.GetTableClient(tableName);
        _tableClient.CreateIfNotExists();
    }

    public async Task<UserEntity?> GetUserAsync(string username)
    {
        try
        {
            var entity = await _tableClient.GetEntityAsync<UserEntity>("Users", username);
            return entity.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task AddUserAsync(UserEntity user)
    {
        await _tableClient.AddEntityAsync(user);
    }
}
