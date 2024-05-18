using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOSPanel : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private Button tosBtn, acceptBtn;

    private void Awake()
    {
        tosBtn.onClick.AddListener(OpenURL);
        acceptBtn.onClick.AddListener(Accept);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Accept()
    {
        this.Hide();
        SoundManager.instance.PlayOneShot(SFX.Btn_Click);
        PlayerPrefs.SetInt("TOS", 1);
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenURL()
    {
        Application.OpenURL(url);
    }
}
