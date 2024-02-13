using System;

public class AuthUtil
{

    public static string HashPassword(string password)
    {
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        {
            // Convert the password string to bytes
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            // Compute the hash value of the password bytes
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash bytes to a hexadecimal string
            string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashedPassword;
        }
    }
}