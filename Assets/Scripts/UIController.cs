using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController UINesnesiOrnegi;

    public Slider OyuncuCaniSlider;
    public Text OyuncuCaniText, OyuncuParasiText;

    public GameObject OlumEkrani;

    public Image SolmaEkrani;
    public float SolmaEkraniSolmaHizi;
    private bool EkraniSoldur, EkraniSeffaflastir;

    public string YeniOyunSahneIsmi, AnaMenuSahnesiIsmi;

    public GameObject DurdurmaEkrani;

    public GameObject KucukHaritaEkrani;

    public GameObject BuyukHaritaText;

    public Slider PatronCaniSlider;

    // UI icerisindeki silah resmi ve texti verildikten sonra bunun kontrolu PlayerAttributesController.cs icerisinde yapilacak.
    public Image AnlikSilahResmi;
    public Text AnlikSilahText;
    private void Awake()
    {
        UINesnesiOrnegi = this;
    }

    void Start()
    {
        // Oyun acilirken ekrani siyah yap sonra beyaza çevir.
        EkraniSeffaflastir = true;
        EkraniSoldur = false;

        AnlikSilahResmi.sprite = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.MevcutSilahlarListesi[
                                    PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.AnlikSilahIndexi].SilahUISprite;
        AnlikSilahText.text = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.MevcutSilahlarListesi[
                                    PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.AnlikSilahIndexi].SilahIsmi;
    }

    
    void Update()
    {
        // Renklerin saydamligi (color.a) belirtilen hizla beyaz olacak.
        if(EkraniSeffaflastir == true)
        {
            SolmaEkrani.color = new Color(SolmaEkrani.color.r, SolmaEkrani.color.g, SolmaEkrani.color.b,
                 Mathf.MoveTowards(SolmaEkrani.color.a,0f, SolmaEkraniSolmaHizi * Time.deltaTime));
            if(SolmaEkrani.color.a == 0f)
            {
                EkraniSeffaflastir = false;
            }
        }

        // Renklerin saydamligi (color.a) belirtilen hizla siyah olacak.
        if (EkraniSoldur == true)
        {
            SolmaEkrani.color = new Color(SolmaEkrani.color.r, SolmaEkrani.color.g, SolmaEkrani.color.b,
                 Mathf.MoveTowards(SolmaEkrani.color.a, 1f, SolmaEkraniSolmaHizi * Time.deltaTime));
            if (SolmaEkrani.color.a == 1f)
            {
                EkraniSoldur = false;
            }
        }
    }

    public void EkraniSoldurmayaBasla()
    {
        EkraniSoldur = true;
        EkraniSeffaflastir = false;
    }

    public void YeniOyunBaslat()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(YeniOyunSahneIsmi);

        Destroy(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject);
    }

    public void MenuyeDon()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(AnaMenuSahnesiIsmi);

        Destroy(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject);
    }

    public void OyunaDevamEt()
    {
        LevelManager.LevelYoneticisiNesnesiOrnegi.OyunuDurdurVeyaDevamEttir();
    }
}
