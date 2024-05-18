using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class PileCoinAnimation : MonoBehaviour
{
    [SerializeField] private Transform coinContainer;
    [SerializeField] private Transform coinTarget;
    [SerializeField] private float interval, moveDuration, scaleDuration;
        
    private List<Vector3> originalPosList;
    private int targetCoinsVal;
    
    private void Awake() {
        originalPosList = new List<Vector3>();
        for(int i = 0; i < coinContainer.childCount; i++){
            originalPosList.Add(coinContainer.GetChild(i).position);
        }
    }
    [Sirenix.OdinInspector.Button]
    public void PlayAnimation(UnityAction onComplete = null)
    {
        coinContainer.gameObject.SetActive(true);
        DL.Utils.CoroutineUtils.SetInterval(this, Move, () =>
        {
            DL.Utils.CoroutineUtils.Invoke(this, () => {
                coinContainer.gameObject.SetActive(false);
                onComplete?.Invoke();
            }, moveDuration + scaleDuration);
        },interval, coinContainer.childCount);
        
    }
    private void Move(int i)
    {
        if(i >= coinContainer.childCount)
            return;
        var sequence = DOTween.Sequence();
        var coin = coinContainer.GetChild(i);
        coin.transform.position = originalPosList[i];
        coin.DORewind();
        sequence.Append(coin.DOScale(1, scaleDuration));
        sequence.Append(
            coin.GetComponent<RectTransform>()
            .DOMove(coinTarget.transform.position, moveDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                coin.localScale = Vector3.zero;
                
                SoundManager.instance.PlayOneShot(SFX.CoinCollect);
            })
        );
        SoundManager.instance.PlayOneShot(SFX.CoinAppear);
    }
}
