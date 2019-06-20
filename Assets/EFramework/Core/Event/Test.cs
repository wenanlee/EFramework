using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start () {
        EFramework.Event.Signal<string> signal = new EFramework.Event.Signal<string>();
        signal.AddListener(aaaa);
        signal.Invoke("fack");
	}

    private void aaaa(string t)
    {
        print(t);
    }
}
