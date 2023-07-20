using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BallLine
{
    public enum GameState
    {
        Prepare,
        Playing,
        Paused,
        PreGameOver,
        GameOver
    }

    public enum GameResult
    {
        WIN ,
        LOSE ,
        NULL
    }

    public enum CoinEarned
    {
        OnceCombo,
        EachCombo,
        IncreaseEachCombo
    }

    public class GameManager : MonoBehaviour
    {

        [Header("Current Level")] public int currentLevelIndex;
        public Level currentPlayingLevel;

        public GameResult currentGameResult;
        
        public static GameManager Instance { get; private set; }
            
        public static event System.Action<GameState, GameState> GameStateChanged;
        [Header("Gameplay Config")]

        [SerializeField]
        private string screenShootPath = "Assets/_BallLine/ScreenShot/";
        public string ScreenShootPath
        {
            get { return screenShootPath; }
        }

        #region  events

        public static  Action<GameResult> GameOverWithResult;

        #endregion
        
        [Range(0f, 1f)]
        public float coinFrequency = 0.1f;
    
        public Color selectShadowColor = Color.green;
        public Color normalShadowColor = Color.black;
        public Color selectTextColor = Color.yellow;
        public Color normalTextColor = Color.white;

        public float moveBallSpeed=1;

        public float shootBallSpeed = 0.1f;

        public int createBombBallAtComboScore=0;

        public float bombBallExplosionRadius = 2.5f;

        public float speedUp = 1;

        public float distanceFromCamera=15.8911f;

        public float moveBackSpeed = 30;

        public float timeSpeedUp=0.5f;

        public CoinEarned monetizationOptions = CoinEarned.OnceCombo;

        public int earnCoinAtComboScore = 1;

        public int coinEarned=1;
        [Range(1,100)]
        public int amountCoinIncrease=0;

        public string backgroundMessage = "Do you want to unlock this background?";

        public string levelMessage = "Do you want to unlock this level?";

        public string characterMessage = "Do you want to unlock this model?";

        private static bool isRestart;

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        public static int GameCount
        {
            get { return _gameCount; }
            private set { _gameCount = value; }
        }

        private static int _gameCount = 0;

        [Header("Set the target frame rate for this game")]
        [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
        public int targetFrameRate = 30;

        [Header("Current game state")]
        [SerializeField]
        private GameState _gameState = GameState.Prepare;

        // List of public variables referencing other objects
        [Header("Object References")]
        public PlayerController playerController;
        public GameObject mainCanvas;
        public GameObject characterUI;
        private ObjectPooling currentPooledObjects;

        void OnEnable()
        {
            GameOverWithResult += CheckGameWinLoss;
            PlayerController.PlayerDied += PlayerController_PlayerDied;
            Level.levelInitEvent += AssignCurrentLevel;
            
        }

       private void AssignCurrentLevel(GameObject level)
       {
           currentPlayingLevel = level.GetComponent<Level>();
       
       }

        void OnDisable()
        {   
            GameOverWithResult -= CheckGameWinLoss;
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
            Level.levelInitEvent -= AssignCurrentLevel;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }

            currentPooledObjects = this.GetComponent<ObjectPooling>();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            // Initial setup
            
            Application.targetFrameRate = targetFrameRate;
            ScoreManager.Instance.Reset();
            Debug.LogError("Starting game");
            PrepareGame();
        }

        private void CheckGameWinLoss(GameResult result)
        {
            currentGameResult = result;
            if (currentGameResult == GameResult.WIN)
            {
                Debug.LogError("You win");
                UIManager.Instance.playNextLevelUI.SetActive(true);
                speedUp = 200f;
                playerController.StartSpeedUp();
                GameOver();
                playerController.isPlay = false;
                AutoUnlockNewLevel();
            }
            else
            {  
                Debug.LogError("You lose");

            }

        }
    

        // Update is called once per frame
        // Listens to the event when player dies and call GameOver
        void PlayerController_PlayerDied()
        {
            if (_gameState == GameState.GameOver)
                return;
            CheckGameWinLoss(GameResult.LOSE);
            GameOver();
        }

        // Make initial setup and preparations before the game can be played
        public void PrepareGame()
        {
            GameState = GameState.Prepare;

            // Automatically start the game if this is a restart.
            if (isRestart)
            {
                isRestart = false;
                StartGame();
                return;
            }
            StartGame();
        }

        // A new game official starts
        
        [ContextMenu("Start Game")]
        public void StartGame()
        {
          
            GameState = GameState.Playing;
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
            }

          //  currentLevelIndex = LevelManager.Instance.CurrentLevelIndex +1 ;
        }

        // Called when the player died
        public void GameOver()
        {
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.StopMusic();
            }

            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            GameState = GameState.GameOver;
            GameCount++;
            // Add other game over actions here if necessary
        }
        

        // Start a new game
        public void RestartGame(float delay = 0)
        {
            isRestart = true;
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HidePlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(false);
        }

        public void ShowPlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(true);
        }

        void StopSpeedUp()
        {
            if (playerController.GetComponent<PlayerController>().isDie)
            {
                playerController.GetComponent<PlayerController>().RaiseEventDie();
            }
            else
            {
                StartCoroutine(Delay());
                playerController.isSpeedUp = false;
                playerController.isBengin = false;
                playerController.EndSpeedUp();
                playerController.CreateShootBall();
                playerController.isPlay = true;
            }

        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.1f);
            playerController.createBallSpeed = (0.97f / GameManager.Instance.moveBallSpeed);
        }

        public void SpeedUp()
        {
            if (playerController.GetComponent<PlayerController>().isDie)
            {
                playerController.StartSpeedUp();
                Invoke("StopSpeedUp", timeSpeedUp * Time.timeScale*0.5f);
            }
            else
            {
                //Time.timeScale = speedUp;
                playerController.isBengin = true;
                playerController.isSpeedUp = true;
                Invoke("StopSpeedUp", timeSpeedUp * Time.timeScale);
            }

        }
    
        IEnumerator SampleDelay(float time)
        {
            yield return new WaitForSeconds(time);
            UIManager.Instance.playNextLevelUI.SetActive(false);
            // this is the main identifier
            StartPlayLevel();
           
        }
        public void PlayNextLevel()
        {   
            if (playerController.levelSetupDone)
            {
                Debug.LogError("Setup Is Done");
                StartCoroutine(SampleDelay(1f));
            }
        }


        public void StartPlayLevel()
        {
            playerController.instantiateNewLevel(currentPlayingLevel.levelSequenceNumber+1);
            // not this , this is to store the currently played level in playerpref and this should be the highest level
            LevelManager.Instance.CurrentLevelIndex = currentPlayingLevel.levelSequenceNumber;
            Debug.LogError("current Level Index" + currentLevelIndex);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        

        private void AutoUnlockNewLevel()
        {
            LevelManager.Instance.Levels[currentPlayingLevel.levelSequenceNumber + 1].GetComponent<Level>().Unlock(false , true);
            LevelManager.Instance.CurrentLevelIndex = currentPlayingLevel.levelSequenceNumber+1;
            // After you win , unlock new level 

        }
        
    }
}