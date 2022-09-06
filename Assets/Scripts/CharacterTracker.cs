using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    // Ornegi alinip 1. levele koyulduktan sonra diðer levellere koyulmasýna gerek yok, kod sayesinde nesnenin ornegi   bellekte kaliyor.
    public static CharacterTracker KarakterTakipEdiciNesnesiOrnegi;

    public int AnlikCanSayisi = 5;
    public int MaxCanSayisi = 5;
    public int AnlikParaMiktari = 0;

    // Start is called before the first frame update
    void Awake()
    {
        KarakterTakipEdiciNesnesiOrnegi = this;
    }
}
