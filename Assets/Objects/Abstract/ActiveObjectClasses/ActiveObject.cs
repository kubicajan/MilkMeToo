using System.Collections;
using Managers;
using PopUps;
using UnityEngine;
using Utilities;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject : KokTreeObject
    {
        [SerializeField] public Sprite shopButtonSprite;
        [SerializeField] public ParticleSystem system;
        [SerializeField] private AudioClip animalNoise;
        [SerializeField] private AudioSource animalNoiseAudioSource;

        private float timer = 0f;
        private double allTimeMilked = 0;

        protected float interval = 1f;
        protected float productionPower = 0;
        protected string description = "";
        protected ParticleSystem milkExplosion;

        protected override void Start()
        {
            base.Start();
            milkExplosion = Instantiate(system, primalSpriteButton.transform);
            milkExplosion.transform.position = primalSpriteButton.transform.position;
            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
        }

        protected void NabijeciSystemTepleVody()
        {
            base.FixedUpdate();
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);
            if (InformationPopUp.instance.isActiveAndEnabled && clickedInfo)
            {
                this.Clicked();
            }
            else
            {
                clickedInfo = false;
            }
        }

        protected override void FixedUpdate()
        {
            NabijeciSystemTepleVody();
            ProduceMilk();
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

        public override void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description, "Amount milked:\n" + allTimeMilked,
                primalSpriteButton.image.sprite, $"{objectCounter}x");
        }

        protected virtual void ProduceMilk()
        {
            if (primalSpriteButton.gameObject.activeSelf)
            {
                if (IsItTime())
                {
                    // float finalPoints = MoneyManagerSingleton.instance.AddMoney(objectCounter * productionPower);
                    // AddToAllTimeMilked(finalPoints);
                    // MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, spriteCanvasPosition);
                    // timer = 0f;
                }
            }
        }

        public virtual void PlayMilked(int? number)
        {
            PlayMilkedNew(primalSpriteButton.gameObject.transform);
        }

        protected void PlayMilkedNew(Transform transformMe)
        {
            ParticleSystem milkExplosion2 = Instantiate(system, transformMe);
            StartCoroutine(PlayMilkedCoroutine(milkExplosion2,  Helpers.GetObjectPositionRelativeToCanvas(transformMe.position)));
        }

        private IEnumerator PlayMilkedCoroutine(ParticleSystem pSystem, Vector2 showMilkPosition)
        {
            yield return new WaitForSeconds(0.25f);
            float finalPoints = MoneyManagerSingleton.instance.AddMoney(objectCounter * productionPower);
            AddToAllTimeMilked(finalPoints);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, showMilkPosition);
            pSystem.Play();
            animalNoiseAudioSource.PlayOneShot(animalNoise);
            // StartCoroutine(PlayNoiseDelayed());
            Destroy(pSystem);
        }

        // protected IEnumerator PlayNoiseDelayed()
        // {
        //     yield return new WaitForSeconds(0.5f);
        //     animalNoiseAudioSource.PlayOneShot(animalNoise);
        // }

        protected void AddToAllTimeMilked(float points)
        {
            allTimeMilked += points; //(float.Parse(allTimeMilked) + points).ToString();
        }
    }
}