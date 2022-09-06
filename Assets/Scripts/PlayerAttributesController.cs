using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributesController : MonoBehaviour
{
    public static PlayerAttributesController OyuncuNitelikleriNesnesiOrnegi; // Dusmanin kovalayabilmesi icin gerekiyor. Awake kullanýlacak.

    [SerializeField] float OyuncuHareketHizi;
    
    private Vector2 KlavyedenGirilenHizVektoru; // wasd
    public Rigidbody2D OyuncuBedeni;
    public Transform OyuncuSilahEli; // Oyuncu silah el objesini buraya ata.
    //private Camera Kamera; // Main kameraya esitle.
    public Animator OyuncuAnimasyonKontrolcusu;

    /* GunController.cs
    public GameObject OyuncuMermisi;
    public Transform MermiyiAteslemeNoktasi;
    public float AtesEtmeSureFarki = 0.2f;
    private float AtesEtmeSayaci;
    */

    // Oyuncu hasar aldiktan sonra gorunmez oldugundaki renk degisimini ayarla.
    public SpriteRenderer OyuncuSprite;

    // Oyuncunun bir yone ani atilma hareketi yapilacak.
    private float OyuncuAnlikHareketHizi;
    public float OyuncuAtilmaHizi = 10f;
    public float OyuncuAtilmaSuresi = 0.5f;
    public float OyuncuAtilmaArasiSureFarki = 5f;
    public float OyuncuAtilmaGorunmezlikSuresi = .5f;

    public int OyuncuAtilmaHareketiSFX = 8;
    /*public int OyuncuMermiSFX = 12;*/

    // Atilma sayaci sayesinde atilma hareketinin ne kadar sure boyunca devam edecegi hesaplanir, deltatime ile bu sure azaltýlýr ve hareket biter.
    [HideInInspector]
    public float AtilmaSayaci;
    // Atilma hareketi bittikten sonra bu sayac calisir ve atilma hareketinin frekansi hesaplanir. Hareket tamamlandiktan sonra calisir.
    [HideInInspector]
    private float AtilmaArasiSureFarkiSayaci;

    // Level bitince oyuncunun hareket edememesi için yapýldý.
    [HideInInspector]
    public bool OyuncuHareketEdebiliyorMu = true;

    public List<GunController> MevcutSilahlarListesi = new List<GunController>();
    [HideInInspector]
    public int AnlikSilahIndexi;

    // Start oncesinde calisir.
    // Bu classin Unity'de mevcut oldugu tum versiyonlar bir ornege esit olacak. 
    private void Awake() 
    {
        OyuncuNitelikleriNesnesiOrnegi = this;

        // Yeni levele gecildiginde oyuncunun bilgilerin tutulmasý icin yapildi.
        // Bunu yapmak sonraki sahnelerde 2 tane oyuncu olmasina neden oluyor. Bu yuzden diger sahnelerde oyuncularin silinmesi lazim.
        // Ayrica oyuncu, son seviyedeki konumuna gore diger seviyeye geciyor, oyuncunun konumunu baslangic odasina gore sifirlamak gerekiyor.
        // Manager objesinin icerisine bir baslangic konumu ayarla.
        // Daha sonra oyuncuyu hareket ettirebilmek icin gerekli olan metotu kullan.
        // Kamerayi null referans olarak aliyor.
        // Kamera = Camera.main kodunu sil, kamera sadece tek bir sahne icin aliniyor.
        // Vector3 EkranPozisyonu kodunu duzelt, CameraController sinifinin ana kamera objesini kullan.
        // Sonraki seviyeye gectigi zaman silahlar tutuluyor ama UI'da yanlis silah ismi gosteriliyor.
        // Start() metotundaki silahýn UI kodlarini UIController icerisine yapistir.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Kamera = Camera.main; // Duz sekilde Kamerayi kullanmak maliyetli oldugu icin baska yontem ile yapilir.


        OyuncuAnlikHareketHizi = OyuncuHareketHizi;

        // Baslangic icin silahlarin icerisindeki sprite ve ismini UI icerisine at. Her silah degistirdiginde bunu gerceklestirmek icin metoda yaz.
        UIController.UINesnesiOrnegi.AnlikSilahResmi.sprite = MevcutSilahlarListesi[AnlikSilahIndexi].SilahUISprite;
        UIController.UINesnesiOrnegi.AnlikSilahText.text = MevcutSilahlarListesi[AnlikSilahIndexi].SilahIsmi;
    }

    void Update()
    {
        if (OyuncuHareketEdebiliyorMu && !LevelManager.LevelYoneticisiNesnesiOrnegi.OyunDurdurulduMu)
        {
            #region Oyuncu Hizi
            KlavyedenGirilenHizVektoru.x = Input.GetAxisRaw("Horizontal"); // Klavye tuslarina basinca yatay hareket al
            KlavyedenGirilenHizVektoru.y = Input.GetAxisRaw("Vertical"); // Klavye tuslarina basinca dikey hareketi al

            // Vektorun buyuklugunu 1 yap.
            // Vektorun dogrultusu ve yonu ayni lakin buyuklugun 1'e esitlenmesi gerekir.
            // Yapilmazsa x ve y eksenleri dogrultusunda ayni anda hareket olmasiyla olagan disi hizda hareketler gorulur.
            KlavyedenGirilenHizVektoru.Normalize();

            /*transform.position += new Vector3(KlavyedenGirilenHizVektoru.x * Time.deltaTime * OyuncuHareketHizi, 
                                              KlavyedenGirilenHizVektoru.y * Time.deltaTime * OyuncuHareketHizi, 0f);*/
            // Ise yaramamasinin sebebi objelere carpmak istemesi, bunun yerine rigidbody'e atama yap

            OyuncuBedeni.velocity = KlavyedenGirilenHizVektoru * OyuncuAnlikHareketHizi; //OyuncuHareketHizi yerine kullanilacak. 
            #endregion

            #region Mouse ve Pozisyon
            Vector3 MousePozisyonu = Input.mousePosition; // Mouse pozisyonunu al. Unity motoru bu pozisyonu sadece oyun icerisinde alýr.
            //Vector3 EkranPozisyonu = Kamera.WorldToScreenPoint(transform.localPosition); // Oyuncunun pozisyonunu normal sahne pozisyonuna aktar
            Vector3 EkranPozisyonu = CameraController.KameraNesnesiOrnegi.AnaKamera.WorldToScreenPoint(transform.localPosition);


            // Vucudu dondurmek icin mouse vucudun solunda mi yoksa sagýnda mi kaldigini bul. Daha sonra bedeni dondur.
            // Silahin rotasyonunu simetri ile ayarla.
            if (MousePozisyonu.x < EkranPozisyonu.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                OyuncuSilahEli.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                OyuncuSilahEli.localScale = Vector3.one;
            }

            // Silahýn rotasyonu formulu. Mouse pozisyonu ve oyuncunun vucut pozisyonu arasindaki tanjant acisini al (y/x)
            // Radyan cinsinden alýnan aciyi normal aciya cevir.
            // Unity, rotasyonlar icin Quaternion(dordey) kullanýr. Rotasyon yapildigi zaman Z ve W eksenleri degisir.
            // Quaternion.Euler() metotu; Z ekseninde rotasyon edilen Z acisini, Y ekseninde rotasyon edilen Y acisini ve ayni sekilde X acisini gonderir.
            // Objeyi istenilen sekilde dondurmek icin ihtiyacýmýz olan sey Z ekseninin acisi.
            Vector2 SilahRotasyonVektoru = new Vector2(MousePozisyonu.x - EkranPozisyonu.x, MousePozisyonu.y - EkranPozisyonu.y);
            float PozisyonlarArasindakiAci = Mathf.Atan2(SilahRotasyonVektoru.y, SilahRotasyonVektoru.x) * Mathf.Rad2Deg;
            OyuncuSilahEli.rotation = Quaternion.Euler(0, 0, PozisyonlarArasindakiAci);
            #endregion

            #region Oyuncu Hiz Animasyonu
            if (KlavyedenGirilenHizVektoru != Vector2.zero)
            {
                OyuncuAnimasyonKontrolcusu.SetBool("MovingBool", true);
            }
            else
            {
                OyuncuAnimasyonKontrolcusu.SetBool("MovingBool", false);
            }
            #endregion

            /* GunController.cs
              #region Oyuncu Ates Etme
            // Sadece sag tarafa gitmemesi icin mermi atesleme noktasi yapildi.
            if (Input.GetMouseButtonDown(0)) // Mouse sol tusa tiklanildigi anda.
            {
                Instantiate(OyuncuMermisi, MermiyiAteslemeNoktasi.position, MermiyiAteslemeNoktasi.rotation);
                AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuMermiSFX);
                AtesEtmeSayaci = AtesEtmeSureFarki; // Basili tutarken ilk ates etmeyi ayarlamak icin.
            }

            if (Input.GetMouseButton(0))  // Mouse sol tusa basili tutarken.
            {
                AtesEtmeSayaci -= Time.deltaTime;

                if (AtesEtmeSayaci <= 0)
                {
                    Instantiate(OyuncuMermisi, MermiyiAteslemeNoktasi.position, MermiyiAteslemeNoktasi.rotation);
                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuMermiSFX);
                    AtesEtmeSayaci = AtesEtmeSureFarki;
                }
            }
            #endregion*/

            #region Oyuncu Atilma Hareketi
            // Space tusuna basildigi zaman sayaclar 0'dan kucukse atilma hareketini yap ve sayaci sifirla.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Space tusuna surekli basildiginda hareketin 1 kez olarak sinirlandirilmasi icin yapildi.
                if (AtilmaArasiSureFarkiSayaci <= 0 && AtilmaSayaci <= 0)
                {
                    OyuncuAnlikHareketHizi = OyuncuAtilmaHizi; // Oyuncunun hareket hizi artacak.
                    AtilmaSayaci = OyuncuAtilmaSuresi; // Atilma hareketinin ne kadar sure boyunca devam edecegini belirle.

                    OyuncuAnimasyonKontrolcusu.SetTrigger("DashTrigger");
                    AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(OyuncuAtilmaHareketiSFX);

                    // Bu classta tanimlanan ve deger verilen gorunmezlik suresini gonder ve oyuncuyu gorunmez yap.
                    PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyuGorunmezYap(OyuncuAtilmaGorunmezlikSuresi);
                }
            }

            if (AtilmaSayaci > 0)
            {
                AtilmaSayaci -= Time.deltaTime;

                if (AtilmaSayaci < 0)
                {
                    // Atilma hareketini durdur, hareket bittikten sonra bir daha ne zaman yapilabilecegini ayarla.
                    OyuncuAnlikHareketHizi = OyuncuHareketHizi;
                    AtilmaArasiSureFarkiSayaci = OyuncuAtilmaArasiSureFarki;
                }
            }

            // Atilma hareketi gerceklestirildikten sonra baska bir sayac ile atilmalar arasindaki sureyi ayarla.
            if (AtilmaArasiSureFarkiSayaci > 0)
            {
                AtilmaArasiSureFarkiSayaci -= Time.deltaTime;
            }
            #endregion


            #region Silah Degistirme
            /* Eger oyuncunun herhangi bir silahi varsa, silah degistirme tusuna basildiginda indexi 1 arttir
             * index = 0, normal silah
             * index = 1, assault
             * index = 2, revolver
             * index = 3 olunca listedeki eleman sayisina esit olacagi icin index degerini = 0 yap ve basa don.
             * Bu olayi her yaptiginda silah degistirme metodunu cagir.
             */
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (MevcutSilahlarListesi.Count > 0)
                {
                    AnlikSilahIndexi++;

                    if (AnlikSilahIndexi >= MevcutSilahlarListesi.Count)
                    {
                        AnlikSilahIndexi = 0;
                    }
                    SilahiDegistir();
                }
                else
                {
                    Debug.LogError("The player has no guns!");
                }
            }
            #endregion
        }

        // Oyuncunun hareket etmesi istenmiyorsa.
        else
        {
           OyuncuBedeni.velocity = Vector2.zero;
           OyuncuAnimasyonKontrolcusu.SetBool("MovingBool", false);
        }

       
    }

    // Her silah objesini devre disi birak daha sonra anlik indexe sahip olan silahi aktif et.
    public void SilahiDegistir()
    {
        foreach (GunController Silah in MevcutSilahlarListesi)
        {
            Silah.gameObject.SetActive(false);
        }
        MevcutSilahlarListesi[AnlikSilahIndexi].gameObject.SetActive(true);

        UIController.UINesnesiOrnegi.AnlikSilahResmi.sprite = MevcutSilahlarListesi[AnlikSilahIndexi].SilahUISprite;
        UIController.UINesnesiOrnegi.AnlikSilahText.text = MevcutSilahlarListesi[AnlikSilahIndexi].SilahIsmi;
    }
}
