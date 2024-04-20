using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip song;

        private void Start()
        {
            audioSource.clip = song;
            audioSource.loop = true;
            audioSource.Play();
        }

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
}