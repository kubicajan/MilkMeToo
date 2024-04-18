using System.Collections;
using UnityEngine;

namespace PopUps
{
    public class InitialPopUp : PopUp<InitialPopUp>
    {
        // protected override void Awake()
        // {
        //     base.Awake();
        //     gameObject.SetActive(true);
        //     StartCoroutine(Workaround());
        // }
        //
        // private IEnumerator Workaround()
        // {
        //     yield return new WaitForSeconds(0.1f);
        //     gameObject.transform.position = new Vector2(0, 0);
        //     SetActive();
        // }
    }
}