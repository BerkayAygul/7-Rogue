using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager SesYoneticisiNesnesiOrnegi;

    public AudioSource LevelMuzigi;
    public AudioSource KaybetmeMuzigi;
    public AudioSource KazanmaMuzigi;
    public AudioSource BossMuzigi;
    public AudioSource MenuMuzigi;

    public AudioSource[] SFXDizisi;
    
    private void Awake()
    {
        SesYoneticisiNesnesiOrnegi = this;
    }

    // Oyuncu oldugu zaman bu muzik calinacagi icin PlayerHealthController.cs'de cagrilacak.

    public void MenuMuziginiCal()
    {
        MenuMuzigi.Play();
        LevelMuzigi.Stop();
    }

    public void LevelMuziginiCal()
    {
        MenuMuzigi.Stop();
        LevelMuzigi.Play();
    }

    public void KaybetmeMuziginiCal()
    {
        MenuMuzigi.Stop();
        LevelMuzigi.Stop();
        KaybetmeMuzigi.Play();
    }

    public void KazanmaMuziginiCal()
    {
        MenuMuzigi.Stop();
        LevelMuzigi.Stop();
        KazanmaMuzigi.Play();
    }

    public void SFXCal(int CalinacakSFXIndexi)
    {
        SFXDizisi[CalinacakSFXIndexi].Stop();
        SFXDizisi[CalinacakSFXIndexi].Play();
    }
}
