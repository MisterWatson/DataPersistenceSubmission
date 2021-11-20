using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneHandler : MonoBehaviour
{
    public static string playerName;
    public static string highScorePlayerName;
    public static int highScore;

    public static SceneHandler sceneHandler;

    private TMP_InputField playerNameInput;

    public List<Tuple<int, string>> top5HighScoresAndPlayers = new List<Tuple<int, string>>();

    void Awake()
    {
        if (sceneHandler == null)
        {
            sceneHandler = this;
            DontDestroyOnLoad(gameObject);
        }
        LoadHighScoreAndPlayer();

        // Debugging only
        //Debug.Log("Top players currently contain:");
        //Debug.Log(top5HighScoresAndPlayers[0]);
        //Debug.Log(top5HighScoresAndPlayers[1]);
        //Debug.Log(top5HighScoresAndPlayers[2]);
        //Debug.Log(top5HighScoresAndPlayers[3]);
        //Debug.Log(top5HighScoresAndPlayers[4]);
    }

    public void LoadGame()
    {
        playerNameInput = GameObject.Find("Username Prompt").GetComponent<TMP_InputField>();
        playerName = playerNameInput.text;

        SceneManager.LoadScene(1);
    }

    public void ShowHighScores()
    {
        SceneManager.LoadScene(2);
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    class SaveData
    {
        public string highScorePlayerName;
        public int highScore;
        public List<Tuple<int, string>> top5PlayersAndHighscores = new List<Tuple<int, string>>(); 
    }

    public void SaveHighScoreAndPlayer()
    {
        SaveData data = new SaveData();
        data.highScorePlayerName = playerName;
        data.highScore = highScore;
        data.top5PlayersAndHighscores.AddRange(top5HighScoresAndPlayers);

        // Recalculate top 5 highscores
        if (data.top5PlayersAndHighscores.Count < 5)
        {
            for (int i = data.top5PlayersAndHighscores.Count; i < 5; i++)
            {
                data.top5PlayersAndHighscores.Add(new Tuple<int, string>(0, "Not yet claimed"));
            }
        }
        data.top5PlayersAndHighscores.Sort((x, y) => y.Item1.CompareTo(x.Item1));

        // Debugging only
        //Debug.Log("current list: ");
        //Debug.Log(data.top5PlayersAndHighscores[0]);
        //Debug.Log(data.top5PlayersAndHighscores[1]);
        //Debug.Log(data.top5PlayersAndHighscores[2]);
        //Debug.Log(data.top5PlayersAndHighscores[3]);
        //Debug.Log(data.top5PlayersAndHighscores[4]);

        data.top5PlayersAndHighscores = data.top5PlayersAndHighscores.GetRange(0, 5);

        //Debug.Log(data.top5HighScoresAndPlayersToSave.Count);
        Debug.Log("Saving highscores to file (total = " + data.top5PlayersAndHighscores.Count + " highscores)");

        // save file in binary format to make sure the list of tuples is saved correctly
        Stream SaveFileStream = File.Create(Application.persistentDataPath + "/savefile.bin");
        BinaryFormatter serializer = new BinaryFormatter();
        serializer.Serialize(SaveFileStream, data);
        SaveFileStream.Close();

        Debug.Log("Highscores saved successfully");

    }

    public void LoadHighScoreAndPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.bin";
        if (File.Exists(path))
        {
            // Load from binary filestream
            Stream openFileStream = File.OpenRead(path);
            BinaryFormatter deserializer = new BinaryFormatter();
            SaveData data = (SaveData)deserializer.Deserialize(openFileStream);
            openFileStream.Close();

            // Update data in list of tuples
            //highScore = data.highScore;
            //highScorePlayerName = data.highScorePlayerName;
            top5HighScoresAndPlayers.AddRange(data.top5PlayersAndHighscores);

            UpdateHighScoreDisplay();

            Debug.Log("File loaded successfully (total = " + data.top5PlayersAndHighscores.Count + " highscores)");
        }
    }

    public void UpdateHighScoreDisplay()
    {

        top5HighScoresAndPlayers.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        top5HighScoresAndPlayers = top5HighScoresAndPlayers.GetRange(0, 5);

        highScore = top5HighScoresAndPlayers[0].Item1;
        highScorePlayerName = top5HighScoresAndPlayers[0].Item2;
    }

    public void QuitApplication()
    {
        // save the current high score
        SaveHighScoreAndPlayer();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
