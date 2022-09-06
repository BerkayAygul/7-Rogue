using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager KarakterSecmeKontrolcusuNesneOrnegi;

    public PlayerAttributesController AktifOyuncu;
    public CharacterSelector AktifSecilenKarakter;

    private void Awake()
    {
        KarakterSecmeKontrolcusuNesneOrnegi = this;
    }
}
