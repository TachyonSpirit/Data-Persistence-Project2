using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public string hsUsername;
    public int hsScore;

    // Start is called before the first frame update
    void Start()
    {
        // Get the current highscore username and current highscore score
        LoadHighScore();
        Debug.Log("hsUsername is  : " + hsUsername);
        Debug.Log("hsScore is     : " + hsScore);
        
        if (PersistencyManager.Instance.userName == "RESET")
        {
            SaveHighScore("Dummy",0);
        }

        // Display the current highscore username and current highscore score on the screen
        BestScoreText.text = "Highscore : " + hsUsername + " : " + hsScore;

        ScoreText.text = "Score " + "(" + PersistencyManager.Instance.userName + ")" + " : 0";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = "Score " + "(" + PersistencyManager.Instance.userName + ") : " + m_Points;
    }

    private void UpdateBestScoreText()
    {
        //BestScoreText.text = "Best Score : " + PersistencyManager.Instance.userName;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Check if the user's score is great than the highscore
        if (m_Points > hsScore)
        {
            // Save the new data to disk!
            SaveHighScore(PersistencyManager.Instance.userName,m_Points);
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string highScore_User;
        public int highScore_Score;
    }
    public void SaveHighScore(string my_user, int my_score)
    {
        SaveData data = new SaveData();
        data.highScore_User = my_user;
        data.highScore_Score = my_score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadHighScore()
    {
        Debug.Log("Started LoadHighScore");
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            hsUsername = data.highScore_User;
            hsScore = data.highScore_Score;
        }
        Debug.Log("Finished LoadHighScore");
    }
}
