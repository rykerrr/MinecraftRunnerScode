using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Poolable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            Destroy(collision.gameObject);
            SoundManager.Instance.PlayAudio("deathbyskele");
            RoguePlatformManager.Instance.GameOver();
            ReturnToPool();
        }
        else
        {
            ReturnToPool();
        }
    }
}
