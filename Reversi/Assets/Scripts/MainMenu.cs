using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenAIAI()
    {
        PlayerPrefs.SetInt("playType", (int) LoadPlayers.PlayType.AIVsAI);
        SceneManager.LoadScene(1);
    }
    public void OpenPlayerAI()
    {
        PlayerPrefs.SetInt("playType", (int)LoadPlayers.PlayType.PlayerVsAI);
        SceneManager.LoadScene(1);
    }
    public void OpenPlayerPlayer()
    {
        PlayerPrefs.SetInt("playType", (int)LoadPlayers.PlayType.PlayerVzPlayer);
        SceneManager.LoadScene(1);
    }
}
