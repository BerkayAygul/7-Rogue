using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public PickupGun[] KutuIcerisindeOlabilecekSilahlarListesi;
    // Ilk basta kapali sandik sprite renderer alinacakken sandik acilirken acik sandigin sprite'i alinacak.
    public SpriteRenderer AnlikSpriteRenderer; 
    public Sprite AcikSandikSprite;

    public GameObject SandigiAcmaBildirimi;

    private bool SandikAcilabilirMi;
    private bool SandikAcikMi = false;

    public Transform SilahinSandiktanCikmaNoktasi;

    public float SandikHareketEtmeZamani = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        if(SandikAcilabilirMi && !SandikAcikMi)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int SandiktanCikabilecekSilah = Random.Range(0, KutuIcerisindeOlabilecekSilahlarListesi.Length);
                Instantiate(KutuIcerisindeOlabilecekSilahlarListesi[SandiktanCikabilecekSilah],
                    SilahinSandiktanCikmaNoktasi.position, SilahinSandiktanCikmaNoktasi.rotation);

                AnlikSpriteRenderer.sprite = AcikSandikSprite;

                SandikAcikMi = true;

                // Sandik acildiktan sonra sandigin buyumesi yapildi.
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }

        if(SandikAcikMi == true)
        {
            // Sandik acildiktan sonra eski boyutuna belirli bir hizda donsun.
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * SandikHareketEtmeZamani);
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            SandigiAcmaBildirimi.SetActive(true);
            SandikAcilabilirMi = true;
        }
    }

    private void OnTriggerExit2D(Collider2D CarpismaObjesi)
    {
        if(CarpismaObjesi.tag == "Player")
        {
            SandigiAcmaBildirimi.SetActive(false);
            SandikAcilabilirMi = false;
        }
    }
}
