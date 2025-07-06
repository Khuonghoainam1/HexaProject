using UnityEngine;

namespace NamCore
{
    /// <summary>
    ///Game manager
    /// </summary>

    public enum GameState
    {
        MainMennu,
        GamePlay,
        GamePause,
        GameOver,
        Loading,

    }
    public class GameManager : Singleton<GameManager>
    {
        #region Fields
        [SerializeField] private GameState m_currentState;
        public int level;
        public GlobalLevelDataSO levelData;
        #endregion

        #region Public Methods
        public GameState CurrentState { get => m_currentState; set => m_currentState = value; }
        #endregion

        #region UnityMethods
        private void Start()
        {
            ChangeState(GameState.MainMennu);
        }
        #endregion

        #region Private Methods

        public void ChangeState(GameState newState)
        {
            m_currentState = newState;
            switch (newState)
            {
                case GameState.MainMennu:
                    Debug.Log(GameState.MainMennu.ToString());
                    break;
                case GameState.GamePlay:
                    Debug.Log(GameState.GamePlay.ToString());
                    break;
                case GameState.GamePause:
                    Debug.Log(GameState.GamePause.ToString());
                    break;
                case GameState.GameOver:
                    Debug.Log(GameState.GameOver.ToString());
                    break;
            }
        }


        public void ResetGame()
        {
            ChangeState(GameState.MainMennu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GameFlowManager.Instance.StartGame();
            }

        }
        #endregion
    }
}

