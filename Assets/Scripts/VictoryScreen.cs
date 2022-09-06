using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float BirTusaBasilmasiniBekle = 2f;
    public GameObject BirTusaBasmaText;
    public string AnaMenuSahnesiIsmi;

    void Start()
    {
        Time.timeScale = 1f;

        Destroy(PlayerAttributesController.OyuncuNitelikleriNesnesiOrnegi.gameObject);
    }

    void Update()
    {
        if(BirTusaBasilmasiniBekle > 0)
        {
            BirTusaBasilmasiniBekle -= Time.deltaTime;
            if(BirTusaBasilmasiniBekle <=0)
            {
                BirTusaBasmaText.SetActive(true);
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(AnaMenuSahnesiIsmi);
            }
        }
    }
}
