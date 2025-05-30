using System.Collections;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ButtonTest : MonoBehaviour
    {
        public int myInt;

        [NaButton(enabledMode: ENaButtonEnableMode.Always)]
        private void IncrementMyInt()
        {
            myInt++;
        }

        [NaButton("Decrement My Int", ENaButtonEnableMode.Editor)]
        private void DecrementMyInt()
        {
            myInt--;
        }

        [NaButton(enabledMode: ENaButtonEnableMode.Playmode)]
        private void LogMyInt(string prefix = "MyInt = ")
        {
            Debug.Log(prefix + myInt);
        }

        [NaButton("StartCoroutine")]
        private IEnumerator IncrementMyIntCoroutine()
        {
            int seconds = 5;
            for (int i = 0; i < seconds; i++)
            {
                myInt++;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
