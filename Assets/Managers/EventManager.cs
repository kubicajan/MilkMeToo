using System.Collections;
using PopUps;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private GameObject eventHolder;
        [SerializeField] private Sprite levelOneSprite;
        [SerializeField] private Sprite levelTwoSprite;

        public static EventManager instance;
        private RectTransform canvasRect;
        private Button eventButton;
        private float timer = 0f;
        private float interval = 3f;
        private float canvasHeight;
        private float canvasWidth;
        private bool popUpOpen;
        private bool eventIsShown;

        private GameObject suckParticleSystem;
        private GameObject voidParticleSystem;
        private GameObject pushParticleSystem;
        private GameObject grassParticleSystem;

        private Level level;

        private enum Level
        {
            Zero,
            First,
            Second
        }

        private void Start()
        {
            level = Level.Zero;
            popUpOpen = false;
            eventIsShown = false;
            EventPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            EventPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            eventButton = GameObject.Find("EventButton").GetComponent<Button>();
            canvasHeight = canvasRect.rect.height;
            canvasWidth = canvasRect.rect.width;
            FirstSetupParticleSystems();
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

        private void FirstSetupParticleSystems()
        {
            suckParticleSystem = GameObject.Find("EventSuckParticle");
            voidParticleSystem = GameObject.Find("EventVoidParticle");
            pushParticleSystem = GameObject.Find("EventPushParticle");
            grassParticleSystem = GameObject.Find("EventGrassParticle");

            Vector3 holderPosition = eventHolder.transform.position;
            suckParticleSystem.gameObject.transform.position = holderPosition;
            voidParticleSystem.transform.position = holderPosition;
            pushParticleSystem.transform.position = holderPosition;
            grassParticleSystem.transform.position = holderPosition;
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

        private void SpawnEvent()
        {
            float randomX = Random.Range(-canvasHeight / 2, canvasHeight / 2);
            float randomY = Random.Range(-canvasWidth / 2, canvasWidth / 2);
            var weirdPosition = Camera.main.WorldToViewportPoint(new Vector2(randomX, randomY));
            eventHolder.transform.position = weirdPosition;
            eventHolder.gameObject.SetActive(true);
            popUpOpen = true;
            eventIsShown = true;
        }

        private void SetUpFirstLevel()
        {
            eventButton.image.sprite = levelOneSprite;
            TurnOnLevelTwoParticles(false);
            TurnOnLevelOneParticles(true);
        }

        private void SetUpSecondLevel()
        {
            eventButton.image.sprite = levelTwoSprite;
            TurnOnLevelOneParticles(false);
            TurnOnLevelTwoParticles(true);
        }

        private void TurnOnLevelOneParticles(bool switchTo)
        {
            pushParticleSystem.SetActive(switchTo);
            grassParticleSystem.SetActive(switchTo);
        }

        private void TurnOnLevelTwoParticles(bool switchTo)
        {
            suckParticleSystem.SetActive(switchTo);
            voidParticleSystem.SetActive(switchTo);
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