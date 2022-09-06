using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesController : MonoBehaviour
{
    public float ParcaninHareketHizi = 3f;
    private Vector3 ParcaninHareketVektoru;

    // Kutularin surekli hareket etmesini engellemek icin yapildi.
    public float ParcaninYavaslatmaHizi = 5f;

    public float ParcalarinYasamSuresi = 3f;

    // Kutu parcalarinin bir anda kaybolmamasi icin yapildi.
    public SpriteRenderer KutuParcalariSpriteRenderer;
    public float KutuParcalarininKaybolmaHizi = 2.5f;

    void Start()
    {
        ParcaninHareketVektoru.x = Random.Range(-ParcaninHareketHizi, ParcaninHareketHizi);
        ParcaninHareketVektoru.y = Random.Range(-ParcaninHareketHizi, ParcaninHareketHizi);
    }


    void Update()
    {
        transform.position += ParcaninHareketVektoru * Time.deltaTime;

        // Lineer interpolasyon. Parcanin hareket hizi sifira yaklasarak azalir.
        ParcaninHareketVektoru = Vector3.Lerp(ParcaninHareketVektoru, Vector3.zero, ParcaninYavaslatmaHizi * Time.deltaTime);

        ParcalarinYasamSuresi -= Time.deltaTime;

        if(ParcalarinYasamSuresi < 0)
        {
            // Mathf(MoveTowards()) metodu baslangic noktasindan belirli bir noktaya gitmek ve orada durmak icin kullanilir.
            KutuParcalariSpriteRenderer.color = new Color(KutuParcalariSpriteRenderer.color.r,
                                                          KutuParcalariSpriteRenderer.color.g,
                                                          KutuParcalariSpriteRenderer.color.b,
                                                          Mathf.MoveTowards
                                                          (KutuParcalariSpriteRenderer.color.a, 0f, KutuParcalarininKaybolmaHizi * Time.deltaTime));
            if(KutuParcalariSpriteRenderer.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
