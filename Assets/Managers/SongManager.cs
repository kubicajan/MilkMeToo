using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SongManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.LoadScene("Menu");
        }

        public void PlayMusic()
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }
    }
}
