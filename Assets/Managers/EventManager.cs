using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] public Button eventButton;

        public static EventManager instance;
        private float timer = 0f;
        private float interval = 3f;
        private float canvasHeight;
        private float canvasWidth;
        private ParticleSystem suckParticles;
        private ParticleSystem voidParticles;
        private RectTransform canvasRect;

        private enum Level
        {
            Zero,
            First,
            Second
        }

        private Level level;

        private void Start()
        {
            level = Level.Zero;
            canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            canvasHeight = canvasRect.rect.height;
            canvasWidth = canvasRect.rect.width;
            suckParticles = GameObject.Find("EventSuckParticle").GetComponent<ParticleSystem>();
            voidParticles = GameObject.Find("EventVoidParticle").GetComponent<ParticleSystem>();
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
            if (IsItTime())
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
            float randomX = Random.Range(-canvasHeight / 2, canvasHeight / 2);
            float randomY = Random.Range(-canvasWidth / 2, canvasWidth / 2);
            eventButton.gameObject.SetActive(true);
            eventButton.interactable = true;
            
            //todo: this is not ideal
            var gg = Camera.main.WorldToViewportPoint(new Vector2(randomX, randomY));

            eventButton.transform.position = gg;

            suckParticles.transform.SetParent(eventButton.gameObject.transform);
            suckParticles.transform.position = gg;
            voidParticles.transform.SetParent(eventButton.gameObject.transform);
            voidParticles.transform.position = gg;

            suckParticles.gameObject.SetActive(true);
            voidParticles.gameObject.SetActive(true);
            suckParticles.Play();
            voidParticles.Play();

            Debug.Log("FIRST");
        }

        private void HandleSecondLevel()
        {
            Debug.Log("SECOND");
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
    }
}