using UnityEngine;
namespace NamCore
{
    public class GameFlowManager : MonoBehaviour
    {
        public static GameFlowManager Instance;

        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
             GoToMenu(); // Mặc định bắt đầu ở Menu
            //StartGame();
        }

        public void GoToMenu()
        {
            CurrentState = GameState.MainMennu;
            SenceLoader.Ins.LoadSence(SenceID.MainMennu);
        }

        public void StartGame()
        {
            CurrentState = GameState.GamePlay;
            SenceLoader.Ins.LoadSence(SenceID.GameplayScene);
        }

      /*  public void EndGame()
        {
            CurrentState = GameState.GameOver;
            SenceLoader.Ins.LoadSence(SenceID.MennuSence);
        }*/
    }
}

