using EFramework.Unity.UnityCommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace EFramework.Unity.Example
{
    /// <summary>
    /// ≤‚ ‘√¸¡Ó––
    /// </summary>
    public class Example : MonoBehaviour
    {
        public bool isRotate;
        public float speed = 1;
        private void Start()
        {

        }
        private void Update()
        {
            if (isRotate)
                transform.Rotate(0, speed, 0);
        }
        [RegisterCommandLine(Name = "fuck", Help = "Cube rotate state")]
        public void SetRotateState(string state)
        {
            Debug.LogError((state == "start"));
            isRotate = (state == "start");
        }
        [RegisterCommandLine(Name = "SetSpeed", Help = "Set cube rotate speed")]
        public void SetRotateSpeed(float speed)
        {
            this.speed = speed;
        }
    }

}
