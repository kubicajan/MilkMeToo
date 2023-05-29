using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }
}