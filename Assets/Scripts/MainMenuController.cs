using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string YuklenecekLevelIsmi;

    public GameObject KayitSilmePaneli;

    public CharacterSelector[] SilinecekKarakterlerListesi;

    public void Start()
    {
        AudioManager.SesYoneticisiNesnesiOrnegi.MenuMuziginiCal();  
    }

    public void OyunuBaslat()
    {
        SceneManager.LoadScene(YuklenecekLevelIsmi);
    }

    public void OyundanCik()
    {
        Application.Quit();
    }

    public void KaydiSil()
    {
        KayitSilmePaneli.SetActive(true);
    }

    public void KayitSilmeyiOnayla()
    {
        KayitSilmePaneli.SetActive(false);

        foreach (CharacterSelector Karakter in SilinecekKarakterlerListesi)
        {
            PlayerPrefs.SetInt(Karakter.OlusturulacakKarakter.name, 0);
        }
    }

    public void KayitSilmeyiIptalEt()
    {
        KayitSilmePaneli.SetActive(false);
    }
}
