using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BallLine
{
    public class Level : MonoBehaviour
    {
        public static Action<GameObject> levelInitEvent;
        
        public int levelSequenceNumber;
        public string levelName;
        public int price;
        public bool isFree = false;
        public int scoreToWin;
        public float  levelSpeed;
        public float levelTimerInSeconds;
        public int levelCoinRewards;  
        private string levelString = "LEVEL"; 
       [SerializeField] private bool isUnlockedAndPlayable; 
        [SerializeField] private PerLevelDataManager thisLevelManager; 
        //***uncomment the cobe below to use pass level when enough score feature

        //public int scoreToPass = 200;

        public  PerLevelDataManager GetLevelManager => thisLevelManager;
        
        
        // call this to  check      
        public bool IsUnlocked
        {
            get
            {
                return (isFree || PlayerPrefs.GetInt(levelName, 0) == 1);
            }
        }
        
        

        void Awake()
        {
            AddLevelData();
            levelInitEvent.Invoke(this.gameObject);

        }

        private void Start()
        {
            if (levelSequenceNumber == 0)
            {
                isUnlockedAndPlayable = true;
                isFree = true;
            }
            
            if(IsUnlocked)
            {
                isUnlockedAndPlayable = true;
            }
        }

        public bool Unlock(bool isDefault=false , bool forceUnlock = false )
        {
            if (IsUnlocked)
            {
                var levelNo= levelSequenceNumber + 1;
                Debug.LogError("Already unlocked  " + levelNo);
                return true;
            }
           
            if (CoinManager.Instance.Coins >= price || forceUnlock)
            {
                PlayerPrefs.SetInt(levelName, 1);
                PlayerPrefs.Save();
                CoinManager.Instance.RemoveCoins(price);
                var levelNo= levelSequenceNumber + 1;
                Debug.LogError(" unlocked NEW LEVEL  " + levelNo);
                //levelName =  "LEVEL" + sequence number

                return true;
            }
            if(isDefault)
            {
                PlayerPrefs.SetInt(levelName, 1);
                PlayerPrefs.Save();
                

                return true;
            }

            return false;
        }
        
        public void AddLevelData()
        {
            foreach (levelParameters  level in thisLevelManager.level  )
            {
                if (this.levelSequenceNumber + 1 == level.levelNumber)
                {
                    this.scoreToWin = level.scoreToWin;
                    this.levelName = levelString + level.levelNumber;
                    this.levelTimerInSeconds = level.levelTimerInSeconds;
                    this.levelCoinRewards = level.levelCoinReward;
                    this.levelSpeed = level.gameSpeed;
                }
            }
        }
    }
}