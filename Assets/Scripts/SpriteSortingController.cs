using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingController : MonoBehaviour
{
    private SpriteRenderer NesneSprite;

    void Start()
    {
        NesneSprite = GetComponent<SpriteRenderer>();

        // Ayni nesneler oyun icerisinde gelisiguzel ust uste geliyorlar.
        // y pozisyonu buyuk olan objelerin Sorting Layer degeri diger objelere gore daha assagida (eksi degerlerde) olacak.
        // Layer degerinin sadece int olmasi gerekirken y degeri noktali degerler alabilecegi icin sayinin yuvarlanmasi gerekir.
        // Degerin -10 ile carpilmasinin nedeni y pozisyonu 0, 0.30, 0.45 olan 3 nesnenin sorting degerinin 0'a yuvarlanmasidir.
        NesneSprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
    }
}
