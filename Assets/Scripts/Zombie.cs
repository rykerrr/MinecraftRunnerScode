using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class Zombie : MonoBehaviour
{
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private float checkRadius;
    [SerializeField] private float checkDelay;
    [SerializeField] private float speedMult;

    private Transform target;
    private Rigidbody2D myRb;

    private float checkTimer;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(RoguePlatformManager.Instance.GameStarted)
        {
            if(Time.time > checkTimer)
            {
                target = CheckForPlayer();
                checkTimer = Time.time + checkDelay;
            }
        }
    }

    private void FixedUpdate()
    {
        if(target)
        {
            myRb.velocity = -Vector2.right * speedMult * Time.fixedDeltaTime;
        }
        else
        {
            myRb.velocity = Vector2.zero;
        }
    }

    private Transform CheckForPlayer()
    {
        Collider2D[] playerCheck = Physics2D.OverlapCircleAll(transform.position, checkRadius, whatToHit);

        if (playerCheck.Length > 0)
        {
            foreach(Collider2D coll in playerCheck)
            {
                if(coll.GetComponent<PlayerController>())
                {
                    return coll.transform;
                }
            }
        }


        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<PlayerController>())
        {
            SoundManager.Instance.PlayAudio("deathbyzombie");
            RoguePlatformManager.Instance.GameOver();
            Destroy(collision.gameObject);
        }
    }
}
#pragma warning restore 0649