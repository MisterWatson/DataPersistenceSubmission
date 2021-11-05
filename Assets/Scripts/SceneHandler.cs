using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
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

    // Start is called before the first frame update
    void Awake()
    {
        if (sceneHandler != null)
        {
            Destroy(gameObject);
            return;
        }
        sceneHandler = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScoreAndPlayer();
    }

    public void LoadGame()
    {
        playerNameInput = GameObject.Find("Username Prompt").GetComponent<TMP_InputField>();
        playerName = playerNameInput.text;

        SceneManager.LoadScene(1);

        // Debugging
        Debug.Log(playerName);
    }

    [System.Serializable]
    class SaveData
    {
        public string highScorePlayerName;
        public int highScore;
    }

    public void SaveHighScoreAndPlayer()
    {
        SaveData data = new SaveData();
        data.highScorePlayerName = playerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        //Debug.Log("saved it");
    }

    public void LoadHighScoreAndPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
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
