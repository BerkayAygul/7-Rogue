using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController PatronKontrolcusuNesneOrnegi;

    public PatronEylemi[] PatronEylemleriDizisi;

    private int PatronAnlikEylemi;
    private float PatronEylemSayaci;

    private float PatronAtesEtmeSayaci;
    private Vector2 PatronHareketYonu;
    public Rigidbody2D PatronRB;

    public int PatronAnlikCani;

    public GameObject PatronOlmeVFX;
    public GameObject PatronHasarAlmaVFX;
    public int PatronOlmeSFX = 2;
    public int PatronMermisiSFX = 12;
    public GameObject BolumdenCikmaNesnesi;

    public PatronSekansi[] SekanslarDizisi;
    public int AnlikSekans;

    private void Awake()
    {
        PatronKontrolcusuNesneOrnegi = this;
    }

    void Start()
    {
        PatronEylemleriDizisi = SekanslarDizisi[AnlikSekans].PatronEylemleriSekansDizisi;

        PatronEylemSayaci = PatronEylemleriDizisi[PatronAnlikEylemi].PatronEylemUzunlugu;

        UIController.UINesnesiOrnegi.PatronCaniSlider.maxValue = PatronAnlikCani;
        UIController.UINesnesiOrnegi.PatronCaniSlider.value = PatronAnlikCani;
    }

    void Update()
    {

        // Patron hareket eylemi.
        if (PatronEylemSayaci > 0)
        {
            PatronEylemSayaci -= Time.deltaTime;
                
            PatronHareketYonu = Vector2.zero;

            if(PatronEylemleriDizisi[PatronAnlikEylemi].PatronHareketEtmeliMi)
            {
                if(PatronEylemleriDizisi[PatronAnlikEylemi].PatronOyuncuyuKovalamaliMi)
                {
                    PatronHareketYonu = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position -
                                         transform.position;
                    PatronHareketYonu.Normalize();
                }

                if (PatronEylemleriDizisi[PatronAnlikEylemi].PatronNoktalaraHareketEtmeliMi &&
                    Vector3.Distance(transform.position, PatronEylemleriDizisi[PatronAnlikEylemi].PatronHareketNoktasi
                    .position) > 0.5f)
                {
                    PatronHareketYonu = PatronEylemleriDizisi[PatronAnlikEylemi].PatronHareketNoktasi.position -
                        transform.position;
                    PatronHareketYonu.Normalize();
                }
            }

            PatronRB.velocity = PatronHareketYonu * PatronEylemleriDizisi[PatronAnlikEylemi].PatronHareketEtmeHizi;

            // Patron ates etme eylemi.
            if(PatronEylemleriDizisi[PatronAnlikEylemi].PatronAtesEtmeliMi)
            {
                PatronAtesEtmeSayaci -= Time.deltaTime;

                if(PatronAtesEtmeSayaci <= 0)
                {
                    PatronAtesEtmeSayaci = PatronEylemleriDizisi[PatronAnlikEylemi].PatronAtesEtmeSureFarki;

                    foreach (Transform AtesNoktalari in PatronEylemleriDizisi[PatronAnlikEylemi].PatronAtesEtmeNoktalari)
                    {
                        Instantiate(PatronEylemleriDizisi[PatronAnlikEylemi].PatronMermisi, AtesNoktalari.position,
                            AtesNoktalari.rotation);
                        AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(PatronMermisiSFX);
                    }
                }
            }
        }

        else
        {
            PatronAnlikEylemi++;

            if(PatronAnlikEylemi >= PatronEylemleriDizisi.Length)
            {
                PatronAnlikEylemi = 0;
            }

            PatronEylemSayaci = PatronEylemleriDizisi[PatronAnlikEylemi].PatronEylemUzunlugu;


        }
    }

    public void PatronHasarAl(int OyuncuHasarMiktari)
    {
        PatronAnlikCani -= OyuncuHasarMiktari;

        if(PatronAnlikCani <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(PatronOlmeVFX, transform.position, transform.rotation);
            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(PatronOlmeSFX);

            // Oyuncu bolumden cikma nesnesinin hemen ustunde olursa bolum hemen biter, bunu engellemek icin yapildi.
            if (Vector3.Distance(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position,
                                BolumdenCikmaNesnesi.transform.position) < 3f)
            {
                BolumdenCikmaNesnesi.transform.position += new Vector3(4f, 0f, 0f);
            }

            BolumdenCikmaNesnesi.SetActive(true);

            UIController.UINesnesiOrnegi.PatronCaniSlider.gameObject.SetActive(false);
        }
        else
        {
            if(PatronAnlikCani <= SekanslarDizisi[AnlikSekans].PatronSekansiSonlandirmaCani && 
                AnlikSekans < SekanslarDizisi.Length -1)
            {
                AnlikSekans++;
                PatronEylemleriDizisi = SekanslarDizisi[AnlikSekans].PatronEylemleriSekansDizisi;
                PatronAnlikEylemi = 0;
                PatronEylemSayaci = PatronEylemleriDizisi[AnlikSekans].PatronEylemUzunlugu;
            }
        }

        UIController.UINesnesiOrnegi.PatronCaniSlider.value = PatronAnlikCani;
    }
}

[System.Serializable]
public class PatronEylemi
{
    [Header("Eylem")]
    public float PatronEylemUzunlugu;

    public bool PatronHareketEtmeliMi;
    public bool PatronOyuncuyuKovalamaliMi;

    public float PatronHareketEtmeHizi;

    public bool PatronNoktalaraHareketEtmeliMi;
    public Transform PatronHareketNoktasi;

    public bool PatronAtesEtmeliMi;
    public GameObject PatronMermisi;
    public float PatronAtesEtmeSureFarki;
    public Transform[] PatronAtesEtmeNoktalari;

}

[System.Serializable]
public class PatronSekansi
{
    [Header("Sekans")]
    public PatronEylemi[] PatronEylemleriSekansDizisi;

    public int PatronSekansiSonlandirmaCani;
}
