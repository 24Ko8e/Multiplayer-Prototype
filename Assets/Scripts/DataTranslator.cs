using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour
{
    static string KILLS_SYMBOL = "[KILLS]";
    static string DEATHS_SYMBOL = "[DEATHS]";

    public static int retrieveKillCount(string data)
    {
        return int.Parse(decodeData(data, KILLS_SYMBOL));
    }
    public static int retrieveDeathCount(string data)
    {
        return int.Parse(decodeData(data, DEATHS_SYMBOL));
    }

    static string decodeData(string  data, string symbol)
    {
        string[] stringParts = data.Split('/');
        foreach (string part in stringParts)
        {
            if (part.StartsWith(symbol))
            {
                return part.Substring(symbol.Length);
            }
        }

        Debug.LogError(symbol + " not found in " + data);
        return "";
    }

    public static string encodeToData(int k, int d)
    {
        return KILLS_SYMBOL + k + "/" + DEATHS_SYMBOL + d;
    }
}
