using System.Collections.Generic;
using UnityEngine;
public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        list.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
    }
}