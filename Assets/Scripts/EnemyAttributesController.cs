using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributesController : MonoBehaviour
{
    [Header("Rigidbody Degiskenleri")]
    public Rigidbody2D DusmanBedeni;

    [Header("Genel Degiskenler")]
    public float DusmanHareketHizi;
    public int DusmanCanSayisi = 150;

    [Header("Kovalayici Dusman")]
    public bool DusmanOyunucuKovalamaliMi;
    public float OyuncuyuKovalamaMesafesi;
    private Vector3 DusmanHareketVektoru;

    [Header("Animasyon Degiskenleri")]
    public Animator DusmanAnimasyonKontrolcusu;

    // Birden fazla splatter oldugu icin dizi icerisinde random gonder.
    [Header("VFX Degiskenleri")]
    public GameObject[] DusmanOlmeSplatterVFXDizisi;
    public GameObject DusmanHasarAlmaVFX;

    [Header("Ates Etme Degiskenleri")]
    public bool DusmanAtesEtmeliMi;
    public GameObject DusmanMermisi;
    public Transform MermiyiAteslemeNoktasi;
    public float AtesEtmeSureFarki = 0.5f;
    private float AtesEtmeSayaci;
    public float AtesEtmeMesafesi = 10f;
    
    [Header("Sprite Degiskenleri")]
    public SpriteRenderer DusmanVucuduSprite;

    [Header("SFX Degiskenleri")]
    public int DusmanOlmeSFX = 2;
    public int DusmanMermisiSFX = 12;

    [Header("Kacan Dusman")]
    public bool DusmanKacmaliMi;
    public float DusmanKacmaMesafesi;

    [Header("Gezen Dusman")]
    public bool DusmanGezmeliMi;
    public float GezmeUzunlugu, DurmaUzunlugu;
    private float GezmeSayaci, DurmaSayaci;
    private Vector3 GezmeYonu;

    [Header("Devriyeci Dusman")]
    public bool DusmanDevriyeyeCikmaliMi;
    public Transform[] DevriyeNoktalariDizisi;
    private int AnlikDevriyeNoktasi;

    [Header("Para Dusurme")]
    public bool ItemDusurebilmeliMi;
    public GameObject[] DusurebilecekItemler;
    public float ItemDusurmeYuzdelikKontrolu;

    private void Start()
    {
        if(DusmanGezmeliMi)
        {
            DurmaSayaci = Random.Range(DurmaUzunlugu * 0.75f, DurmaUzunlugu * 1.25f);
        }
    }


    // Dusman oyuncuya belirlenen mesafeden daha yakinsa,
    // Oyuncunun mesafesinden dusmanin mesafesini cikar.
    // Elde edilen vektorun buyuklugunu 1 yap.
    // Dusman bu yonde hareket etsin.
    void Update()
    {
        // Dusmanin vucudu oyuncu tarafindan gorunebiliyorsa hareket, ates gibi eylemleri gerceklestir.
        // Eger oyuncu olduyse bu islemleri yapma.
        if (DusmanVucuduSprite.isVisible && PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject.activeInHierarchy)
        {
            DusmanHareketVektoru = Vector3.zero;
            if (Vector3.Distance(transform.position, PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position) < OyuncuyuKovalamaMesafesi && DusmanOyunucuKovalamaliMi)
            {
                DusmanHareketVektoru = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position - transform.position;

                // Dusmanin bedeninin oyuncunun hareketine gore degismesi.
                if(DusmanHareketVektoru.x > 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else
            {
                // Gezme Sayaci 0'dan buyukse surekli azalt, dusmana hareket yonu vererek gezmesini sagla
                // Gezme Sayaci 0'dan kucukse durma sayacina deger ver
                // Durma Sayaci 0'dan buyukse surekli azalt
                // Durma Sayaci 0'dan kucukse gezme sayaci'na deger ver, dusmana hareket yonu degeri ver.
                if (DusmanGezmeliMi)
                {
                    if (GezmeSayaci > 0)
                    {
                        GezmeSayaci -= Time.deltaTime;

                        DusmanHareketVektoru = GezmeYonu;

                        if (GezmeSayaci <= 0)
                        {
                            DurmaSayaci = Random.Range(DurmaUzunlugu * 0.75f, DurmaUzunlugu * 1.25f);
                        }
                    }
                    if (DurmaSayaci > 0)
                    {
                        DurmaSayaci -= Time.deltaTime;

                        if (DurmaSayaci <= 0)
                        {
                            GezmeSayaci = Random.Range(GezmeUzunlugu * 0.75f, GezmeUzunlugu * 1.25f);

                            GezmeYonu = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }
                // Devriye noktalarinin pozisyonundan dusmanin pozisyonunu cikar, dusman bu sayede o noktaya gitsin.
                if (DusmanDevriyeyeCikmaliMi)
                {
                    DusmanHareketVektoru = DevriyeNoktalariDizisi[AnlikDevriyeNoktasi].position - transform.position;
                    // Eger dusman devriye noktasina cok yaklastiysa o noktaya gitti varsay ve dizideki sonraki noktaya gec.
                    if (Vector3.Distance(transform.position, DevriyeNoktalariDizisi[AnlikDevriyeNoktasi].position) < .2f)
                    {
                        AnlikDevriyeNoktasi++;

                        // Eger dusman son devriye noktasina geldiyse ilk devriye noktasina donsun. 
                        if (AnlikDevriyeNoktasi >= DevriyeNoktalariDizisi.Length)
                        {
                            AnlikDevriyeNoktasi = 0;
                        }
                    }
                }
            }
            
            // Eger oyuncu bu dusmana yakinsa kacsin.
            if (DusmanKacmaliMi && Vector3.Distance(transform.position, PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position) < DusmanKacmaMesafesi)
            {
                DusmanHareketVektoru = transform.position - PlayerAttributesController.
                    OyuncuNitelikleriNesnesiOrnegi.transform.position;
            }
            /*else
            {
                DusmanHareketVektoru = Vector3.zero;
            }*/
            DusmanHareketVektoru.Normalize();

            DusmanBedeni.velocity = DusmanHareketVektoru * DusmanHareketHizi;

            // Oyuncu ile dusman arasindaki mesafe belirlenen ates etme mesafesinden kucukse ates et.
            if (DusmanAtesEtmeliMi && Vector3.Distance(
                transform.position, PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position) 
                < AtesEtmeMesafesi)
            {
                AtesEtmeSayaci -= Time.deltaTime;
                if (AtesEtmeSayaci <= 0)
                {
                    AtesEtmeSayaci = AtesEtmeSureFarki;
                    Instantiate(DusmanMermisi, MermiyiAteslemeNoktasi.position, MermiyiAteslemeNoktasi.transform.rotation);
                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(DusmanMermisiSFX);
                }
            }
        }
        // Oyuncu oldugu zaman hareket etmeye devam ettigi icin dusmanin hizini sifirla.
        else
        {
            DusmanBedeni.velocity = Vector2.zero;
        }

        if (DusmanHareketVektoru != Vector3.zero)
        {
            DusmanAnimasyonKontrolcusu.SetBool("MovingBool", true);
        }
        else
        {
            DusmanAnimasyonKontrolcusu.SetBool("MovingBool", false);
        }

    }

    public void DusmanaHasarVer(int VerilecekHasarSayisi)
    {
        DusmanCanSayisi -= VerilecekHasarSayisi;

        Instantiate(DusmanHasarAlmaVFX, transform.position, transform.rotation);

        if(DusmanCanSayisi <= 0)
        {
            Destroy(gameObject);
            AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(DusmanOlmeSFX);

            // Dusman olunce yere birakilan kalintilar, VFX.
            int SeciliSplatter = Random.Range(0, DusmanOlmeSplatterVFXDizisi.Length);

            // Sayiyi 90 ile carparak daha cok cesitlilik sagla.
            int SplatterRotasyonu = Random.Range(0, 4);

            Instantiate(DusmanOlmeSplatterVFXDizisi[SeciliSplatter], transform.position, Quaternion.Euler(0f,0f, SplatterRotasyonu*90f));

            if (ItemDusurebilmeliMi)
            {
                float ItemDusurmeSansi = Random.Range(0f, 100f);

                if (ItemDusurmeSansi < ItemDusurmeYuzdelikKontrolu)
                {
                    int RastgeleDusenItemIndexi = Random.Range(0, DusurebilecekItemler.Length);
                    Instantiate(DusurebilecekItemler[RastgeleDusenItemIndexi], transform.position, transform.rotation);
                }
            }
        }
    }
}
