using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEnterTemp : MonoBehaviour, IPointerClickHandler
{
    private void Awake()
    {
        gameObject.SetActive(true);
        Debug.Log(gameObject.activeInHierarchy);
        Debug.Log(gameObject.activeSelf);
    }

    public void OnPointerClick(PointerEventData data)
    {
        // This will only execute if the objects collider was the first hit by the click's raycast
        gameObject.GetComponent<Animator>().SetBool("startGame", true);
    }

    public void EndEvent()
    {
        gameObject.SetActive(false);
        RoguePlatformManager.Instance.StartGame();
    }
}
