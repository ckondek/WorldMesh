using UnityEngine;

public class Utils
{
    public static string RandomString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string generatedString = "";
        for (int i = 0; i < length; i += 1)
        {
            generatedString += chars[Random.Range(0, chars.Length - 1)];
        }
        return generatedString;
    }
}
