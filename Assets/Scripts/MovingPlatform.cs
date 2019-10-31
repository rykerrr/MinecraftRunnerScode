using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class MovingPlatform : MonoBehaviour
{
    public bool Active { get; set; }
    public float platSpeed;
    [SerializeField] private int minXLimit = 2;

    Rigidbody2D myRb;
    Vector3 startVector;

    private void Start()
    {
        Active = true;
        startVector = transform.position;
        myRb = GetComponent<Rigidbody2D>();

        int ranDir = Random.Range(0, 1);

        if (ranDir == 0)
        {
            myRb.velocity = Vector2.right * platSpeed * Time.deltaTime;
        }
        else if (ranDir == 1)
        {
            myRb.velocity = -Vector2.right * platSpeed * Time.deltaTime;
        }
    }

    private void Update()
    {
        if (Active)
        {
            if (transform.position.x > startVector.x + minXLimit)
            {
                myRb.velocity = -Vector2.right * platSpeed * Time.deltaTime;
            }
            else if (transform.position.x < startVector.x - minXLimit)
            {
                myRb.velocity = Vector2.right * platSpeed * Time.deltaTime;
            }
        }

        if (!Active)
        {
            StopMoving();
        }
    }

    public void StopMoving()
    {
        if (myRb)
        {
            myRb.velocity = Vector2.zero;
            Destroy(myRb);
        }
    }
}
#pragma warning restore 0649