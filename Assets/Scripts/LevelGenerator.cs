using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject LayoutOda;
    // Prefab edilmis oda orneginin rengini degistirip gorsel olarak odayi gorebilmek icin yapildi. Alfa = 1, gorunmez.
    public Color BaslangicRengi, BitisRengi, ShopRengi, SandikOdasiRengi;
    // Bitise kadar kac oda olacak.
    public int OlusturulacakOdaSayisi;
    public Transform OdaOlusturmaNoktasi;

    public enum OdaYonu {Yukari, Asagi, Sag, Sol};
    public OdaYonu SeciliOlanYon;

    // Odalarýn birbirlerine olabilecek mesafesine gore yapilir.
    public float XKoordinatiOffset = 18f;
    public float YKoordinatiOffset = 10f;

    // Odanin hangi layer'de oldugunu gostermek icin yapildi, Physics2D.OverlapCircle() icin kullanilacak.
    public LayerMask LayoutOdaLayer;

    // En son oda
    private GameObject BitisOdasi;

    // Oda sayisina gore olusturulacak odalarin dizisi.
    private List<GameObject> LayoutOdaDizisi = new List<GameObject>();

    // Outlinelar bu obje icerisinde duracak.
    public OdaOutlineSinifi OutlineOdalar;

    // Outline olusturduktan sonra bu dizinin icerisine konularak belirtilmeli.
    private List<GameObject> OlusturulanOutlinelarDizisi = new List<GameObject>();

    // Baslangic odasi dusman olmayan bir oda, bitis odasi cikisa sahip bir oda.
    public RoomCenterController BaslangicOdasiMerkezi, BitisOdasiMerkezi, ShopOdasiMerkezi, SandikOdasiMerkezi;
    // Diger oda merkezleri.
    public RoomCenterController[] OlasiOdaMerkezleri;

    private GameObject ShopOdasi;
    public bool ShopEklenilsinMi;
    public int ShopaOlanMinMesafe, ShopaOlanMaxMesafe;

    private GameObject SandikOdasi;
    public bool SandikOdasiEklenilsinMi;
    public int SandikOdasinaOlanMinMesafe, SandikOdasinaOlanMaxMesafe;
    void Start()
    {
        // Ilk odayi olustur ve rengini belirt.
        // Ilk oda ve son oda liste icerisinde olmayacak.
        Instantiate(LayoutOda, OdaOlusturmaNoktasi.position, OdaOlusturmaNoktasi.rotation).GetComponent<SpriteRenderer>().color = BaslangicRengi;
        // Int tipini donusturmek icin type casting kullanilir.
        SeciliOlanYon = (OdaYonu)Random.Range(0, 4);

        // Ilk odayi olusturduktan sonra olusturma noktasini degistir.
        OdaOlusturmaNoktasiniHareketEttir();

        // Belirtilen oda sayisi kadar oda olustur.
        for(int i=0; i<OlusturulacakOdaSayisi; i++)
        {
            // Hangi odanin bitis odasi oldugunu saptamak icin odalari takip etmek gerekiyor.
            GameObject UretilenYeniOda = Instantiate(LayoutOda, OdaOlusturmaNoktasi.position, OdaOlusturmaNoktasi.rotation);

            // Uretilen yeni odalari listeye koy.
            LayoutOdaDizisi.Add(UretilenYeniOda);

            // Son odanin rengini belirtmek icin yapildi.
            if(i+1 == OlusturulacakOdaSayisi)
            {
                UretilenYeniOda.GetComponent<SpriteRenderer>().color = BitisRengi;
                // Dizideki bitis odasini dizi icerisine alma.
                // Bu sayede bitis odasini baslangic odasinin yaninda uretilmemesini sagla.
                // Bitis odasi dizideki en son elemana gore uretilecek.
                LayoutOdaDizisi.RemoveAt(LayoutOdaDizisi.Count-1);
                BitisOdasi = UretilenYeniOda;
            }

            SeciliOlanYon = (OdaYonu)Random.Range(0, 4);
            OdaOlusturmaNoktasiniHareketEttir();

            // Unity fiziðinde uzayda belirli bir noktada cember cizer.
            // Eger baska bir cemberle karsilasilirsa oda noktasini tekrar hareket ettir.
            // Asagi veya yukari git demek yanlis olur, crash sebebi olabilir.
            while (Physics2D.OverlapCircle(OdaOlusturmaNoktasi.position, .2f, LayoutOdaLayer))
            {
                OdaOlusturmaNoktasiniHareketEttir();
            }
        }

        if(ShopEklenilsinMi)
        {
            int ShopIndexi = Random.Range(ShopaOlanMinMesafe, ShopaOlanMaxMesafe + 1);
            ShopOdasi = LayoutOdaDizisi[ShopIndexi];
            LayoutOdaDizisi.RemoveAt(ShopIndexi);
            ShopOdasi.GetComponent<SpriteRenderer>().color = ShopRengi;
        }

        if (SandikOdasiEklenilsinMi)
        {
            int SandikOdasiIndexi = Random.Range(SandikOdasinaOlanMinMesafe, SandikOdasinaOlanMaxMesafe + 1);
            SandikOdasi = LayoutOdaDizisi[SandikOdasiIndexi];
            LayoutOdaDizisi.RemoveAt(SandikOdasiIndexi);
            SandikOdasi.GetComponent<SpriteRenderer>().color = SandikOdasiRengi;
        }

        // Odalar rastgele olusturuluyor ama dis hatlari yok.
        // Baslangic odasi dizi icerisinde tutulmadigi icin baslangic odasina bir dis hat olustur.
        // Her layout icin belirli bir dis hat olustur.
        // Bitis odasi dizi icerisinde tutulmadigi icin bitis odasina bir dis hat olustur.
        OdaOutlineOlustur(Vector3.zero);
        foreach (GameObject SeciliOda in LayoutOdaDizisi)
        {
            OdaOutlineOlustur(SeciliOda.transform.position);
        }
        OdaOutlineOlustur(BitisOdasi.transform.position);

        if(ShopEklenilsinMi)
        {
            OdaOutlineOlustur(ShopOdasi.transform.position);
        }
        if (SandikOdasiEklenilsinMi)
        {
            OdaOutlineOlustur(SandikOdasi.transform.position);
        }

        // Hangi odada uretilecegini belirtmek icin, oda merkezi icerisine uygun odayi verebilmek icin RoomController alinir.
        foreach (GameObject OutlineOda in OlusturulanOutlinelarDizisi)
        {
            bool OdaMerkeziUret = true;
            
            // Baslangic odasinin merkezini koy.
            if(OutlineOda.transform.position == Vector3.zero)
            {
                Instantiate(BaslangicOdasiMerkezi, OutlineOda.transform.position, OutlineOda.transform.rotation).KapiSinifiObjesi =
                OutlineOda.GetComponent<RoomController>();

                OdaMerkeziUret = false;
            }

            // Bitis odasinin merkezini koy.
            if(OutlineOda.transform.position == BitisOdasi.transform.position)
            {
                Instantiate(BitisOdasiMerkezi, OutlineOda.transform.position, OutlineOda.transform.rotation).KapiSinifiObjesi =
                OutlineOda.GetComponent<RoomController>();

                OdaMerkeziUret = false;
            }

            // Shop odasinin merkezini koy.
            if (ShopEklenilsinMi)
            {
                if (OutlineOda.transform.position == ShopOdasi.transform.position)
                {
                    Instantiate(ShopOdasiMerkezi, OutlineOda.transform.position, OutlineOda.transform.rotation).KapiSinifiObjesi =
                    OutlineOda.GetComponent<RoomController>();

                    OdaMerkeziUret = false;
                }
            }
            if (SandikOdasiEklenilsinMi)
            {
                if (OutlineOda.transform.position == SandikOdasi.transform.position)
                {
                    Instantiate(SandikOdasiMerkezi, OutlineOda.transform.position, OutlineOda.transform.rotation).KapiSinifiObjesi =
                    OutlineOda.GetComponent<RoomController>();

                    OdaMerkeziUret = false;
                }
            }


            // Kalan odalarin merkezlerini verilen merkezlere gore rastgele uret.
            if (OdaMerkeziUret)
            {
                int MerkezSecimi = Random.Range(0, OlasiOdaMerkezleri.Length);

                
                Instantiate(OlasiOdaMerkezleri[MerkezSecimi], OutlineOda.transform.position, OutlineOda.transform.rotation).KapiSinifiObjesi =
                    OutlineOda.GetComponent<RoomController>();
            }
        }
    }

    
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void OdaOlusturmaNoktasiniHareketEttir()
    {
        switch (SeciliOlanYon)
        {
            case OdaYonu.Yukari:
                OdaOlusturmaNoktasi.position += new Vector3(0f, YKoordinatiOffset,0f);
                break;
            case OdaYonu.Asagi:
                OdaOlusturmaNoktasi.position += new Vector3(0f, -YKoordinatiOffset, 0f);
                break;
            case OdaYonu.Sag:
                OdaOlusturmaNoktasi.position += new Vector3(XKoordinatiOffset, 0f, 0f);
                break;
            case OdaYonu.Sol:
                OdaOlusturmaNoktasi.position += new Vector3(-XKoordinatiOffset, 0f, 0f);
                break;
        }
    }

    // Oda dis hatlari olustur, daha sonra arayüzden tilemap ekle, tilemap'lere layer ekle, kapýlarý ekle.
    public void OdaOutlineOlustur(Vector3 AlinanOdaOlusturmaNoktasi)
    {
        // Her bool odanin baglantilarini temsil edecek
        bool YukaridaOdaVarMi = Physics2D.OverlapCircle(AlinanOdaOlusturmaNoktasi + new Vector3(0f, YKoordinatiOffset, 0f), .2f, LayoutOdaLayer);
        bool AsagidaOdaVarMi = Physics2D.OverlapCircle(AlinanOdaOlusturmaNoktasi + new Vector3(0f, -YKoordinatiOffset, 0f), .2f, LayoutOdaLayer);
        bool SagdaOdaVarMi = Physics2D.OverlapCircle(AlinanOdaOlusturmaNoktasi + new Vector3(XKoordinatiOffset, 0f, 0f), .2f, LayoutOdaLayer);
        bool SoldaOdaVarMi = Physics2D.OverlapCircle(AlinanOdaOlusturmaNoktasi + new Vector3(-XKoordinatiOffset, 0f, 0f), .2f, LayoutOdaLayer);

        int YonSayisi = 0;

        if(YukaridaOdaVarMi)
        {
            YonSayisi++;
        }
        if (AsagidaOdaVarMi)
        {
            YonSayisi++;
        }
        if (SagdaOdaVarMi)
        {
            YonSayisi++;
        }
        if (SoldaOdaVarMi)
        {
            YonSayisi++;
        }
        // Yon sayisina bak ve bool degiskenlerine gore hangi odalara giris olduguna bak ve outline olustur ve outline dizisi icerisine uygun outline ekle.
        switch (YonSayisi)
        {
            case 0:
                Debug.LogError("Etrafta oda yok!");
                break;
            case 1:
                if(YukaridaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.TekliGirisYukari, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (AsagidaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.TekliGirisAsagi, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.TekliGirisSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (SoldaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.TekliGirisSol, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                break;
            case 2:
                if (YukaridaOdaVarMi && AsagidaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisYukariAsagi, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (YukaridaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisYukariSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if(YukaridaOdaVarMi && SoldaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisYukariSol, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (AsagidaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisAsagiSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (AsagidaOdaVarMi && SoldaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisAsagiSol, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (SoldaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.IkiliGirisSolSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                break;
            case 3:
                if(YukaridaOdaVarMi && AsagidaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.UcluGirisYukariAsagiSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (YukaridaOdaVarMi && AsagidaOdaVarMi && SoldaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.UcluGirisYukariAsagiSol, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (YukaridaOdaVarMi && SoldaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.UcluGirisYukariSolSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                if (AsagidaOdaVarMi && SoldaOdaVarMi && SagdaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.UcluGirisAsagiSolSag, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                break;
            case 4:
                if(YukaridaOdaVarMi && AsagidaOdaVarMi && SagdaOdaVarMi && SoldaOdaVarMi)
                {
                    OlusturulanOutlinelarDizisi.Add(Instantiate(OutlineOdalar.DortluGirisYukariAsagiSagSol, AlinanOdaOlusturmaNoktasi, transform.rotation));
                }
                break;
        }
    }
}

// MonoBehaviour alinmadigi icin manuel olarak serializable yazilmali.
// Oda dis hatlari burada tutulacak.
[System.Serializable]
public class OdaOutlineSinifi
{
    public GameObject TekliGirisYukari, TekliGirisAsagi, TekliGirisSag, TekliGirisSol,
                      IkiliGirisYukariAsagi, IkiliGirisSolSag, IkiliGirisAsagiSag, IkiliGirisAsagiSol, IkiliGirisYukariSol, IkiliGirisYukariSag,
                      UcluGirisYukariAsagiSag, UcluGirisYukariAsagiSol, UcluGirisAsagiSolSag, UcluGirisYukariSolSag,
                      DortluGirisYukariAsagiSagSol;
}

