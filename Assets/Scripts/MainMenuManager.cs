﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable 0649
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Text highScoreText;

    [SerializeField] private int highscore;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("McBuilderHighscore", 0);
    }

    private void Update()
    {
        highScoreText.text = "HighScore: " + highscore;
    }

    public void ExitGame()
    {
        // add ads here <evil smiley face>
            Application.Quit();
    }

    public void StartGame()
    {
        // add an ad here

            SceneManager.LoadScene("GameScene");
    }

    public void ButtonClick()
    {
        SoundManager.Instance.PlayAudio("buttonclick");
    }
}
#pragma warning restore 0649