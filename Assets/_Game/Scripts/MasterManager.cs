using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterManager : MonoBehaviour
{
    [SerializeField] private TOSPanel tosPanel;
    private void Awake() {
        if(PlayerPrefs.GetInt("TOS", 0) == 0){
            tosPanel.Show();
        }
        else
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
}
