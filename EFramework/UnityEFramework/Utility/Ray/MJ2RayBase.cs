using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MJ2RayBase : MonoBehaviour
{
    public bool IsShow;
    public float distance = 5;
    public float duration = 0.1f;
    public LayerMask layerMask;
    private Ray ray;
    private void Start()
    {
        StartCoroutine(CheckOverlapItem());
    }
    IEnumerator CheckOverlapItem()
    {
        RaycastHit hit;
        while (true)
        {
            yield return new WaitForSeconds(duration);
            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, distance,layerMask.value))
            {
                RayTrigger(hit,true);
            }
            else
            {
                RayTrigger(hit,false);
            }
        }
    }

    public virtual void RayTrigger(RaycastHit hit,bool state)
    {

    }

    private void OnDrawGizmos()
    {
        if (IsShow)
        {
            Gizmos.color = new Color(1, 0, 0, 1f);
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction);
        }
    }
}
