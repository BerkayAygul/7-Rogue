using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject OyuncuMermisi;
    public Transform MermiyiAteslemeNoktasi;
    public float AtesEtmeSureFarki = 0.2f;
    private float AtesEtmeSayaci;

    public int OyuncuMermiSFX = 12;

    //UI da silahi gostermek icin yapildi.
    public string SilahIsmi;
    public Sprite SilahUISprite;

    public int ItemMaliyeti;
    public Sprite SilahShopSprite;

    // Update is called once per frame
    void Update()
    {
        // Sadece sag tarafa gitmemesi icin mermi atesleme noktasi yapildi.
        if(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuHareketEdebiliyorMu && !LevelManager.LevelYoneticisiNesnesiOrnegi.OyunDurdurulduMu)
        {
            // Surekli tiklamalarda silahin daha hizli ateslenmesini engellemek icin yapildi.
            if(AtesEtmeSayaci > 0)
            {
                AtesEtmeSayaci -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) // Mouse sol tusa tiklanildigi anda.
                {
                    Instantiate(OyuncuMermisi, MermiyiAteslemeNoktasi.position, MermiyiAteslemeNoktasi.rotation);
                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuMermiSFX);
                    AtesEtmeSayaci = AtesEtmeSureFarki; // Basili tutarken ilk ates etmeyi ayarlamak icin.
                }

                /*if (Input.GetMouseButton(0))  // Mouse sol tusa basili tutarken.
                {
                    AtesEtmeSayaci -= Time.deltaTime;

                    if (AtesEtmeSayaci <= 0)
                    {
                        Instantiate(OyuncuMermisi, MermiyiAteslemeNoktasi.position, MermiyiAteslemeNoktasi.rotation);
                        AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuMermiSFX);
                        AtesEtmeSayaci = AtesEtmeSureFarki;
                    }
                }*/
            }
        }
    }
}
