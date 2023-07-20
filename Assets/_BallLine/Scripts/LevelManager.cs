using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BallLine
{
    [System.Serializable]
    public class LevelTest
    {
        public GameObject level;
        public Sprite levelImage;
    }
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
     

        public static readonly string CURRENT_Level_KEY = "SGLIB_CURRENT_Level";

        
        // this saves the data in player prefs
        public int CurrentLevelIndex
        {
            get
            {
                return PlayerPrefs.GetInt(CURRENT_Level_KEY, 0);
            }
            set
            {
                PlayerPrefs.SetInt(CURRENT_Level_KEY, value);
                PlayerPrefs.Save();
            }
        }

        public GameObject[] Levels;
        public LevelTest[] levelTests;
        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        //Called Automatically after winning a gam
        //
        //Highest Unlocked LevelIndex -> Index means always  value -1 
        public void OnWinUnlockNewLevel()
        {
            
        }
    }
}