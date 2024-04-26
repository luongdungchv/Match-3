using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TMP_Text textTime, textScore;
    [SerializeField] private int time;

    private int currentTime, currentScore;

    public int Score => currentScore;

    private Coroutine timeCoroutine;
    private void Awake(){
        if(instance != null) Destroy(instance.gameObject);
        instance = this;
        currentTime = time;
        currentScore = 0;
        textTime.text = "80s";
        textScore.text = "0";
        
    }
    private void Start(){
        StartCountDown();
    }
    private void StartCountDown(){
        timeCoroutine = DL.Utils.CoroutineUtils.SetInterval(this, () => {
            currentTime--;
            textTime.text = currentTime.ToString() + "s";
        },
        () => {
            Timeout();
        },
        1, time);
    }
    public void AddScore(int score){
        currentScore += score;
        this.textScore.text = currentScore.ToString();
    }
    private void Timeout(){
        StopCoroutine(timeCoroutine);
        var highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(currentScore > highScore) PlayerPrefs.SetInt("HighScore", currentScore);
        UIWin.instance.Show();
    }
}
