using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class InspectorButtonAttribute : Attribute
{
    public string DisplayName { get; }

    public InspectorButtonAttribute(string displayName = null)
    {
        DisplayName = displayName;
    }
}