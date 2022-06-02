using System;
using UnityEngine;

[Serializable]
#pragma warning disable 0649
public class LavaPlatformController
{
    [SerializeField] private Transform lavaPlatform;
    [SerializeField] private Transform plr;

    private float startY;
    private Vector3 refVeloc;
    private Vector3 refVeloc2;
    float floatveloc1;
    float floatveloc2;

    public void Init()
    {
        startY = lavaPlatform.position.y;
    }

    public void Tick()
    {
        lavaPlatform.position = new Vector3(plr.position.x, Mathf.SmoothDamp(lavaPlatform.position.y, 2.52f, ref floatveloc1, 6f), lavaPlatform.position.z);
        //lavaPlatform.position = Vector3.SmoothDamp(lavaPlatform.position, new Vector3(plr.position.x, 0.82f, plr.position.z), ref refVeloc, 5f);
    }

    public void ReverseTick()
    {
        //lavaPlatform.position = Vector3.SmoothDamp(lavaPlatform.position, new Vector3(plr.position.x, startY, plr.position.z), ref refVeloc2, 0.5f);
        lavaPlatform.position = new Vector3(plr.position.x, Mathf.SmoothDamp(lavaPlatform.position.y, startY, ref floatveloc1, 0.2f), lavaPlatform.position.z);
    }
}
#pragma warning restore 0649