using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649
public class RoguePlatformManager : MonoBehaviour
{
    #region Singleton
    private static RoguePlatformManager instance;
    private static object m_lock = new object();
    private static bool shuttingDown = false;
    public static RoguePlatformManager Instance
    {
        get
        {
            lock (m_lock)
            {
                if (shuttingDown)
                {
                    return null;
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<RoguePlatformManager>();

                    if (instance == null)
                    {
                        return null;
                    }
                }

                return instance;
            }
        }

        private set
        {
            instance = value;
        }
    }
    #endregion

    public bool GameStarted { get; private set; } = false;
    public int Score { get; set; }
    public int TimeLeft { get; private set; }
    public int HighScore { get; private set; }

    [SerializeField] private Transform[] platformPrefabs;
    [SerializeField] private Transform zombiePrefab;
    [SerializeField] private Transform skelePrefab;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private PlayerController plrContrll;
    [SerializeField] private ColorTransitionManager colorManager;

    [SerializeField] private float yLimit;
    [SerializeField] private float offsetMin = 1f;
    [SerializeField] private float offsetMax = 5f;
    [SerializeField] private int roundTime = 60;

    private float timer;

    private void Start()
    {
        TimeLeft = roundTime;
        timer = Time.time + 1;
    }

    public void Update()
    {
        if (plrContrll)
        {
            if (plrContrll.transform.position.y <= yLimit)
            {
                if (GameStarted)
                {
                    SoundManager.Instance.PlayAudio("zombdeath");
                    SoundManager.Instance.PlayAudio("falloffworld");
                    GameOver();
                }
            }

            if (GameStarted)
            {
                if (!plrContrll.IsMoving)
                {
                    if (Time.time > timer)
                    {
                        if (TimeLeft <= 0)
                        {
                            GameOver();
                        }

                        TimeLeft--;
                        timer = Time.time + 1;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void GameOver()
    {
        SoundManager.Instance.StopAudio("gamethemesong");
        SetHighscoreIfPossible();
        Poolable.Clear();
        endGameScreen.SetActive(true);
        GameStarted = false;
    }

    public void CreatePlatform()
    {
        int pref = Random.Range(0, platformPrefabs.Length);

        if (pref == 1)
        {
            pref = Random.Range(0, platformPrefabs.Length);
        }

        float offset = Random.Range(offsetMin, offsetMax);
        Vector3 newPos = new Vector3(plrContrll.plats[plrContrll.plats.Count - 1].position.x + offset, 0f, 0f);
        Transform newPlat = Instantiate(platformPrefabs[pref], new Vector3(Random.Range(newPos.x - 2f, newPos.x + 6f), newPos.y, newPos.z), Quaternion.identity);

        newPlat.localScale = new Vector3(Random.Range(platformPrefabs[pref].localScale.x / 1.4f, platformPrefabs[pref].localScale.x * 1.2f), platformPrefabs[pref].localScale.y, platformPrefabs[pref].localScale.z);

        if (newPlat.GetComponent<MovingPlatform>())
        {
            newPlat.GetComponent<MovingPlatform>().platSpeed = Random.Range(30, 60);
        }
        else
        {
            int zombChance = Random.Range(0, 60);

            if (zombChance > 40)
            {
                Transform newZomb = Instantiate(zombiePrefab, new Vector3(Random.Range(newPlat.transform.position.x - newPlat.transform.localScale.x / 2.2f, newPlat.transform.position.x + newPlat.transform.localScale.x / 2.2f), 1.4f, 0f)
                        , Quaternion.identity);
            }
            else
            {
                int skeleChance = Random.Range(0, 60);

                if (skeleChance < 46)
                {
                    Transform newSkele = Instantiate(skelePrefab, new Vector3(Random.Range(newPlat.transform.position.x - newPlat.transform.localScale.x / 2.2f, newPlat.transform.position.x + newPlat.transform.localScale.x / 2.2f), 1.4f, 0f)
                        , Quaternion.identity);
                }
            }
        }



        plrContrll.AddPlatform(newPlat);
        colorManager.AddPlatform(newPlat.GetComponent<SpriteRenderer>());
        TimeLeft = roundTime;
        timer = Time.time + 1;
        Score++;
    }

    public void StartGame()
    {
        SoundManager.Instance.PlayAudio("gamethemesong");
        startGameScreen.SetActive(false);
        GameStarted = true;
    }

    public void ReturnToMenu()
    {
        if (AdsManager.Instance.ShowRewardedAd((sr) => { SceneManager.LoadScene("MainMenu"); SetHighscoreIfPossible(); }) == false)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RestartGame()
    {
        if (AdsManager.Instance.ShowRegularAd((sr) => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); SetHighscoreIfPossible(); }) == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnApplicationQuit()
    {
        SetHighscoreIfPossible();
    }

    private void SetHighscoreIfPossible()
    {
        HighScore = Score > PlayerPrefs.GetInt("McBuilderHighscore") ? Score : PlayerPrefs.GetInt("McBuilderHighscore");
        PlayerPrefs.SetInt("McBuilderHighscore", HighScore);
    }
}
#pragma warning disable 0649    
