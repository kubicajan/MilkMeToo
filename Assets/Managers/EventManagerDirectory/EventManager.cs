using System;
using System.Collections;
using PopUps;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] public GameObject eventHolder;

        [SerializeField] private Sprite levelOneSprite;
        [SerializeField] private Sprite levelTwoSprite;

        public static EventManager instance;
        private float timer = 0f;
        private float interval = 3f;
        private float canvasHeight;
        private float canvasWidth;
        private RectTransform canvasRect;
        private bool popUpOpen;
        private bool eventIsShown;
        private Button eventButton;

        private ParticleSystem suckParticleSystem;
        private ParticleSystem voidParticleSystem;
        private ParticleSystem pushParticleSystem;
        private ParticleSystem grassParticleSystem;


        private enum Level
        {
            Zero,
            First,
            Second
        }

        private Level level;

        private void Start()
        {
            eventIsShown = false;
            EventPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            EventPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            popUpOpen = false;
            level = Level.Zero;
            canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            eventButton = GameObject.Find("EventButton").GetComponent<Button>();
            canvasHeight = canvasRect.rect.height;
            canvasWidth = canvasRect.rect.width;

            suckParticleSystem = GameObject.Find("EventSuckParticle").GetComponent<ParticleSystem>();
            voidParticleSystem = GameObject.Find("EventVoidParticle").GetComponent<ParticleSystem>();
            pushParticleSystem = GameObject.Find("EventPushParticle").GetComponent<ParticleSystem>();
            grassParticleSystem = GameObject.Find("EventGrassParticle").GetComponent<ParticleSystem>();

            suckParticleSystem.transform.position = eventHolder.transform.position;
            voidParticleSystem.transform.position = eventHolder.transform.position;
            pushParticleSystem.transform.position = eventHolder.transform.position;
            grassParticleSystem.transform.position = eventHolder.transform.position;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void FixedUpdate()
        {
            if (!eventIsShown && level != Level.Zero && !popUpOpen && IsItTime())
            {
                SpawnEvent();
            }
        }

        public IEnumerator LevelUpCoroutine()
        {
            while (eventIsShown)
            {
                // done to wait until there is no more event, so it would not override.
                yield return null;
            }

            level += 1;
            switch (level)
            {
                case Level.First:
                    SetUpFirstLevel();
                    break;
                case Level.Second:
                    SetUpSecondLevel();
                    break;
                default:
                    break;
            }
        }

        private void SetUpFirstLevel()
        {
            ConfigurePopUp("FIRST_LEVEL", "YO MOMMA");

            eventButton.image.sprite = levelOneSprite;
            suckParticleSystem.gameObject.SetActive(false);
            voidParticleSystem.gameObject.SetActive(false);

            pushParticleSystem.gameObject.SetActive(true);
            grassParticleSystem.gameObject.SetActive(true);
        }

        private void SetUpSecondLevel()
        {
            ConfigurePopUp("SECOND LEVEL", "SMASH OR PASS");
            eventButton.image.sprite = levelTwoSprite;
            suckParticleSystem.gameObject.SetActive(true);
            voidParticleSystem.gameObject.SetActive(true);

            pushParticleSystem.gameObject.SetActive(false);
            grassParticleSystem.gameObject.SetActive(false);
        }

        private void SpawnEvent()
        {
            popUpOpen = true;
            float randomX = Random.Range(-canvasHeight / 2, canvasHeight / 2);
            float randomY = Random.Range(-canvasWidth / 2, canvasWidth / 2);
            var weirdPosition = Camera.main.WorldToViewportPoint(new Vector2(randomX, randomY));
            eventHolder.transform.position = weirdPosition;
            eventHolder.gameObject.SetActive(true);
            eventIsShown = true;
        }

        private bool IsItTime()
        {
            timer += Time.deltaTime;

            if (timer > interval)
            {
                timer = 0;
                return true;
            }

            return false;
        }

        private void ConfigurePopUp(string description, string question)
        {
            EventPopUp.instance.ConfigureFields(description, question);
        }

        private void OnSetInactiveTriggeredHandler()
        {
            eventIsShown = false;
            popUpOpen = false;
        }

        private void OnShowPopUpTriggeredHandler()
        {
            popUpOpen = true;
            eventHolder.gameObject.SetActive(false);
        }
    }
}