using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SongManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource kokPanelAudioSourceForSong;
        [SerializeField] private AudioSource cowPanelAudioSourceForSong;
        [SerializeField] private AudioSource shopPanelAudioSourceForSong;
        [SerializeField] private AudioSource eventAudioSource;
        [SerializeField] private AudioClip kokPanelSong;
        [SerializeField] private AudioClip cowPanelSong;
        [SerializeField] private AudioClip shopPanelSong;
        [SerializeField] private AudioClip eventSong;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip purchase;
        private List<AudioSource> shopAudios = new();
        private List<AudioSource> cowPanelAudios = new();
        private List<AudioSource> koktreeAudios = new();
        private int rememberLastPanel;

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

        private void Start()
        {
            // cowPanelAudios.Add(GameObject.Find("audioSourceDrugs").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("Ginger1audioSource").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCapyAnimation").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCatAnimation").GetComponent<AudioSource>());
            kokPanelAudioSourceForSong.clip = kokPanelSong;
            kokPanelAudioSourceForSong.loop = true;
            cowPanelAudioSourceForSong.clip = cowPanelSong;
            cowPanelAudioSourceForSong.loop = true;
            shopPanelAudioSourceForSong.clip = shopPanelSong;
            shopPanelAudioSourceForSong.loop = true;
            eventAudioSource.clip = eventSong;
            eventAudioSource.loop = true;
            UpdateAudioMutes(1);
        }

        public void UpdateAudioMutes(int panelNumber)
        {
            // todo: uncomment
             // switch (panelNumber)
             // {
             //     case 0:
             //         MuteShopAudios(true);
             //         MuteCowPanel(true);
             //         MuteKoktreeAudios(false);
             //         kokPanelAudioSourceForSong.Play();
             //         cowPanelAudioSourceForSong.Pause();
             //         shopPanelAudioSourceForSong.Pause();
             //         eventAudioSource.Pause();
             //         rememberLastPanel = panelNumber;
             //         break;
             //     case 1:
             //         MuteShopAudios(true);
             //         MuteCowPanel(false);
             //         MuteKoktreeAudios(true);
             //         kokPanelAudioSourceForSong.Pause();
             //         cowPanelAudioSourceForSong.Play();
             //         shopPanelAudioSourceForSong.Pause();
             //         eventAudioSource.Pause();
             //         rememberLastPanel = panelNumber;
             //         break;
             //     case 2:
             //         MuteShopAudios(false);
             //         MuteCowPanel(true);
             //         MuteKoktreeAudios(true);
             //         kokPanelAudioSourceForSong.Pause();
             //         cowPanelAudioSourceForSong.Pause();
             //         shopPanelAudioSourceForSong.Play();
             //         eventAudioSource.Pause();
             //         rememberLastPanel = panelNumber;
             //         break;
             //     case 3:
             //         MuteShopAudios(true);
             //         MuteCowPanel(true);
             //         MuteKoktreeAudios(true);
             //         kokPanelAudioSourceForSong.Pause();
             //         cowPanelAudioSourceForSong.Pause();
             //         shopPanelAudioSourceForSong.Pause();
             //         eventAudioSource.Play();
             //         break;
             // }
        }

        public void PlayLastOne()
        {
            UpdateAudioMutes(rememberLastPanel);
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