using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Kameranin bu sekilde uygulanmasinin sebebi, oyuncunun girecegi oda degisecegi zaman kameranin o odaya odaklanmasini saglamak.  

    public static CameraController KameraNesnesiOrnegi;
    public float HareketHizi;
    public Transform TakipEdilecekHedef;

    public Camera AnaKamera, BuyukHaritaKamerasi;

    private bool BuyukHaritaKamerasiAktifMi;

    public bool PatronOdasiMi;

    void Awake()
    {
        KameraNesnesiOrnegi = this;   
    }

    void Start()
    {
        if(PatronOdasiMi)
        {
            TakipEdilecekHedef = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform;
        }
        
    }

    void Update()
    {
        if(TakipEdilecekHedef != null)
        {
            // Kamerayi takip edilecek hedefe hareket ettir.
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(TakipEdilecekHedef.position.x, TakipEdilecekHedef.position.y, -10),
                HareketHizi * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.M) && !PatronOdasiMi)
        {
            if(BuyukHaritaKamerasiAktifMi == false)
            {
                BuyukHaritaKamerasiniAktiflestir();
            }
            else
            {
                BuyukHaritaKamerasiniDeaktiveEt();
            }

        }
            
    }

    public void TakipEdilecekHedefiDegistir(Transform YeniHedef)
    {
        // RoomController sinifinda kullanilacak.
        TakipEdilecekHedef = YeniHedef;
    }

    public void BuyukHaritaKamerasiniAktiflestir()
    {
        // Oyunu durdurup buyuk haritayi acinca timescale tekrar 1f oluyor.
        if(LevelManager.LevelYoneticisiNesnesiOrnegi.OyunDurdurulduMu == false)
        {
            BuyukHaritaKamerasiAktifMi = true;
            // Objenin kendisi yerine kamerayi ac veya kapat.
            BuyukHaritaKamerasi.enabled = true;
            AnaKamera.enabled = false;

            PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuHareketEdebiliyorMu = false;
            Time.timeScale = 0f;

            // Buyuk harita acildiginda kucuk haritayi kaldirmak icin raw image devredisi birak.
            UIController.UINesnesiOrnegi.KucukHaritaEkrani.SetActive(false);
            UIController.UINesnesiOrnegi.BuyukHaritaText.SetActive(true);
        }

    }

    public void BuyukHaritaKamerasiniDeaktiveEt()
    {
        if(LevelManager.LevelYoneticisiNesnesiOrnegi.OyunDurdurulduMu == false)
        {
            BuyukHaritaKamerasiAktifMi = false;
            BuyukHaritaKamerasi.enabled = false;
            AnaKamera.enabled = true;

            PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.OyuncuHareketEdebiliyorMu = true;
            Time.timeScale = 1f;

            UIController.UINesnesiOrnegi.KucukHaritaEkrani.SetActive(true);
            UIController.UINesnesiOrnegi.BuyukHaritaText.SetActive(false);
        }
    }
}
