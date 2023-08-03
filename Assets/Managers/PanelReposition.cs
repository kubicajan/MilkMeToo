using System;
using UnityEngine;

namespace Managers
{
    public class PanelReposition : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject kokPanel;

        private void Start()
        {
            kokPanel.transform.position = new Vector2(-GetCorrectedScreenWidth(), 0);
            shopPanel.transform.position = new Vector2(GetCorrectedScreenWidth(), 0);
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

        private Vector3 Correct(Vector3 vector)
        {
            return Camera.main.ScreenToWorldPoint(vector);
        }
    }
}