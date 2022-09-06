using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    [SerializeField] float MermiHizi = 7.5f;

    public Rigidbody2D MermiBedeni;
    public GameObject MermiCarpmaVFX;

    public int VerilecekHasarSayisi = 25;
    void Update()
    {
        MermiBedeni.velocity = transform.right * MermiHizi;
    }

    // Carpisma aninda dusman classini tasiyan bir nesne arayacagi icin diger nesnelerle olan carpismalarda hata alinir.
    // Bu yuzden tag kullanilir.
    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        Instantiate(MermiCarpmaVFX, transform.position, transform.rotation);
 

        Destroy(gameObject);

        if (CarpismaNesnesi.tag == "Enemy")
        {
            CarpismaNesnesi.GetComponent<EnemyAttributesController>().DusmanaHasarVer(VerilecekHasarSayisi);
        }

        if(CarpismaNesnesi.tag == "Boss")
        {
            CarpismaNesnesi.GetComponent<BossController>().PatronHasarAl(VerilecekHasarSayisi);

            Instantiate(BossController.PatronKontrolcusuNesneOrnegi.PatronHasarAlmaVFX, transform.position,
                transform.rotation);
        }
    }

    private void OnBecameInvisible() // Bu obje ekranin disina cikti.
    {
        Destroy(gameObject);
    }
}
