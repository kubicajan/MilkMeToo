using PopUps;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] public GameObject eventHolder;

        public static EventManager instance;
        private float timer = 0f;
        private float interval = 3f;
        private float canvasHeight;
        private float canvasWidth;
        private RectTransform canvasRect;
        private bool popUpOpen;

        private enum Level
        {
            Zero,
            First,
            Second
        }

        private Level level;

        private void Start()
        {
            EventPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            EventPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            popUpOpen = false;
            level = Level.Zero;
            canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            canvasHeight = canvasRect.rect.height;
            canvasWidth = canvasRect.rect.width;
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
            if (!popUpOpen && IsItTime())
            {
                switch (level)
                {
                    case Level.First:
                        HandleFirstLevel();
                        break;
                    case Level.Second:
                        HandleSecondLevel();
                        break;
                    default:
                        break;
                }
            }
        }

        public void LevelUp()
        {
            level += 1;
        }

        private void HandleFirstLevel()
        {
            ConfigurePopUp("FIRST_LEVEL", "YO MOMMA");
            popUpOpen = true;
            float randomX = Random.Range(-canvasHeight / 2, canvasHeight / 2);
            float randomY = Random.Range(-canvasWidth / 2, canvasWidth / 2);
            var gg = Camera.main.WorldToViewportPoint(new Vector2(randomX, randomY));
            eventHolder.transform.position = gg;
            eventHolder.gameObject.SetActive(true);
        }

        private void HandleSecondLevel()
        {
            ConfigurePopUp("SECOND LEVEL", "SMASH OR PASS");
            popUpOpen = true;
            float randomX = Random.Range(-canvasHeight / 2, canvasHeight / 2);
            float randomY = Random.Range(-canvasWidth / 2, canvasWidth / 2);
            var gg = Camera.main.WorldToViewportPoint(new Vector2(randomX, randomY));
            eventHolder.transform.position = gg;
            eventHolder.gameObject.SetActive(true);
            
        }

        public void ClosePopUp()
        {
            popUpOpen = false;
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

        public void ConfigurePopUp(string description, string question)
        {
            EventPopUp.instance.ConfigureFields(description, question);
        }
        
        private void OnSetInactiveTriggeredHandler()
        {
            popUpOpen = false;
        }
        
        private void OnShowPopUpTriggeredHandler()
        {
            popUpOpen = true;
            eventHolder.gameObject.SetActive(false);
        }
    }
}