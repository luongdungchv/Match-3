using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScaler : MonoBehaviour
{
    [SerializeField] private Canvas refCanvas;
    [SerializeField] private Camera mainCam;

    private void Start() {
        var size = refCanvas.GetComponent<RectTransform>().sizeDelta;
        var ratio = size.x / 1080;
        mainCam.orthographicSize = 5 / ratio;
    }
}
