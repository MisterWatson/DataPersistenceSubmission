using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneHandler : MonoBehaviour
{
    public static string playerName;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadGame()
    {
        playerNameInput = GameObject.Find("Username Prompt").GetComponent<TMP_InputField>();
        playerName = playerNameInput.text;

        SceneManager.LoadScene(1);

        // Debugging
        Debug.Log(playerName);
    }
}
