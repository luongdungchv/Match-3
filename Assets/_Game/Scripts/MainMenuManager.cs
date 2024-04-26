using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame(){
        SoundManager.instance.PlayOneShot(SFX.Btn_Click);
        SceneManager.UnloadSceneAsync("MainMenu").completed += (e) => {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        };
        
    }
}
