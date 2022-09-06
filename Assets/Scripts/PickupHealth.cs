using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    public int AlinacakSaglikMiktari = 1;

    // Oyuncunun item duser dusmez almamasi icin yapildi.
    public float AlinmayiBeklemeSuresi = .8f;

    public int CanPakediAlmaSFX = 7;

    void Update()
    {
        AlinmayiBeklemeSuresi -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if(CarpismaNesnesi.tag == "Player" && PlayerHealthController.OyuncuCanNesnesiOrnegi.AnlikCanSayisi != 5
           && AlinmayiBeklemeSuresi <= 0)
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyuIyilestir(AlinacakSaglikMiktari);
            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(CanPakediAlmaSFX);

            Destroy(gameObject);
        }
    }
}
