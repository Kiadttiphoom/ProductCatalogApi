namespace ProductCatalogApi.Authentication;

public interface IJwtService
{
    string GenerateToken(string userId, string username);
    bool ValidateToken(string token);
}
