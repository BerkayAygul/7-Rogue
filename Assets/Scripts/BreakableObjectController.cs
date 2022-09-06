using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectController : MonoBehaviour
{
    public GameObject[] KirilmisKutuParcalari;
    public int AzamiParcaSayisi = 5;

    // Kutular kirilinca icerisinden item cikmasi icin yapildi.
    public bool ItemDusurebilmeliMi;
    public GameObject[] DusurebilecekItemler;
    public float ItemDusurmeYuzdelikKontrolu;

    public int KutuKirilmaSFX = 0;

    /*private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.AtilmaSayaci > 0)
        {
            if (CarpismaNesnesi.tag == "Player")
            {
                Destroy(gameObject);

                int RastgeleParcaSecimi = Random.Range(0, ParcalanmisKutuParcalari.Length);

                Instantiate(ParcalanmisKutuParcalari[RastgeleParcaSecimi], transform.position, transform.rotation);
            }
        }
    }*/

    public void KutuyuKir()
    {
        Destroy(gameObject);

        // Kutu kirilma ses efektini cal.
        AudioManager.SesYoneticisiNesnesiOrnegi.SFXCal(KutuKirilmaSFX);

        int RastgeleDusecekParcaSayisi = Random.Range(1, AzamiParcaSayisi);

        // Dusecek parca sayisini sec ve secilen parcalari tek tek olustur.
        for (int i = 0; i < RastgeleDusecekParcaSayisi; i++)
        {
            int RastgeleParcaSecimi = Random.Range(0, KirilmisKutuParcalari.Length);

            Instantiate(KirilmisKutuParcalari[RastgeleParcaSecimi], transform.position, transform.rotation);
        }

        // Item Dusur
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

    // Oyuncu one atilma hareketini gerceklestirdigi zaman objeyi kir.
    private void OnCollisionEnter2D(Collision2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.gameObject.tag == "Player" &&
             PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.AtilmaSayaci > 0)
        {
            KutuyuKir();
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.gameObject.tag == "Player_Bullet")
        {
            KutuyuKir();
        }
    }
}
