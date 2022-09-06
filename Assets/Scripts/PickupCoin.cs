using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCoin : MonoBehaviour
{
    public int AlinacakParaMiktari = 1;

    // Oyuncunun item duser dusmez almamasi icin yapildi.
    public float AlinmayiBeklemeSuresi = .8f;

    public int ParaAlmaSFX = 5;

    void Update()
    {
        if(AlinmayiBeklemeSuresi > 0)
        {
            AlinmayiBeklemeSuresi -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.tag == "Player" && AlinmayiBeklemeSuresi <= 0)
        {
            LevelManager.LevelYoneticisiNesnesiOrnegi.ParaEldeEt(AlinacakParaMiktari);
            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(ParaAlmaSFX);

            Destroy(gameObject);
        }
    }
}
