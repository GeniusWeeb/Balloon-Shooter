using System.Collections;
using System.Collections.Generic;
using BallLine;
using UnityEngine;

public class LevelChooseUIManager : MonoBehaviour
{


    public void CloseThis()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.LevelChooseBtn.SetActive(true);
    }
}
