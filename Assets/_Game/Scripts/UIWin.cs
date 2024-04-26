using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    public static UIWin instance;
    [SerializeField] private Button replayBtn, quitBtn;
    [SerializeField] private TMP_Text scoreText, highscoreText;
    
    private void Awake(){
        instance = this;
        replayBtn.onClick.AddListener(ReplayClick);
        quitBtn.onClick.AddListener(QuitClick);
        this.gameObject.SetActive(false);
    }

    public void Show(){
        this.gameObject.SetActive(true);
        scoreText.text = $"YOUR SCORE: {ScoreManager.instance.Score.ToString()}";
        highscoreText.text = $"HIGH SCORE: {PlayerPrefs.GetInt("HighScore", 0).ToString()}";
    }

    private void ReplayClick(){
        SoundManager.instance.PlayOneShot(SFX.Btn_Click);
        SceneManager.UnloadSceneAsync("Game").completed += (e) => {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        };
    }
    private void QuitClick(){
        SoundManager.instance.PlayOneShot(SFX.Btn_Click);
        SceneManager.UnloadSceneAsync("Game").completed += (e) => {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        };
    }
}
