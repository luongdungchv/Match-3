using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BoardCell : SerializedMonoBehaviour
{
    [SerializeField] private CellDataSO cellData;
    private BoardView boardView;
    private int value;
    [SerializeField] private (int, int) coordinate;
    private Vector3 originalScale;

    public int Value => this.value;

    public (int, int) Coordinate => this.coordinate;
    private void Awake(){
        this.originalScale = this.transform.localScale;
    }

    public void ResetScale(){
        this.transform.localScale = this.originalScale;
    }
    public Tween PlayDisappearAnimation(float duration, UnityAction onComplete = null){
        return this.transform.DOScale(Vector3.zero, duration).OnComplete(() => onComplete?.Invoke());
    }

    public void SetValue(int value){
        this.value = value;
        this.GetComponent<SpriteRenderer>().sprite = cellData.GetSprite(value);
    }
    public void SetCoordinate(int x, int y){
        this.coordinate = (x, y);
    }
    public void SetBoardView(BoardView boardView){
        this.boardView = boardView;
    }
    private Vector3 lastMousePos;
    private void OnMouseDrag() {
        if(lastMousePos != Vector3.zero){
            var delta = Input.mousePosition - lastMousePos;
            if(delta.magnitude > 4){
                if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y)){
                    var (offsetX, offsetY) = (delta.x > 0 ? 1 : -1, 0);
                    var (x, y) = coordinate;
                    var  (targetX, targetY) = (x + offsetX, y + offsetY);
                    boardView.SwitchCells((x, y), (targetX, targetY));
                }
                else{
                    var (offsetX, offsetY) = (0, delta.y > 0 ? 1 : -1);
                    var (x, y) = coordinate;
                    var  (targetX, targetY) = (x + offsetX, y + offsetY);
                    boardView.SwitchCells((x, y), (targetX, targetY));
                }
            }
        }
        lastMousePos = Input.mousePosition;
    }
    private void OnMouseUp() {
        lastMousePos = Vector3.zero;
    }
    private void OnMouseExit() {
        lastMousePos = Vector3.zero;
    }
}
