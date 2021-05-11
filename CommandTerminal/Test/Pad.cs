using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CommandTerminal.Terminal.Shell.RunCommand("car 1");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            foreach (var item in CommandTerminal.Terminal.Shell.Commands)
            {
                CommandTerminal.Terminal.Log(item.Key);
            }
        }
        //help 
        //help car (输出"Hint")
    }
}
