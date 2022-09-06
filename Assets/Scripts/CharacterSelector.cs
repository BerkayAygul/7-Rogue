using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private bool KarakterSecilebilirMi;

    public GameObject KarakterSecmeMesaji;

    public PlayerAttributesController OlusturulacakKarakter;

    public bool KarakterAcilmali;

    void Start()
    {
        // Kafesteki karakter acildiysa oyun objesini etkinlestir, karakterlerin sadece 1 kez acilmasini saglar.
        // Sadece acilabilecek karakterler icin bu islemi gerceklestir.
        if(KarakterAcilmali)
        {
            if (PlayerPrefs.HasKey(OlusturulacakKarakter.name))
            {
                if (PlayerPrefs.GetInt(OlusturulacakKarakter.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(KarakterSecilebilirMi)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Sahneye getirelecek yeni oyuncu bu pozisyona gore getirelecek.
                Vector3 OyuncuPozisyonu = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position;

                // Sahneye yeni oyuncu getirilecegi zaman start metotu calisacak ve yeni nesne uretilecek.
                Destroy(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject);

                PlayerAttributesController OlusturulacakYeniOyuncuNesnesi = Instantiate(
                    OlusturulacakKarakter, OyuncuPozisyonu, OlusturulacakKarakter.transform.rotation);

                // Oyuncu sinifinin nesnesine yeni oyuncuyu ver.
                PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi = OlusturulacakYeniOyuncuNesnesi;

                gameObject.SetActive(false);

                CameraController.KameraNesnesiOrnegi.TakipEdilecekHedef = OlusturulacakYeniOyuncuNesnesi.transform;

                // Oyuncuyu olusturduktan sonra aktif oyuncu oldugunu belirt (PlayerAttributesController, test icin).
                // Bu scripte sahip olan karakter seçme objesini aktif et (Character Select - Default oyuncu).

                // Oyuncu baska bir karakter sececegi zaman o karakterin nesnesini belirt (test icin),
                // alinan karakterin karakter secme objesini ustteki kod sayesinde devre disi birak,
                // biraktigi karakterin karakter secme objesini aktif et.
                CharacterSelectManager.KarakterSecmeKontrolcusuNesneOrnegi.AktifOyuncu = OlusturulacakYeniOyuncuNesnesi;
                CharacterSelectManager.KarakterSecmeKontrolcusuNesneOrnegi.AktifSecilenKarakter.gameObject.SetActive(true);
                CharacterSelectManager.KarakterSecmeKontrolcusuNesneOrnegi.AktifSecilenKarakter = this;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            KarakterSecilebilirMi = true;
            KarakterSecmeMesaji.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D CarpismaObjesi)
    {
        if (CarpismaObjesi.tag == "Player")
        {
            KarakterSecilebilirMi = false;
            KarakterSecmeMesaji.SetActive(false);
        }
    }
}
