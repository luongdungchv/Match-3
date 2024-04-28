using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundManager : SerializedMonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private Dictionary<SFX, AudioClip> audioList;

    [SerializeField] private AudioClip bgm;

    private void Awake() {
        instance = this;
    }

    public void PlayOneShot(SFX sfx){
        soundPlayer.PlayOneShot(audioList[sfx]);
    }
}
public enum SFX{
    Btn_Click, Score, Switch
}
