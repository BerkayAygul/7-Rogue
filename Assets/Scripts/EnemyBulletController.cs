using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] float MermiHizi = 5f;

    private Vector3 MermiYonu;
    //public Rigidbody2D MermiBedeni;
    //public GameObject MermiCarpmaVFX;

    // Oyuncunun cani 1'er azaldigi icin su an bu degisken kullanilmiyor.
    public int VerilecekHasarSayisi = 25;


    void Start()
    {
        MermiYonu = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position - transform.position;
        MermiYonu.Normalize();
    }


    void Update()
    {
        transform.position += MermiYonu * MermiHizi * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if(CarpismaNesnesi.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
        Destroy(gameObject);
    }

    // Mermi ekran disina ciktiginda
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
