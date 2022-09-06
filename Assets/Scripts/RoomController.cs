using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public bool AcilanKapiyiKapat;
    // Merkez oda sinifinda kullanilacak
    //public bool DusmanKalmadigindaKapilariAc;

    public GameObject[] KapilarDizisi;
    // Merkez oda sinifinda kullanilacak
    //public List<GameObject> DusmanlarListesi = new List<GameObject>();
    [HideInInspector]
    public bool KapilarAktif;

    // Odanin uzerini kaplayacak, oyuncu odaya girdigi zaman deaktivite olacak.
    public GameObject HaritaGizleyicisi;

    void Update()
    {
        // Merkez oda sinifinda kullanilacak
        /*if(DusmanlarListesi.Count > 0 && KapilarAktif && DusmanKalmadigindaKapilariAc)
        {
            for(int i = 0; i < DusmanlarListesi.Count; i++)
            {
                if (DusmanlarListesi[i] == null)
                {
                    // Eger kaldirilmazsa listede null eleman olarak kalirlar.
                    DusmanlarListesi.RemoveAt(i);
                    i--;
                }
            }
        }

        if(DusmanlarListesi.Count == 0)
        {
            foreach (GameObject Kapi in KapilarDizisi)
            {
                Kapi.SetActive(false);
            }
        }*/
    }

    // Oda merkezi sinifinda kullanilacak
    public void KapilariAc()
    {
        foreach (GameObject Kapi in KapilarDizisi)
        {
            Kapi.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaCismi)
    {
        if (CarpismaCismi.tag == "Player")
        {
            // Oyuncu yeni odaya girdigi zaman siyah ekrani kaldir.
            HaritaGizleyicisi.SetActive(false);

            // Oyuncu yeni odaya girdigi zaman bu scripte sahip olan odanin koordinatlarini gonder.
            CameraController.KameraNesnesiOrnegi.TakipEdilecekHedefiDegistir(transform);

            if (AcilanKapiyiKapat)
            {
                foreach (GameObject Kapi in KapilarDizisi)
                {
                    Kapi.SetActive(true);
                }
            }
        }

        KapilarAktif = true;


    }

    private void OnTriggerExit2D(Collider2D CarpismaCismi)
    {
        if(CarpismaCismi.tag == "Player")
        {
            KapilarAktif = false;
        }
    }

   
}
