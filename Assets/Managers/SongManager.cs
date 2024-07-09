using System.Collections.Generic;
using Objects.ActiveObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SongManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Drugs Drugs;
        [SerializeField] private AudioSource kokPanelAudioSourceForSong;
        [SerializeField] private AudioSource cowPanelAudioSourceForSong;
        [SerializeField] private AudioSource shopPanelAudioSourceForSong;
        [SerializeField] private AudioSource eventAudioSource;
        [SerializeField] private AudioClip kokPanelSong;
        [SerializeField] private AudioClip gamerSong;
        [SerializeField] private AudioClip cowPanelSong;
        [SerializeField] private AudioClip shopPanelSong;
        [SerializeField] private AudioClip eventSong;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip purchase;
        [SerializeField] private Image musicHolder;
        [SerializeField] private GameObject soundButton;


        private List<AudioSource> shopAudios = new();
        private List<AudioSource> cowPanelAudios = new();
        private List<AudioSource> koktreeAudios = new();
        private List<AudioSource> specialAudios = new();
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
            cowPanelAudios.Add(GameObject.Find("Ginger1audioSource").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCapyAnimation").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCatAnimation").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceCow").GetComponent<AudioSource>());
            cowPanelAudios.Add(GameObject.Find("audioSourceDrugs").GetComponent<AudioSource>());


            specialAudios.Add(GameObject.Find("audioSourceSplash").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("audioSourceSwipe").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("audioSourceEvent").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("audioSourceEventPopUpClick").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("eventSongAudi").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("audioSource").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("audioSourceHorn").GetComponent<AudioSource>());
            specialAudios.Add(GameObject.Find("tiktokAudioSource").GetComponent<AudioSource>());


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

        private bool mutedMusic = false;
        private bool mutedAudios = false;

        private int counter = 0;

        public void MuteAllMusic()
        {
            counter++;

            if (counter == 1)
            {
                musicHolder.color = Color.blue;
                mutedMusic = false;
                cowPanelAudioSourceForSong.clip = gamerSong;
                cowPanelAudioSourceForSong.Play();
                return;
            }

            if (counter == 2)
            {
                musicHolder.color = Color.red;
                mutedMusic = true;
                cowPanelAudioSourceForSong.Pause();
                shopPanelAudioSourceForSong.Pause();
                return;
            }

            if (counter == 3)
            {
                musicHolder.color = Color.white;
                mutedMusic = false;
                cowPanelAudioSourceForSong.clip = cowPanelSong;
                cowPanelAudioSourceForSong.Play();
                UpdateAudioMutes(1);
                counter = 0;
            }
        }

        public void MuteAllSounds()
        {
            mutedAudios = !mutedAudios;
            if (!mutedAudios)
            {
                soundButton.transform.GetComponent<Image>().color = Color.cyan;
                MuteSpecialAudios(false);
                UpdateAudioMutes(1);
            }
            else
            {
                soundButton.transform.GetComponent<Image>().color = Color.red;
                MuteSpecialAudios(true);
                MuteShopAudios(true);
                MuteCowPanel(true);
            }
        }


        public void UpdateAudioMutes(int panelNumber)
        {
            switch (panelNumber)
            {
                case 0:
                    if (!mutedAudios)
                    {
                        MuteShopAudios(false);
                        MuteCowPanel(true);
                    }

                    Drugs.onMilkingScreen = false;

                    cowPanelAudioSourceForSong.Pause();
                    if (!shopPanelAudioSourceForSong.isPlaying && !mutedMusic)
                    {
                        shopPanelAudioSourceForSong.Play();
                    }

                    rememberLastPanel = panelNumber;
                    break;
                case 1:
                    if (!mutedAudios)
                    {
                        MuteShopAudios(true);
                        MuteCowPanel(false);
                    }

                    Drugs.onMilkingScreen = true;
                    if (!cowPanelAudioSourceForSong.isPlaying && !mutedMusic)
                    {
                        cowPanelAudioSourceForSong.Play();
                    }

                    shopPanelAudioSourceForSong.Pause();
                    rememberLastPanel = panelNumber;
                    break;
                case 2:
                    Drugs.onMilkingScreen = false;
                    if (!mutedAudios)
                    {
                        MuteShopAudios(false);
                        MuteCowPanel(true);
                    }

                    cowPanelAudioSourceForSong.Pause();
                    if (!shopPanelAudioSourceForSong.isPlaying && !mutedMusic)
                    {
                        shopPanelAudioSourceForSong.Play();
                    }

                    // eventAudioSource.Pause();
                    rememberLastPanel = panelNumber;
                    break;
                case 3:
                    Drugs.onMilkingScreen = false;
                    if (!mutedAudios)
                    {
                        MuteShopAudios(true);
                        MuteCowPanel(true);
                    }

                    if (!mutedMusic)
                    {
                        cowPanelAudioSourceForSong.volume = cowPanelAudioSourceForSong.volume / 2;
                        shopPanelAudioSourceForSong.volume = shopPanelAudioSourceForSong.volume / 2;
                    }

                    break;
            }
        }

        public void PlayLastOne()
        {
            cowPanelAudioSourceForSong.volume = cowPanelAudioSourceForSong.volume * 2;
            shopPanelAudioSourceForSong.volume = shopPanelAudioSourceForSong.volume * 2;
            UpdateAudioMutes(rememberLastPanel);
        }

        private void MuteShopAudios(bool yesNo)
        {
            MuteAudios(yesNo, shopAudios);
        }

        private void MuteSpecialAudios(bool yesNo)
        {
            MuteAudios(yesNo, specialAudios);
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