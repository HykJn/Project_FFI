using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class DataManager
{
    static readonly byte[] KEY = ASCIIEncoding.ASCII.GetBytes("BluSH4G*");

    public static void SaveData(SaveData data)
    {
        string path = Path.Combine(Application.persistentDataPath, "data.dat");
        string json = JsonUtility.ToJson(data, true);

        string encrypted = Encrypt(json);

        File.WriteAllText(path, encrypted);
    }

    public static SaveData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "data.dat");
        if (!File.Exists(path)) throw new Exception();
        string encrypted = File.ReadAllText(path);
        string json = Decrypt(encrypted);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data;
    }
    
    public static bool CheckDataExists()
    {
        string path = Path.Combine(Application.persistentDataPath, "data.dat");
        return File.Exists(path);
    }

    public static bool DeleteData()
    {
        string path = Path.Combine(Application.persistentDataPath, "data.dat");
        if (!File.Exists(path)) return false;
        File.Delete(path);
        return true;
    }

    static string Encrypt(string data)
    {
        DESCryptoServiceProvider des = new();
        des.Mode = CipherMode.ECB;
        des.Padding = PaddingMode.PKCS7;

        des.Key = KEY;
        des.IV = KEY;

        MemoryStream mStream = new();
        CryptoStream cStream = new(mStream, des.CreateEncryptor(), CryptoStreamMode.Write);

        byte[] buffer = Encoding.UTF8.GetBytes(data);

        cStream.Write(buffer, 0, data.Length);
        cStream.FlushFinalBlock();

        return Convert.ToBase64String(mStream.ToArray());
    }

    static string Decrypt(string data)
    {
        DESCryptoServiceProvider des = new();

        des.Mode = CipherMode.ECB;
        des.Padding = PaddingMode.PKCS7;

        des.Key = KEY;
        des.IV = KEY;

        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
        byte[] buffer = Convert.FromBase64String(data);

        cStream.Write(buffer, 0, buffer.Length);
        cStream.FlushFinalBlock();

        return Encoding.UTF8.GetString(mStream.GetBuffer());
    }

}