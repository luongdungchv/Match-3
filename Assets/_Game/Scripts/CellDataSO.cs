using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cell Data", menuName = "Scriptable Objects/Cell Data")]
public class CellDataSO: ScriptableObject
{
    [SerializeField] private List<Sprite> cellSpriteList;

    public int MaxCellValue => cellSpriteList.Count - 1;
    public Sprite GetSprite(int cellVal){
        return cellSpriteList[cellVal];
    }
}
