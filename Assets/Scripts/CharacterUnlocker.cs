using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlocker : MonoBehaviour
{
    private bool KarakterAcilabilirMi;
    public GameObject KarakterAcmaMesaji;

    public CharacterSelector[] SecilebilecekKarakterlerListesi;
    private CharacterSelector AcilacakKarakter;

    public SpriteRenderer KafestekiKarakter;

    void Start()
    {
        AcilacakKarakter = SecilebilecekKarakterlerListesi[Random.Range(0, SecilebilecekKarakterlerListesi.Length)];

        KafestekiKarakter.sprite = AcilacakKarakter.OlusturulacakKarakter.OyuncuSprite.sprite;
    }

    void Update()
    {
        if(KarakterAcilabilirMi)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Acilan karakteri kayit etmek icin yapildi.
                // 0 = Karakter kitli, 1 = Karakter acik.
                PlayerPrefs.SetInt(AcilacakKarakter.OlusturulacakKarakter.name, 1);
                Instantiate(AcilacakKarakter, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            KarakterAcilabilirMi = true;
            KarakterAcmaMesaji.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D CarpismaObjesi)
    {
        if (CarpismaObjesi.tag == "Player")
        {
            KarakterAcilabilirMi = false;
            KarakterAcmaMesaji.SetActive(false);
        }
    }


}
