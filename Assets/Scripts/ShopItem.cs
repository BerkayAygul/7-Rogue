using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject SatinAlmaMesaji;

    private bool SatinAlmaAlaninaDokunuluyorMu;

    public bool CanYenilemeSatinAlmaAlani, CanGelistirmeAlani, SilahSatinAlmaAlani;

    public int ItemMaliyeti;

    public int CanGelistirmeMiktari;

    public GunController[] SatinAlinabilecekSilahlarListesi;
    private GunController AlinacakSilah;
    public SpriteRenderer AlinacakSilahSpriteRenderer;
    public Text SilahBilgisiText;

    private void Start()
    {
        if(SilahSatinAlmaAlani)
        {
            int SeciliSilah = Random.Range(0, SatinAlinabilecekSilahlarListesi.Length);
            AlinacakSilah = SatinAlinabilecekSilahlarListesi[SeciliSilah];

            AlinacakSilahSpriteRenderer.sprite = AlinacakSilah.SilahShopSprite;
            SilahBilgisiText.text = AlinacakSilah.SilahIsmi + "\n - " + AlinacakSilah.ItemMaliyeti + " Gold - ";
            ItemMaliyeti = AlinacakSilah.ItemMaliyeti;
        }
    }

    private void Update()
    {
        if(SatinAlmaAlaninaDokunuluyorMu)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.LevelYoneticisiNesnesiOrnegi.AnlikParaMiktari >= ItemMaliyeti)
                {
                    LevelManager.LevelYoneticisiNesnesiOrnegi.ParaHarca(ItemMaliyeti);

                    if(CanYenilemeSatinAlmaAlani && (PlayerHealthController.OyuncuCanNesnesiOrnegi.AnlikCanSayisi != 
                       PlayerHealthController.OyuncuCanNesnesiOrnegi.MaxCanSayisi))
                    {
                            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyuIyilestir(
                            PlayerHealthController.OyuncuCanNesnesiOrnegi.MaxCanSayisi);
                    }
                    if(CanGelistirmeAlani)
                    {
                        PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuMaxCaniniArttir(CanGelistirmeMiktari);
                    }
                    if(SilahSatinAlmaAlani)
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

                    gameObject.SetActive(false);
                    SatinAlmaAlaninaDokunuluyorMu = false;

                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(18);
                }
                else
                {
                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(19);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            SatinAlmaMesaji.SetActive(true);
            SatinAlmaAlaninaDokunuluyorMu = true;
        }
    }

    private void OnTriggerExit2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            SatinAlmaMesaji.SetActive(false);
            SatinAlmaAlaninaDokunuluyorMu = false;  
        }
    }
}
