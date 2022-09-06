using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    [SerializeField] float MermiHizi = 5f;

    private Vector3 MermiYonu;

    // Oyuncunun cani 1'er azaldigi icin su an bu degisken kullanilmiyor.
    public int VerilecekHasarSayisi = 25;


    void Start()
    {
        //MermiYonu = PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.transform.position - transform.position;
        //MermiYonu.Normalize();
        MermiYonu = transform.right;
    }


    void Update()
    {
        transform.position += MermiYonu * MermiHizi * Time.deltaTime;

        if(!BossController.PatronKontrolcusuNesneOrnegi.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D CarpismaNesnesi)
    {
        if (CarpismaNesnesi.tag == "Player")
        {
            PlayerHealthController.OyuncuCanNesnesiOrnegi.OyuncuyaHasarVer();
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
