using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetStartSeed : MonoBehaviour
{
    public static string seed;
    void Start()
    {
        seed = AddToSeed(DateTime.Now.Ticks.ToString());
        GetComponent<TMP_InputField>().text = Format(seed);
    }


    public void DeFormatSeed()
    {
        GetComponent<TMP_InputField>().text = seed.ToString();
    }
    public void FormatSeed()
    {
        seed = GetComponent<TMP_InputField>().text;
        GetComponent<TMP_InputField>().text = Format(seed);
    }

    public string AddToSeed(string str)
    {
        while (str.Length < 20)
        {
            str += UnityEngine.Random.Range(0, 10);
        }
        return str;
    }
    public string Format(string str)
    {
        str = AddToSeed(str);
        seed = str;
        int start = 0;
        var final = "";
        while (start <= 16)
        {
            final += str.Substring(start, 4) + ((start + 4 <= 16) ? ":" : "");
            start += 4;
        }
        return final;
    }
}