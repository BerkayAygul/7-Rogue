using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenterController : MonoBehaviour
{
    public List<GameObject> DusmanlarListesi = new List<GameObject>();

    public RoomController KapiSinifiObjesi;
    public bool DusmanKalmadigindaKapilariAc;
    // Start is called before the first frame update
    void Start()
    {
        if(DusmanKalmadigindaKapilariAc)
        {
            KapiSinifiObjesi.AcilanKapiyiKapat = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DusmanlarListesi.Count > 0 && KapiSinifiObjesi.KapilarAktif && DusmanKalmadigindaKapilariAc)
        {
            for (int i = 0; i < DusmanlarListesi.Count; i++)
            {
                if (DusmanlarListesi[i] == null)
                {
                    // Eger kaldirilmazsa listede null eleman olarak kalirlar.
                    DusmanlarListesi.RemoveAt(i);
                    i--;
                }
            }
        }

        if (DusmanlarListesi.Count == 0)
        {
            KapiSinifiObjesi.KapilariAc();
        }
    }
}
