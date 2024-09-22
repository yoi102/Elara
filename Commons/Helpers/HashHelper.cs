using System.Security.Cryptography;
using System.Text;

namespace Commons.Helpers;

public static class HashHelper
{
    public static string ComputeMd5Hash(string input)
    {
        using MD5 mD = MD5.Create();
        return ToHashString(mD.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }

    public static string ComputeMd5Hash(Stream input)
    {
        using MD5 mD = MD5.Create();
        return ToHashString(mD.ComputeHash(input));
    }

    public static string ComputeSha256Hash(Stream stream)
    {
        using SHA256 sHA = SHA256.Create();
        return ToHashString(sHA.ComputeHash(stream));
    }

    public static string ComputeSha256Hash(string input)
    {
        using SHA256 sHA = SHA256.Create();
        return ToHashString(sHA.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }

    private static string ToHashString(byte[] bytes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            stringBuilder.Append(bytes[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}