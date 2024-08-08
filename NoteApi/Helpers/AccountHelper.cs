using System;
using System.Security.Cryptography;
using System.Text;

namespace NotesApi;

public enum Role
{
    Administrator = 1,
    User = 2,
    Guest = 3
}

public class AccountHelper
{
    public static string GetStringRole(Role role) => role switch
    {
        Role.Administrator => "Administrator",
        Role.User => "User",
        Role.Guest => "Guest",
        _ => throw new KeyNotFoundException()
    };

    // Метод для генерации соли
    public static byte[] GenerateSalt(int size = 10)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[size];
            rng.GetBytes(salt);
            return salt;
        }
    }

    // Метод для хэширования пароля с использованием соли
    public static string HashPassword(string password, byte[] salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Конкатенируем пароль и соль
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];

            // копирует байты пароля в массив passwordWithSalt
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSalt, 0, passwordBytes.Length);
            // копирует соль сразу после пароля
            Buffer.BlockCopy(salt, 0, passwordWithSalt, passwordBytes.Length, salt.Length);

            // Вычисляем хэш
            byte[] hash = sha256.ComputeHash(passwordWithSalt);

            // Преобразуем массив байтов в строку Base64 и возвращаем
            return Convert.ToBase64String(hash);
        }
    }

    // Метод для проверки пароля
    public static bool VerifyPassword(string password, byte[] salt, string passwordHash)
    {
        var hashToVerify = HashPassword(password, salt);
        return string.Equals(hashToVerify, passwordHash);
    }
}