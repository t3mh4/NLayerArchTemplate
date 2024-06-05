namespace NLayerArchTemplate.Core.Helper;

public class PasswordHelper
{
    // Hashes a password using BCrypt
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verifies a password against a hashed password
    public static bool VerifyPassword(string hashedPassword, string candidatePassword)
    {
        return BCrypt.Net.BCrypt.Verify(candidatePassword, hashedPassword);
    }
}
