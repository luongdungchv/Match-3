using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask mask;
    private BoardView boardView;
    [SerializeField] private Session currentSession;
    [SerializeField] private BoardCell chosenCell;
    [SerializeField] private GameObject marker;

    private void Awake()
    {
        this.boardView = GetComponent<BoardView>();
    }


    private void Update()
    {
        if (Input.GetMouseButton(0) && !boardView.IsPlayingAnimation)
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000, mask))
            {
                var hitCell = hitInfo.collider.gameObject.GetComponent<BoardCell>();
                if (currentSession == Session.Active)
                {
                    var hitCellCoord = hitCell.CoordinateVector;
                    var chosenCellCoord = chosenCell.CoordinateVector;
                    var dir = hitCellCoord - chosenCellCoord;
                    if (dir == Vector2Int.up || dir == Vector2Int.down || dir == Vector2Int.left || dir == Vector2Int.right)
                    {
                        boardView.SwitchCells(chosenCell.Coordinate, hitCell.Coordinate);
                        this.currentSession = Session.Terminated;
                        this.chosenCell = null;
                        marker.SetActive(false);
                    }
                    else if(dir == Vector2Int.zero)
                    {
                        //chosenCell = null;
                        currentSession = Session.Active;
                        
                    }
                    else{
                        chosenCell = null;
                        currentSession = Session.Terminated;
                        marker.SetActive(false);
                    }
                }
                else if (currentSession == Session.Ready)
                {
                    currentSession = Session.Active;
                    chosenCell = hitCell;
                    marker.transform.position = hitCell.transform.position;
                    marker.SetActive(true);
                }
            }
        }
        if (currentSession == Session.Terminated && Input.GetMouseButtonUp(0))
        {
            currentSession = Session.Ready;
        }
    }
    public enum Session
    {
        Ready, Active, Terminated
    }
}
