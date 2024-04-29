using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterManager : MonoBehaviour
{
    public static MasterManager Instance;
    [SerializeField] private TOSPanel tosPanel;
    private void Awake() {
        if(Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        if(PlayerPrefs.GetInt("TOS", 0) == 0){
            tosPanel.Show();
        }
        
    }
}
