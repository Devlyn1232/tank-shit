using UnityEngine; 
using System.Collections; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
 
public class GameManager : MonoBehaviour 
{ 
    public HighScores m_HighScores;
    
    public Text m_MessageText;            
    public Text m_TimerText; 

    public GameObject m_HighScorePanel; 
    public Text m_HighScoresText; 
 
    public Button m_NewGameButton; 
    public Button m_HighScoresButton; 

    public GameObject[] m_Tanks; 
    public mouseLook MouseLook;
     
    

    private float m_gameTime = 0; 
    public float GameTime { get { return m_gameTime; } } 
    
    private bool OneTankLeft() 
    { 
        MouseLook.mL();
        int numTanksLeft = 0; 
 
        for (int i = 0; i < m_Tanks.Length; i++) 
        { 
            if (m_Tanks[i].activeSelf == true) 
            { 
                numTanksLeft++; 
            } 
        } 
 
        return numTanksLeft <= 1; 
    } 
    
    public void OnNewGame() 
    { 

        m_NewGameButton.gameObject.SetActive(false); 
        m_HighScoresButton.gameObject.SetActive(false); 
        m_HighScorePanel.SetActive(false); 
        
        
        
        MouseLook.mL();

        m_gameTime = 0; 
        m_GameState = GameState.Playing; 
        m_TimerText.gameObject.SetActive(true); 
        m_MessageText.text = ""; 

        for (int i = 0; i < m_Tanks.Length; i++) 
        { 
                m_Tanks[i].SetActive(true); 
        } 
        SceneManager.LoadScene("SampleScene");
    } 
    public void OnHighScores() 
    { 
        MouseLook.mF();
        m_MessageText.text = ""; 
        m_TimerText.gameObject.SetActive(true); 
        m_HighScoresButton.gameObject.SetActive(false); 
        m_HighScorePanel.SetActive(true); 

        string text = ""; 
        for (int i = 0; i < m_HighScores.scores.Length; i++) 
        { 
            int seconds = m_HighScores.scores[i]; 
            text += string.Format("{0:D2}:{1:D2}\n",  
            (seconds / 60), (seconds % 60)); 
        } 
        m_HighScoresText.text = text; 
    } 
    
    private bool IsPlayerDead() 
    { 
        
        for (int i = 0; i < m_Tanks.Length; i++) 
        { 
            if (m_Tanks[i].activeSelf == false) 
            { 
                if (m_Tanks[i].tag == "Player") 
                    return true; 
            } 
        } 
        return false; 
    } 

    public enum GameState 
    { 
        Start, 
        Playing, 
        GameOver 
    }; 
 
    private GameState m_GameState; 
    public GameState State { get { return m_GameState; } } 
    
    private void Start() 
    { 
        for (int i = 0; i < m_Tanks.Length; i++) 
        { 
            m_Tanks[i].SetActive(false); 
        } 

        m_TimerText.gameObject.SetActive(false); 
        m_MessageText.text = "Enter To Start";
        MouseLook.mL();
        m_HighScorePanel.gameObject.SetActive(false); 
        m_NewGameButton.gameObject.SetActive(false); 
        m_HighScoresButton.gameObject.SetActive(false); 
    }

    private void Awake() 
    { 
        m_GameState = GameState.Start; 
    } 

    void Update() 
    { 
        switch (m_GameState) 
        { 
            case GameState.Start: 
                if (Input.GetKeyUp(KeyCode.Return) == true) 
                { 
                    m_TimerText.gameObject.SetActive(true); 
                    m_MessageText.text = ""; 
                    m_GameState = GameState.Playing; 
 
                    for (int i = 0; i < m_Tanks.Length; i++) 
                    { 
                        m_Tanks[i].SetActive(true); 
                    } 
                } 
                break; 
                case GameState.Playing: 
                bool isGameOver = false; 
 
                m_gameTime += Time.deltaTime; 
                int seconds = Mathf.RoundToInt(m_gameTime); 
                m_TimerText.text = string.Format("{0:D2}:{1:D2}",  
                (seconds / 60), (seconds % 60)); 
 
                if (OneTankLeft() == true) 
                { 
                    isGameOver = true; 
                } 
                else if (IsPlayerDead() == true) 
                { 
                    isGameOver = true; 
                } 
 
                if (isGameOver == true) 
                { 
                    m_GameState = GameState.GameOver; 
                    m_TimerText.gameObject.SetActive(true); 

                    m_NewGameButton.gameObject.SetActive(true); 
                    m_HighScoresButton.gameObject.SetActive(true);

                    if (IsPlayerDead() == true) 
                    { 
                        m_MessageText.text = "TRY AGAIN"; 
                    } 
                    else 
                    { 
                        
                        m_MessageText.text = "WINNER!"; 
                        MouseLook.mF();
                        // save the score 
                        m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime)); 
                        m_HighScores.SaveScoresToFile();
                    } 
                } 
                break; 
                case GameState.GameOver: 
                if (Input.GetKeyUp(KeyCode.Return) == true) 
                { 
                    m_gameTime = 0; 
                    m_GameState = GameState.Playing; 
 
                    for (int i = 0; i < m_Tanks.Length; i++) 
                    { 
                        m_Tanks[i].SetActive(true); 
                    } 
                } 
                break; 
            } 
        if (Input.GetKeyUp(KeyCode.Escape)) 
        { 
            Application.Quit(); 
        } 
    
    
    }

}