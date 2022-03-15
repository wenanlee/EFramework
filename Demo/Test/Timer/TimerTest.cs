using EFramework.Unity.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color color = Color.white;
        float transitionDuration = 5f;

        Timer.Register(transitionDuration,
           onUpdate: secondsElapsed => color.r = 255 * (secondsElapsed / transitionDuration),
           onComplete: () => Debug.Log("Color is now red"));

        this.AttachTimer(5f, () => {
            this.gameObject.transform.position = Vector3.zero;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
