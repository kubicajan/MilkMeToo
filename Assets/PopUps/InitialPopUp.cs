using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class InitialPopUp : PopUp<InitialPopUp>
    {
        protected override void Awake()
        {
            base.Awake();
            gameObject.transform.position = new Vector2(0, 0);
             SetActive();
        }
    }
}