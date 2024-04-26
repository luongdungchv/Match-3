using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureManager : MonoBehaviour
{
    [SerializeField] private List<Animator> figureAnimatorList;

    public void PlayBravoAnimation(){
        figureAnimatorList.ForEach(x => x.SetTrigger("Bravo"));
    }
}
