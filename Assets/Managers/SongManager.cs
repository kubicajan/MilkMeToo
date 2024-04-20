using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SongManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip purchase;

        public static SongManager instance;

        private void Awake()
        {
//            DontDestroyOnLoad(transform.gameObject);

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private List<AudioSource> shopAudios = new List<AudioSource>();
        private List<AudioSource> cowPanelAudios = new List<AudioSource>();
        private List<AudioSource> koktreeAudios = new List<AudioSource>();

        private void Start()
        {
            cowPanelAudios.Add(GameObject.Find("audioSourceDrugsAnimation").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCapyAnimation").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCatAnimation").GetComponent<AudioSource>());
            UpdateAudioMutes(1);
        }

        public void UpdateAudioMutes(int counter)
        {
            Debug.Log(counter);
            switch (counter)
            {
                case 0:
                    MuteShopAudios(true);
                    MuteCowPanel(true);
                    MuteKoktreeAudios(false);
                    break;
                case 1:
                    MuteShopAudios(true);
                    MuteCowPanel(false);
                    MuteKoktreeAudios(true);
                    break;
                case 2:
                    MuteShopAudios(false);
                    MuteCowPanel(true);
                    MuteKoktreeAudios(true);
                    break;
            }
        }

        private void MuteShopAudios(bool yesNo)
        {
            MuteAudios(yesNo, shopAudios);
        }

        private void MuteCowPanel(bool yesNo)
        {
            MuteAudios(yesNo, cowPanelAudios);
        }

        private void MuteKoktreeAudios(bool yesNo)
        {
            MuteAudios(yesNo, koktreeAudios);
        }

        private void MuteAudios(bool mute, List<AudioSource> audioSources)
        {
            foreach (AudioSource eyoAudioSource in audioSources)
            {
                eyoAudioSource.mute = mute;
            }
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

        public void PlayClick()
        {
            audioSource.PlayOneShot(click);
        }

        public void PlayPurchase()
        {
            audioSource.PlayOneShot(purchase);
        }
    }
}