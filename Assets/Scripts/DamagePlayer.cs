using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if(CarpismaNesnesi.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
    }

    private void OnTriggerStay2D(Collider2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
    }

    private void OnCollisionEnter2D(Collision2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.gameObject.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
    }

    private void OnCollisionStay2D(Collision2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.gameObject.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
    }

    
}
