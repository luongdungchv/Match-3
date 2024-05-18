using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundManager : SerializedMonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private Dictionary<SFX, AudioClip> audioList;


    private void Awake() {
        if(instance != null){
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayOneShot(SFX sfx){
        soundPlayer.PlayOneShot(audioList[sfx]);
    }
    [Sirenix.OdinInspector.Button]
    private void TestSound(SFX sfx){
        PlayOneShot(sfx);
    }
}
public enum SFX{
    Btn_Click, Score, Switch, CoinAppear, CoinCollect
}
