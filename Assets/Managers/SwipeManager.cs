using System;
using System.Collections;
using PopUps;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

namespace Managers
{
    public class SwipeManager : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private Vector3 panelPosition;
        public float percentThreshold = 20f;
        public float easing = 0.25f;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip woosh;
        private int counter = 1;

        private void Awake()
        {
            //to make phone not turn off
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            InformationPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            InitialPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            EventPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            InformationPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            InitialPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            KokTreePopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            EventPopUp.OnShowPopUpTriggered += OnShowPopUpTriggeredHandler;
            panelPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 correctedPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            Vector3 correctedPressPosition = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
            float difference = correctedPressPosition.x - correctedPosition.x;
            float newPosition = panelPosition.x - difference;

            if (!IsOutOfScreen(newPosition))
            {
                transform.position = panelPosition - new Vector3(difference, 0, 0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 correctedPosition = Correct(eventData.position);
            Vector3 correctedPressPosition = Correct(eventData.pressPosition);
            float correctedScreenWidth = GetCorrectedScreenWidth();
            float percentage = (correctedPressPosition.x - correctedPosition.x) / correctedScreenWidth;

            if (Math.Abs(percentage) >= percentThreshold)
            {
                Vector3 newLocation = panelPosition;

                if (percentage > 0)
                {
                    newLocation += new Vector3(-correctedScreenWidth, 0, 0);
                    InitialHanlder.NOW = true;
                    if ((Vector2)panelPosition == new Vector2(0, 0))
                    {
                        InitialHanlder.shopSwipedOnce = true;
                    }
                }
                else if (percentage < 0)
                {
                    newLocation += new Vector3(correctedScreenWidth, 0, 0);
                    if ((Vector2)panelPosition == new Vector2(0, 0))
                    {
                        InitialHanlder.kokTreeSwipedOnce = true;
                    }
                }

                if (!IsOutOfScreen(newLocation.x))
                {
                    panelPosition = newLocation;
                    if ((Vector2)panelPosition == new Vector2(0, 0))
                    {
                        counter = 1;
                    }

                    if (panelPosition.x > 0)
                    {
                        counter = 2;
                    }
                    
                    if (panelPosition.x < 0)
                    {
                        counter = 0;
                    }


                    SongManager.instance.UpdateAudioMutes(counter);
                    StartCoroutine(SmoothMove(transform.position, panelPosition, easing));
                }
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelPosition, easing));
            }
        }

        private Vector3 Correct(Vector3 vector)
        {
            return Camera.main.ScreenToWorldPoint(vector);
        }

        private bool IsOutOfScreen(float position)
        {
            position = RoundNumber(position);
            return position > GetCorrectedScreenWidth() || position < -GetCorrectedScreenWidth();
        }

        private float GetCorrectedScreenWidth()
        {
            Vector3 correctedScreen = Correct(new Vector3(Screen.width, 0, 0));
            return RoundNumber(correctedScreen.x * 2);
        }

        private float RoundNumber(float number)
        {
            return (float)Math.Round(number, 2);
        }

        IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
        {
            audioSource.PlayOneShot(woosh);
            float t = 0f;
            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }

        private void OnShowPopUpTriggeredHandler()
        {
            enabled = false;
        }

        private void OnSetInactiveTriggeredHandler()
        {
            enabled = true;
        }
    }
}