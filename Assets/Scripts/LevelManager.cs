using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager LevelYoneticisiNesnesiOrnegi;
    public float SahneYuklemeBeklemeSuresi = 4f;
    public string SonrakiLevelIsmi;

    public bool OyunDurdurulduMu;

    public int AnlikParaMiktari;

    public Transform OyuncuBaslangicNoktasi;
    private void Awake()
    {
        LevelYoneticisiNesnesiOrnegi = this;
    }

    void Start()
    {
        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position = OyuncuBaslangicNoktasi.position;
        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuHareketEdebiliyorMu = true; 
           
        AnlikParaMiktari = CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.AnlikParaMiktari;

        Time.timeScale = 1f;

        UIController.UINesnesiOrnegi.OyuncuParasiText.text = AnlikParaMiktari.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            OyunuDurdurVeyaDevamEttir();
        }
    }

    public IEnumerator LeveliBekleyerekSonlandir()
    {
        AudioManager.SesYoneticisiNesnesiOrnegi.KazanmaMuziginiCal();

        PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuHareketEdebiliyorMu = false;

        UIController.UINesnesiOrnegi.EkraniSoldurmayaBasla();

        yield return new WaitForSeconds(SahneYuklemeBeklemeSuresi);

        CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.AnlikParaMiktari = AnlikParaMiktari;
        CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.AnlikCanSayisi = PlayerHealthController.OyuncuCanNesnesiOrnegi.AnlikCanSayisi;
        CharacterTracker.KarakterTakipEdiciNesnesiOrnegi.MaxCanSayisi = PlayerHealthController.OyuncuCanNesnesiOrnegi.MaxCanSayisi;

        SceneManager.LoadScene(SonrakiLevelIsmi);
    }

    public void OyunuDurdurVeyaDevamEttir()
    {
        if(!OyunDurdurulduMu)
        {
            UIController.UINesnesiOrnegi.DurdurmaEkrani.SetActive(true);
            OyunDurdurulduMu = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.UINesnesiOrnegi.DurdurmaEkrani.SetActive(false);
            OyunDurdurulduMu = false;

            Time.timeScale = 1f;
        }
    }

    public void ParaEldeEt(int ParaMiktariAl)
    {
        AnlikParaMiktari += ParaMiktariAl;

        UIController.UINesnesiOrnegi.OyuncuParasiText.text = AnlikParaMiktari.ToString();
    }

    public void ParaHarca(int ParaMiktariAl)
    {
        AnlikParaMiktari -= ParaMiktariAl;

        if(AnlikParaMiktari <= 0)
        {
            AnlikParaMiktari = 0;
        }

        UIController.UINesnesiOrnegi.OyuncuParasiText.text = AnlikParaMiktari.ToString();
    }
}
