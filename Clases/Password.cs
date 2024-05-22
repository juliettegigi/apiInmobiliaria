using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace proyecto1.Clases;

public class Password
{
    private readonly string Salt;

    public Password(string salt)
    {
        Salt = salt;
    }

    public string HashPassword(string password)
    {
        
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(Salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
        
        return hashed;
    }

    public bool EsIgual(string passIngresada, string passDB)
    {
        
        return HashPassword(passIngresada) == passDB;
    }
}

