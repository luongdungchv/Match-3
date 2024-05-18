using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using DL.Utils;
using TMPro;

public class BoardView : SerializedMonoBehaviour
{
    [SerializeField] private BoardCell[,] boardCells;
    [SerializeField] private BoardNodel model;
    [SerializeField] private Queue<BoardCell> cellPool;
    [SerializeField] private CellDataSO cellData;
    [SerializeField] private TMP_Text notiText;
    [SerializeField] private float switchDuration, disappearDuration, moveDuration;

    [SerializeField] private bool isPlayingAnimation;
    [SerializeField] private float gap;

    public bool IsPlayingAnimation => this.isPlayingAnimation;

    private void Awake()
    {
        this.GenerateLevel();
    }

    private void Start()
    {
        this.GameStart();
    }

    public void GenerateLevel()
    {
        var boardValues = model.GenerateRandomValues(this.boardCells.GetLength(0), this.boardCells.GetLength(1), cellData.MaxCellValue);
        boardCells.Loop((item, x, y) =>
        {
            item.SetValue(boardValues[x, y]);
            item.SetCoordinate(x, y);
            item.SetBoardView(this);
        });
    }
    [Sirenix.OdinInspector.Button]
    private void SetUpPositions()
    {
        boardCells.Loop((item, x, y) =>
        {
            item.transform.position = new Vector2(x, y) * gap;
        });
    }
    [Button]
    public void SwitchCells((int, int) cell1, (int, int) cell2)
    {
        if (isPlayingAnimation) return;
        var (x1, y1) = cell1;
        var (x2, y2) = cell2;
        if (x1 < 0 || x1 >= model.boardWidth || x2 < 0 || x2 >= model.boardWidth || y1 < 0 || y1 >= model.boardHeight || y2 < 0 || y2 >= model.boardHeight) return;

        //SoundManager.instance.PlayOneShot(SFX.Switch);

        var pos1 = boardCells[x1, y1].transform.position;
        var pos2 = boardCells[x2, y2].transform.position;

        var sequence = DOTween.Sequence();
        sequence.Join(boardCells[x2, y2].transform.DOMove(pos1, switchDuration));
        sequence.Join(boardCells[x1, y1].transform.DOMove(pos2, switchDuration));
        this.isPlayingAnimation = true;
        sequence.OnComplete(() =>
        {
            model.SwitchCells(cell1, cell2);
            var result = model.CheckAndGetResult();

            var cache = boardCells[x1, y1];
            boardCells[x1, y1] = boardCells[x2, y2];
            boardCells[x2, y2] = cache;
            boardCells[x1, y1].SetCoordinate(x1, y1);
            boardCells[x2, y2].SetCoordinate(x2, y2);

            if (result.Count > 0)
            {
                var sequence = DOTween.Sequence();
                foreach (var cell in result)
                {
                    var (x, y) = cell;
                    var tween = boardCells[x, y].PlayDisappearAnimation(disappearDuration);
                    cellPool.Enqueue(boardCells[x, y]);
                    sequence.Join(tween);
                }
                sequence.OnComplete(() =>
                {
                    model.RemoveCells(result);
                    var movement = model.GetMovementMatrix(out int[] removedPerColumns);
                    var fillsPerColumns = model.GetRandomFillValues(removedPerColumns, cellData.MaxCellValue);
                    MoveAndFillCell(movement, fillsPerColumns);
                });
                ScoreManager.instance.AddScore(10 * result.Count);

                SoundManager.instance.PlayOneShot(SFX.Score);
            }
            else
            {
                boardCells[x1, y1].transform.DOMove(pos2, switchDuration);
                boardCells[x2, y2].transform.DOMove(pos1, switchDuration).OnComplete(() => isPlayingAnimation = false);

                var cache2 = boardCells[x1, y1];
                boardCells[x1, y1] = boardCells[x2, y2];
                boardCells[x2, y2] = cache2;

                boardCells[x1, y1].SetCoordinate(x1, y1);
                boardCells[x2, y2].SetCoordinate(x2, y2);

                model.SwitchCells(cell1, cell2);
            }
        });
    }

    private void MoveAndFillCell(int[,] movementMatrix, List<int>[] fillsPerColumns)
    {
        var sequence = DOTween.Sequence();
        movementMatrix.Loop((item, x, y) =>
        {
            var targetPos = boardCells[x, y - item].transform.position;
            var cell = boardCells[x, y];
            sequence.Join(cell.transform.DOMove(targetPos, moveDuration));
            boardCells[x, y - item] = cell;
            cell.SetCoordinate(x, y - item);
            isPlayingAnimation = true;

        });
        for (int i = 0; i < fillsPerColumns.Length; i++)
        {
            var fill = fillsPerColumns[i];
            for (int j = 1; j <= fill.Count; j++)
            {
                var fillVal = fill[j - 1];
                var cell = this.cellPool.Dequeue();
                cell.SetValue(fillVal);

                var cellPos = cell.transform.position;
                var y = (model.boardHeight - j) + fill.Count;
                cell.transform.position = new Vector3(i * gap, y * gap, cellPos.z);
                cell.ResetScale();
                sequence.Join(cell.transform.DOMove(cell.transform.position - (Vector3.up * fill.Count * gap), moveDuration));
                boardCells[i, model.boardHeight - j] = cell;
                cell.SetCoordinate(i, model.boardHeight - j);
            }
        }
        sequence.OnComplete(() =>
        {
            model.MoveBoardValues(movementMatrix);
            model.FillValue(fillsPerColumns);
            DL.Utils.CoroutineUtils.Invoke(this, RecheckBoard, 0.1f);
            isPlayingAnimation = false;
        });
    }



    private void RecheckBoard()
    {
        var result = model.CheckAndGetResult();
        if (result.Count > 0)
        {
            var sequence = DOTween.Sequence();
            foreach (var cell in result)
            {
                var (x, y) = cell;
                isPlayingAnimation = true;
                var tween = boardCells[x, y].PlayDisappearAnimation(disappearDuration);
                cellPool.Enqueue(boardCells[x, y]);
                sequence.Join(tween);
            }
            sequence.OnComplete(() =>
            {
                model.RemoveCells(result);
                var movement = model.GetMovementMatrix(out int[] removedPerColumns);
                var fillsPerColumns = model.GetRandomFillValues(removedPerColumns, cellData.MaxCellValue);
                MoveAndFillCell(movement, fillsPerColumns);
            });
            ScoreManager.instance.AddScore(10 * result.Count);
            SoundManager.instance.PlayOneShot(SFX.Score);
        }
        else
        {
            if(!isPlayingAnimation) ScoreManager.instance.CheckAndPlayCoinAnim();
            if (!model.DetectMove())
            {
                this.PlayResetAnimation(this.RecheckBoard);
            }
        }
    }
    public void GameStart()
    {
        DL.Utils.CoroutineUtils.Invoke(this, RecheckBoard, 0.4f);
    }
    private void PlayResetAnimation(TweenCallback onComplete)
    {
        this.isPlayingAnimation = true;
        var sequence = DOTween.Sequence();
        this.notiText.transform.localScale = new Vector3(1, 0, 1);
        sequence.Append(notiText.transform.DOScale(Vector3.one, this.moveDuration * 2));
        sequence.Append(notiText.transform.DOScale(new Vector3(1, 0, 1), moveDuration * 2).SetDelay(moveDuration * 1.5f));
        sequence.OnComplete(() =>
        {
            this.GenerateLevel();
            var secondSequence = DOTween.Sequence();
            this.boardCells.Loop((item, x, y) =>
            {
                var worldPos = new Vector2(x, y) * gap;
                var startPos = worldPos + Vector2.up * gap * 5;
                item.transform.position = startPos;
                var tween = item.transform.DOMove(worldPos, moveDuration);
                if (x == 0 && y == 0) secondSequence.Append(tween);
                else secondSequence.Join(tween);
            });
            secondSequence.OnComplete(() =>
            {
                this.isPlayingAnimation = false;
                onComplete?.Invoke();
            });
        });
    }
    [Sirenix.OdinInspector.Button]
    private void Test()
    {
        //this.GenerateLevel();
        this.PlayResetAnimation(this.RecheckBoard);
    }
}
