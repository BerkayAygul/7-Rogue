using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public GunController AlinacakSilah;
    // Oyuncunun item duser dusmez almamasi icin yapildi.
    public float AlinmayiBeklemeSuresi = .8f;

    public int CanPakediAlmaSFX = 7;

    void Update()
    {
        AlinmayiBeklemeSuresi -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if(CarpismaNesnesi.tag == "Player" && AlinmayiBeklemeSuresi <= 0)
        {
            bool OyuncuBuSilahaSahipMi = false;

            foreach (GunController KontrolEdilecekSilah in PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.MevcutSilahlarListesi)
            {
                if (AlinacakSilah.SilahIsmi == KontrolEdilecekSilah.SilahIsmi)
                {
                    OyuncuBuSilahaSahipMi = true;
                }
            }
            if (OyuncuBuSilahaSahipMi == false)
            {
                GunController OlusturulacakSilah = Instantiate(AlinacakSilah);
                OlusturulacakSilah.transform.parent = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSilahEli;
                OlusturulacakSilah.transform.position = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSilahEli.position;
                OlusturulacakSilah.transform.localRotation = Quaternion.Euler(Vector3.zero);
                OlusturulacakSilah.transform.localScale = Vector3.one;

                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.MevcutSilahlarListesi.Add(OlusturulacakSilah);
                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.AnlikSilahIndexi =
                    PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.MevcutSilahlarListesi.Count - 1;
                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.SilahiDegistir();

            }

            Destroy(gameObject);

            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(7);
        }

    }
}
