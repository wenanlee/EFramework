using EFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    [NaughtyAttributes.Dropdown("events")]
    public string eventStr;
    public string[] events { get => EventsCenter.Instance.EventLst; set => EventsCenter.Instance.EventLst = value; }

    private void Start()
    {
        EventManager.AddListener("¸É", (int id) => { Debug.Log("¸É " + id); });
    }
}
