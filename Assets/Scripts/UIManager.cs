using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class UIManager : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text endScoreText;
    [SerializeField] private Text timeLeft;
    [SerializeField] private Text highScore;
    [SerializeField] private Image speakerImg;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Sprite[] speakerSprite;

    [SerializeField] private float roundTime;

    bool mute = false;

    private void Update()
    {
        score.text = "Score: " + RoguePlatformManager.Instance.Score;
        timeLeft.text = "Time left: " + RoguePlatformManager.Instance.TimeLeft;

        if (endPanel.activeSelf == true)
        {
            highScore.text = "High score: " + RoguePlatformManager.Instance.HighScore;
            endScoreText.text = "Game Over\n" + score.text;
        }
    }

    public void MuteThemeSong()
    {
        if(!mute)
        {
            SoundManager.Instance.StopAudio("gamethemesong");
            speakerImg.sprite = speakerSprite[1];
            mute = true;
        }
        else if(mute)
        {
            SoundManager.Instance.PlayAudio("gamethemesong");
            speakerImg.sprite = speakerSprite[0];
            mute = false;
        }
    }

    public void ButtonClick()
    {
        SoundManager.Instance.PlayAudio("buttonclick");
    }
}
#pragma warning restore 0649