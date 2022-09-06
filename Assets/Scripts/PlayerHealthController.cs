using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Oyuncu oldugu zaman script'in yok olmamasý için bu obje yapildi. 
    // Baska oyunculara gecislerde script gorevine devam edecek.
    public static PlayerHealthController OyuncuCanNesnesiOrnegi;

    public int AnlikCanSayisi;
    public int MaxCanSayisi = 5;

    public float HasarSonrasiGorunmezlikSuresi = 2f;
    private float GorunmezlikSayaci = 0f;

    public int OyuncuOlmeSFX = 9;
    public int OyuncuHasarAlmaSFX = 11;

    private void Awake()
    {
        OyuncuCanNesnesiOrnegi = this;
    }

    void Start()
    {
        MaxCanSayisi = CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.MaxCanSayisi;
        AnlikCanSayisi = CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.AnlikCanSayisi;


        //AnlikCanSayisi = MaxCanSayisi;

        UIController.UINesnesiOrnegi.OyuncuCaniSlider.maxValue = MaxCanSayisi;
        UIController.UINesnesiOrnegi.OyuncuCaniSlider.value = AnlikCanSayisi;
        UIController.UINesnesiOrnegi.OyuncuCaniText.text = AnlikCanSayisi.ToString() + " / " + MaxCanSayisi.ToString();
    }
    
    void Update()
    {
        // Oyuncunun gorunmez olacagi zamani ayarlamak icin yapildi.
        if(GorunmezlikSayaci > 0)
        {
            GorunmezlikSayaci -= Time.deltaTime;

            if (GorunmezlikSayaci <= 0)
            {
                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color =
                new Color(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.r,
                          PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.g,
                          PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.b,
                          1f);
            }
        }
        
    }

    public void OyuncuyaHasarVer()
    {
        if(GorunmezlikSayaci <= 0)
        {
            AnlikCanSayisi--;

            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuHasarAlmaSFX);

            GorunmezlikSayaci = HasarSonrasiGorunmezlikSuresi;

            PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color =
                new Color(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.r,
                          PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.g,
                          PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.b,
                          .5f);

            if (AnlikCanSayisi <= 0)
            {
                // Oyuncuyu yok etmek yerine oyun objesini deaktive ediyoruz. Bu sayede oyuncu ile alakalý scriptler yok edilmiyor. 
                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject.SetActive(false);

                UIController.UINesnesiOrnegi.OlumEkrani.SetActive(true);

                // Oyuncu oldugunde kaybetme muzigini cal.
                AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuOlmeSFX);
                AudioManager.SesYoneticisiNesnesiOrnegi.KaybetmeMuziginiCal();
            }

            UIController.UINesnesiOrnegi.OyuncuCaniSlider.value = AnlikCanSayisi;
            UIController.UINesnesiOrnegi.OyuncuCaniText.text = AnlikCanSayisi.ToString() + " / " + MaxCanSayisi.ToString();
        }
    }

    public void OyuncuyuGorunmezYap(float GorunmezlikSuresi)
    {
        GorunmezlikSayaci = GorunmezlikSuresi;

        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color =
              new Color(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.r,
                        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.g,
                        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuSprite.color.b,
                        .5f);
    }

    public void OyuncuyuIyilestir(int EklenilecekCanMiktari)
    {
        AnlikCanSayisi += EklenilecekCanMiktari;

        // Oyuncunun can sayisinin 5'den fazla olmamasi icin yapildi.
        if(AnlikCanSayisi > MaxCanSayisi)
        {
            AnlikCanSayisi = MaxCanSayisi;
        }

        UIController.UINesnesiOrnegi.OyuncuCaniSlider.value = AnlikCanSayisi;
        UIController.UINesnesiOrnegi.OyuncuCaniText.text = AnlikCanSayisi.ToString() + " / " + MaxCanSayisi.ToString();
    }

    public void OyuncuMaxCaniniArttir(int GelistirilecekCanMiktari)
    {
        MaxCanSayisi += GelistirilecekCanMiktari;
        AnlikCanSayisi += GelistirilecekCanMiktari;

        UIController.UINesnesiOrnegi.OyuncuCaniSlider.maxValue = MaxCanSayisi;
        UIController.UINesnesiOrnegi.OyuncuCaniSlider.value = AnlikCanSayisi;
        UIController.UINesnesiOrnegi.OyuncuCaniText.text = AnlikCanSayisi.ToString() + " / " + MaxCanSayisi.ToString();

    }
}
