using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (target)
        {
            if (target.position.x + 5.5f > transform.position.x)
            {
                transform.position = new Vector3(target.position.x + 5.5f, transform.position.y, transform.position.z);
            }
        }
    }
}
#pragma warning restore 0649