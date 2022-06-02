using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class BridgeController : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsArrow;
    [SerializeField] private LayerMask groundCheckLayermask;
    [SerializeField] private LayerMask platMask;
    [SerializeField] private PlayerController plrController;
    [SerializeField] private Transform bridge;
    [SerializeField] private Transform bridgeEnd;
    [SerializeField] private GameObject ignorObj;
    [SerializeField] private float bridgeFallSmoothTime = 3f;

    private bool wasButtonHeld = false;
    private bool rotating = false;

    private float smDampVelocity;
    private Vector3 startScale;
    private Vector3 startPos;

    private float groundchecktimer;
    private float bridgeDelayTimer;

    private void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
        GroundCheck();
    }

    private void Update()
    {
        if (RoguePlatformManager.Instance.GameStarted)
        {
            //if(Time.time > groundchecktimer)
            //{
            //    GroundCheck();
            //    groundchecktimer = Time.time + 3f;
            //}

            if (!Input.GetMouseButton(0) || rotating) // when released, drop the platform
            {
                if (wasButtonHeld)
                {
                    Collider2D[] arrows = Physics2D.OverlapCircleAll(bridgeEnd.position, 2f, whatIsArrow);

                    if(arrows.Length > 0)
                    {
                        foreach(Collider2D arrow in arrows)
                        {
                            Destroy(arrow);
                        }
                    }

                    Collider2D checkForMovPlat = Physics2D.OverlapCircle(bridgeEnd.position, 0.4f, platMask);
                    rotating = true;
                    Vector3 rotVector = new Vector3(0f, 0f, Mathf.SmoothDamp(bridge.parent.localEulerAngles.z, -90f, ref smDampVelocity, bridgeFallSmoothTime));
                    bridge.parent.localEulerAngles = rotVector;

                    if(checkForMovPlat)
                    {
                        if(checkForMovPlat.GetComponent<MovingPlatform>())
                        {
                            MovingPlatform plat = checkForMovPlat.GetComponent<MovingPlatform>();
                            plat.Active = false;
                            plat.StopMoving();
                            plat.enabled = false;
                        }
                    }

                    if (bridge.parent.eulerAngles.z <= 266 && bridge.parent.eulerAngles.z >= 230)
                    {
                        rotating = false;
                        BridgeDropped();
                    }
                }
            }

            if (Input.GetMouseButton(0)) // Input.GetMosueButton(0) works with touches
            {
                if(plrController.CanStop == true)
                {
                    if(Time.time > bridgeDelayTimer)
                    {
                        plrController.StopMoving();
                    }
                }

                if (plrController.IsMoving || rotating)
                {
                    return;
                }

                if(Time.time > bridgeDelayTimer)
                {
                    Expand();
                }
            }
        }
    }

    private void Expand()
    {
        Vector3 addVector = new Vector3(0f, (Mathf.Log(RoguePlatformManager.Instance.Score + 3, 2) / 100f) * 2.6f, 0f); // 0.09f currently
        bridgeEnd.localPosition = new Vector3(0f, (bridge.localScale.y + addVector.y), 0f);
        bridge.localScale += addVector;
        bridge.localPosition += addVector / 2;
        wasButtonHeld = true;
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(bridge.position - Vector3.up, -Vector2.up, 5f, groundCheckLayermask);

        if (hit.collider != null)
        {
            if(!hit.collider.GetComponent<Skeleton>() && !hit.collider.GetComponent<Zombie>())
            {
                ignorObj = hit.collider.gameObject;
            }
  
        }
    }

    private void BridgeDropped()
    {
        wasButtonHeld = false;
        Transform newBrDebris = Instantiate(bridge, new Vector3(bridge.position.x, 2.3f, 1f), Quaternion.Euler(new Vector3(0f, 0f, -90f)));
        newBrDebris.GetComponent<Collider2D>().isTrigger = false;

        Destroy(newBrDebris.GetComponent<BridgeController>());
        Collider2D hit = Physics2D.OverlapCircle(bridgeEnd.position, 0.5f, groundCheckLayermask);
   
        GroundCheck();
        plrController.MoveToNextPos(bridge.transform.localScale, bridgeEnd.position, hit ? hit.transform ? hit.transform : null : null);

        bridge.localScale = startScale;
        bridge.parent.eulerAngles = Vector3.zero;
        bridge.localPosition = startPos;

        if (hit)
        {
            if (!hit.GetComponent<Skeleton>() && !hit.GetComponent<Zombie>())
            {
                if(hit.gameObject != ignorObj)
                {
                    RoguePlatformManager.Instance.CreatePlatform();
                }
            }
            else if(hit.GetComponent<MovingPlatform>())
            {
                MovingPlatform plat = hit.GetComponent<MovingPlatform>();
                plat.Active = false;
                plat.StopMoving();
                plat.enabled = false;
            }
        }

        GroundCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rotating)
        {
            if (collision.GetComponent<Zombie>() || collision.GetComponent<Skeleton>())
            {
                SoundManager.Instance.PlayAudio("zombierekt");
                Destroy(collision.gameObject);
                RoguePlatformManager.Instance.Score += 2;
            }
            else
            {
                if (collision.GetComponent<MovingPlatform>())
                {
                    MovingPlatform plat = collision.GetComponent<MovingPlatform>();
                    plat.Active = false;
                    plat.StopMoving();
                    plat.enabled = false;
                }

                if (ignorObj != collision.gameObject)
                {
                    plrController.MoveToNextPos(bridge.transform.localScale, bridgeEnd.transform.position);
                }
            }
        }
    }

    public void WaitBeforeBridge()
    {
        bridgeDelayTimer = Time.time + 0.35f;
        return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bridgeEnd.position, 0.3f);
    }

}
#pragma warning disable 0649