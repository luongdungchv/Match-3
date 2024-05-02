using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DL.Utils;
using System.Threading;
using Sirenix.Utilities;

public class BoardNodel : SerializedMonoBehaviour
{
    [SerializeField] private int[,] mainBoard, pendingBoard, tempBoard;

    public int boardWidth => mainBoard.GetLength(0);
    public int boardHeight => mainBoard.GetLength(1);

    public int[,] GenerateRandomValues(int width, int height, int maxValue){
        var seed = 0;
        var randomObj = new System.Random();
        mainBoard = new int[width, height];
        mainBoard.Loop((item, x, y) => {
            mainBoard[x, y] = randomObj.Next(0, maxValue + 1);
        });

        return mainBoard;
    }

    [Sirenix.OdinInspector.Button]
    public HashSet<(int, int)> CheckAndGetResult()
    {
        var result = new List<(int, int)>();

        for(int x = 0; x < boardWidth; x++){
            var match = new List<(int, int)>();
            var lastValue = -1;
            for(int y = 0; y < boardHeight; y++){
                var value = mainBoard[x,y];
                if(lastValue != value){
                    if(match.Count >= 3) result.AddRange(match);
                    match.Clear();
                }
                match.Add((x, y));
                lastValue = value;
                if(y == boardHeight - 1)
                    if(match.Count >= 3) result.AddRange(match);
            }
        }

        for(int y = 0; y < boardHeight; y++){
            var match = new List<(int, int)>();
            var lastValue = -1;
            for(int x = 0; x < boardWidth; x++){
                var value = mainBoard[x,y];
                if(lastValue != value){
                    if(match.Count >= 3) result.AddRange(match);
                    match.Clear();
                }
                match.Add((x, y));
                lastValue = value;
                if(x == boardWidth - 1)
                    if(match.Count >= 3) result.AddRange(match);
            }
        }
        var realResult = new HashSet<(int, int)>();
        result.ForEach(x => realResult.Add(x));
        return realResult;
    }

    public void RemoveCells(HashSet<(int,int)> cells){
        foreach(var (x, y) in cells){
            mainBoard[x, y] = -1;
        }
    }

    public int[,] GetMovementMatrix(out int[] removedPerColumns){
        var result = new int[boardWidth, boardHeight];
        removedPerColumns = new int[boardWidth];
        for(int x = 0; x < boardWidth; x++){
            var removeCount = 0;
            for(int y = 0; y < boardHeight; y++){
                if(mainBoard[x, y] < 0){
                    removeCount++;
                }
                else{
                    result[x, y] = removeCount;
                }
            }
            removedPerColumns[x] = removeCount;
        }
        return result;
    }
    public void FillValue(List<int>[] fillsPerColumn){
        for(int i = 0; i < fillsPerColumn.Length; i++){
            var fill = fillsPerColumn[i];
            for (int j = 1; j <= fill.Count; j++)
            {
                mainBoard[i, boardHeight - j] = fill[j - 1];
            }
        }
            
    }
 
    public void MoveBoardValues(int[,] moveMatrix){
        moveMatrix.Loop((item, x, y) => {
            var targetY = y - item;
            var moveValue = mainBoard[x, y];
            mainBoard[x, y] = -1;
            mainBoard[x, targetY] = moveValue;
        });
    }

    public List<int>[] GetRandomFillValues(int[] removedPerColumns, int maxItemVal){
        var result = new List<int>[boardWidth];
        var randomObj = new System.Random();
        for (int i = 0; i < removedPerColumns.Length; i++)
        {
            var value = new List<int>();
            for (int j = 0; j < removedPerColumns[i]; j++)
            {
                value.Add(randomObj.Next(0, maxItemVal + 1));
            }
            result[i] = value;
        }
        return result;
    }
    public void SwitchCells((int, int)cell1, (int, int)cell2){
        var cache = mainBoard[cell1.Item1, cell1.Item2];
        mainBoard[cell1.Item1, cell1.Item2] = mainBoard[cell2.Item1, cell2.Item2];
        mainBoard[cell2.Item1, cell2.Item2] = cache;
    }

    public bool DetectMove(){
        for(int x = 0; x < boardWidth; x++){
            for(int y = 0; y < boardHeight - 2; y++){
                var val = mainBoard[x, y];
                var val1 = mainBoard[x, y + 1];
                var val2 = mainBoard[x, y + 2];
                if(val1 == val2 && val1 != val){
                    if(x -1 >= 0 && mainBoard[x - 1, y] == val1) return true;
                    if(x + 1 < boardWidth && mainBoard[x + 1, y] == val1) return true;
                    if(y - 1 >= 0 && mainBoard[x, y - 1] == val1) return true;
                }
                if(val == val2 && val1 != val){
                    if(x -1 >= 0 && mainBoard[x - 1, y + 1] == val) return true;
                    if(x + 1 < boardWidth && mainBoard[x + 1, y + 1] == val) return true;
                    if(y + 3 < boardHeight && mainBoard[x, y + 3] == val) return true;
                }
                if(val1 == val && val1 != val2){
                    if(x -1 >= 0 && mainBoard[x - 1, y + 2] == val1) return true;
                    if(x + 1 < boardWidth && mainBoard[x + 1, y + 2] == val1) return true;
                    if(y + 3 < boardHeight && mainBoard[x, y + 3] == val) return true;
                }
            }
        }

        for(int y = 0; y < boardHeight; y++){
            for(int x = 0; x < boardWidth - 2; x++){
                var val = mainBoard[x, y];
                var val1 = mainBoard[x + 1, y];
                var val2 = mainBoard[x + 2, y];
                if(val1 == val2 && val1 != val){
                    if(y -1 >= 0 && mainBoard[x, y - 1] == val1) return true;
                    if(y + 1 < boardHeight && mainBoard[x, y + 1] == val1) return true;
                    if(x - 1 >= 0 && mainBoard[x - 1, y] == val1) return true;
                }
                if(val == val2 && val1 != val){
                    if(y -1 >= 0 && mainBoard[x + 1, y - 1] == val) return true;
                    if(y + 1 < boardHeight && mainBoard[x + 1, y + 1] == val) return true;
                    if(x + 3 < boardWidth && mainBoard[x + 3, y] == val) return true;
                }
                if(val1 == val && val1 != val2){
                    if(y -1 >= 0 && mainBoard[x + 2, y - 1] == val1) return true;
                    if(y + 1 < boardHeight && mainBoard[x + 2, y + 1] == val1) return true;
                    if(x + 3 < boardWidth && mainBoard[x + 3, y] == val) return true;
                }
                
            }
        }
        return false;
    }

    [Button]
    private void CacheBoard(){
        tempBoard = new int[boardWidth, boardHeight];
        mainBoard.Loop((item, x, y) => {
            tempBoard[x, y] = item;
        });
    }
    [Button]
    private void RestoreBoard(){
       tempBoard.Loop((item, x, y) => {
            mainBoard[x, y] = item;
        }); 
    }

}
