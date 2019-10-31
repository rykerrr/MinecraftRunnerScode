using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class Skeleton : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform arrowPrefab;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Slider fireLoadBar;
    [SerializeField] private float engageDistance;
    [SerializeField] private float fireDelay;
    [SerializeField] private float arrowSpeed;

    private float fireTimer;
    private float checkTimer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            fireLoadBar.value = fireTimer - Time.time;
            if(Time.time > fireTimer)
            {
                Fire();
            }
        }
        else
        {
            if(Time.time > checkTimer)
            {
                CheckForPlayer();
            }
        }

    }

    private void CheckForPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, engageDistance, whatIsPlayer);

        if (hit)
        {
            if(hit.transform)
            {
                target = hit.transform;
                fireTimer = Time.time + fireDelay;
            }
        }

        checkTimer = Time.time + 3f;
    }

    private void Fire()
    {
        SoundManager.Instance.PlayAudio("skelefire");
        fireTimer = fireDelay + Time.time;
        Poolable arrow = Poolable.Get(() => Poolable.CreateObj<Arrow>(arrowPrefab.gameObject));
        arrow.transform.position = firePos.position;
        arrow.GetComponent<Rigidbody2D>().velocity = -Vector2.right * arrowSpeed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, engageDistance);
    }
}
#pragma warning restore 0649    