using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource gameplayBGM;
    public AudioSource buttonAudioSource;
    public AudioSource gameAudio;
    public AudioClip ButtonSound;
    public AudioClip coinSound;
    public AudioClip deathSound;
    public AudioClip gameBG;
    public AudioClip levelFinishAudio;
    public AudioClip countDown;
    public AudioClip devilSlash;

    public static GameManager instance;

    [Header("Panels")]
    public Image touchControlsP;
    public Image pauseScreen;
    public Image levelFinish;
    public Image deathP;
    public Image timerP;
    //public Image finishtxtP;

    [Header("Level Progress Bar")]
    public Image fillBar;
    public Text currentLevel;
    public Text nextLevel;

    public Transform playerTransform;
    public Transform endlineTransform;

    private Vector3 endlinePos;

    private float fullDistance;
    
    
    public ParticleSystem fireParticle;
    //public ParticleSystem trailParticle;
    public Text coins;
    public Text timerText;
    public SpriteRenderer plusOne;


    public GameObject[] levels;


    public bool moveright, moveleft, moveforward;
    public int coinadd;
    public int countdownTime;
    
    
    void Awake()
    {
        instance = this;
        gameplayBGM.PlayOneShot(gameBG);
    }


    void Start()
    {
        endlinePos = endlineTransform.position;
        fullDistance = GetDistance();
        SetLevelTexts((MainMenuManager.levelNumber+1));
        fireParticle.gameObject.SetActive(false);
        //trailParticle.gameObject.SetActive(false);
        levelFinish.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        deathP.gameObject.SetActive(false);
        timerP.gameObject.SetActive(true);
        //finishtxtP.gameObject.SetActive(true);
        levels[MainMenuManager.levelNumber].SetActive(true);
        StartCoroutine(CountDownSequence());
        //gameAudio.PlayDelayed(0.8f);
        gameAudio.PlayOneShot(countDown);


    }
    void SetLevelTexts(int level)
    {
        currentLevel.text = level.ToString();
        nextLevel.text = (level + 1).ToString();
    }

    private float GetDistance()
    {
        //return Vector3.Distance(playerTransform.position,endlinePos);
        return (endlinePos - playerTransform.position).sqrMagnitude;
    }

    void UpdateFillBar(float value)
    {
        fillBar.fillAmount = value;
    }
    private void Update()
    {
        if (playerTransform.position.z <= endlinePos.z)
        {
            float newDistance = GetDistance();
            float progressValue = Mathf.InverseLerp(fullDistance, 0f, newDistance);
            UpdateFillBar(progressValue);
        }
    }
    IEnumerator CountDownSequence()
    {
        while (countdownTime > 0)
        {
            timerText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        timerText.text = "GO!";
        yield return new WaitForSeconds(1f);
        timerP.gameObject.SetActive(false);
        //trailParticle.gameObject.SetActive(true);
    }

    public void CoinCounter()
    {
        coinadd++;
        coins.text = "x " + coinadd.ToString();
       
        
    }
    public void InLevelButtons(string str)
    {
        // if (str == "right")
        //{
        //    moveright = true;
        //    moveleft = false;
        //}
        //else if (str == "left")
        //{
        //    moveleft = true;
        //    moveright = false;
        //}
        //if (str == "stopR")
        //{
        //    moveright = false;
        //}
        //else if (str == "stopL")
        //{
        //    moveleft = false;
        //}
        if (str == "Pause")
        {
            buttonAudioSource.PlayOneShot(ButtonSound);
            Time.timeScale = 0;
            touchControlsP.gameObject.SetActive(false);
            pauseScreen.gameObject.SetActive(true);
            AudioListener.volume = 0;
        }
        else if (str == "Resume")
        {
            buttonAudioSource.PlayOneShot(ButtonSound);
            //  nextLevel.Play();
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
            touchControlsP.gameObject.SetActive(true);
            AudioListener.volume=1;
        } 
        else if (str == "Home")
        {
            //  nextLevel.Play();
            buttonAudioSource.PlayOneShot(ButtonSound);
            Time.timeScale = 1;
            AudioListener.volume = 1;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else if (str == "nextL")
        {
            //  nextLevel.Play();
            buttonAudioSource.PlayOneShot(ButtonSound);
            MainMenuManager.levelNumber += 1;
            levelFinish.gameObject.SetActive(false);
            touchControlsP.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Levels", MainMenuManager.levelNumber);
            
            if (MainMenuManager.levelNumber > 4)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            else
                SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);

        }
        else if (str == "Retry")
        {
            Time.timeScale = 1f;
            buttonAudioSource.PlayOneShot(ButtonSound);
            deathP.gameObject.SetActive(false);
            touchControlsP.gameObject.SetActive(true);
            SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
            
        }


    }
}
