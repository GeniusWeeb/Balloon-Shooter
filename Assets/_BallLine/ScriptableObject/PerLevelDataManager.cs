using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LevelDataManager" , fileName = "LevelEditor")]
public class PerLevelDataManager : ScriptableObject
{

    public List<levelParameters> level;
    
    public levelParameters GetLevelFinalData()
    {
        return new levelParameters();
    }
}

[System.Serializable]
public class levelParameters
{
    public int levelNumber;
    public int scoreToWin;
    public float gameSpeed;
    public float levelTimerInSeconds;
    public int levelCoinReward;

}
