using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoresManager : MonoBehaviour
{
    public List<TextMeshProUGUI> highScoreDisplays = new List<TextMeshProUGUI>(); 

    private SceneHandler sceneHandler;

    //private List<Tuple<int, string>> currentHighScoresAndPlayers;

    // Start is called before the first frame update
    void Awake()
    {
        sceneHandler = SceneHandler.sceneHandler;
    }

    private void Start()
    {
        for (int index = 0; index < sceneHandler.top5HighScoresAndPlayers.Count; index++)
        {
            highScoreDisplays[index].text = sceneHandler.top5HighScoresAndPlayers[index].Item1 + " - " + sceneHandler.top5HighScoresAndPlayers[index].Item2;            
        }
        Debug.Log("updated all highscores");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
