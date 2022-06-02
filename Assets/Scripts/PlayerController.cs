using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class PlayerController : MonoBehaviour
{
    public int CurrentPlatform { get; private set; }
    public bool IsMoving { get; private set; }
    public bool CanStop { get; private set; }

    [SerializeField] private BridgeController bridgeController;
    [SerializeField] private List<Transform> platforms = new List<Transform>();
    [SerializeField] private Transform currentPlatform;
    [SerializeField] private Animator anim;
    [SerializeField] private Vector3 platOffset = new Vector2(0f, 0.3f);
    [SerializeField] private Vector3 newPos;

    [SerializeField] private float speedMult;

    public List<Transform> plats => platforms;

    private Vector3 smDampVelocity;
    private Rigidbody2D myRb;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        anim.SetBool("isMoving", false);
    }

    private void FixedUpdate()
    {
        if (RoguePlatformManager.Instance.GameStarted)
        {
            if (IsMoving)
            {
                if(CanStop)
                {
                    return;
                }

                myRb.velocity = Vector2.right * speedMult * Time.deltaTime;
                anim.SetBool("isMoving", true);

                if (transform.position.x >= newPos.x + 0.5f)
                {
                    anim.SetBool("isMoving", false);
                    myRb.velocity = new Vector2(0, myRb.velocity.y);
                    CanStop = true;
                    IsMoving = false;
                    bridgeController.WaitBeforeBridge();
                }
            }
        }
    }

    public void MoveToNextPos(Vector3 bridgeSize, Vector3 bridgeEnd, Transform newPlat = null)
    {
        newPos = (bridgeEnd + platOffset);


        if (newPlat)
        {
            CurrentPlatform++;
        }

        IsMoving = true;
    }

    public void StopMoving()
    {
        IsMoving = false;
        CanStop = false;
    }

    public void AddPlatform(Transform platform)
    {
        platforms.Add(platform);

    }
}
#pragma warning restore 0649