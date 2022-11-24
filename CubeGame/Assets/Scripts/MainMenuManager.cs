using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    public Image startGameP;
    public Image levelSelectionP;
    public Image loadingP;
    public Text Score;
    public Image[] levels;
    public Image[] locked;
    public Slider loadingBar;
    public float minLB=0f;
    public float maxLB=1f;
    public static int levelNumber;
    public AudioSource GameBG;
    public AudioSource playButton;
   

    private void Awake()
    {
        GameBG.Play();
    }

    private void Start()
    {

        startGameP?.gameObject.SetActive(true);
        levelSelectionP?.gameObject.SetActive(false);
        Score.text = PlayerPrefs.GetInt("Score").ToString();
        LevelChecker();
    }
    public void InLevelButtons(string str)
    {
        if (str == "play")
        {
            playButton.Play();
            startGameP.gameObject.SetActive(false);
            levelSelectionP.gameObject.SetActive(true);
        }
        if (str == "Quit")
        {
            playButton.Play();
            Application.Quit();
        }
        if (str == "Back")
        {
            playButton.Play();
            startGameP.gameObject.SetActive(true);
            levelSelectionP.gameObject.SetActive(false);

        }
        if (str == "GamePlayLoader")
        {
            playButton.Play();

            StartCoroutine(LoadingDelay());
            
       

        }
    }
    public void LevelSelector(int levelNumb)
    {
        levelNumber = levelNumb;
        InLevelButtons("GamePlayLoader");


    }
    public void LevelChecker()
    {
        for (int i = 0; i <= PlayerPrefs.GetInt("Levels"); i++)
        {
            locked[i].gameObject.SetActive(false);
            levels[i].GetComponent<Button>().interactable = true;
        }
    }
    IEnumerator LoadingDelay()
    {
        loadingP.gameObject.SetActive(true);
        AudioListener.volume = 0;
        yield return new WaitForSeconds(5f);
        AudioListener.volume = 1;
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }
    private void Update()
    {
        if (loadingP.gameObject.activeInHierarchy)
        {                                   
            loadingBar.value = Mathf.Lerp(loadingBar.value, maxLB, 0.8f* Time.deltaTime);

        }
    }
    //IEnumerator SkullDelay()
    //{

    //}
}
