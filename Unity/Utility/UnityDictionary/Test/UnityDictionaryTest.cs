using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityDictionaryTest : MonoBehaviour
{
    public UnityDictionary<string, string[]> dict = new UnityDictionary<string, string[]>();
    void Start()
    {
        dict.Add("a", new string[] { "aaaa","bbbb" });
        dict.Add("b", new string[] { "aaaa", "bbbb" });
        dict.Remove("a");
        dict["b"][1] = "cccc";
    }
}
