using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Dialoguesystem : MonoBehaviour
{
    [SerializeField]
    int dialoguecounter;
    [SerializeField]
    int dialoguen;
    [SerializeField]
    int[] portraitid;
    [SerializeField]
    int[] emotionid;
    [SerializeField]
    string[] dialogues;
    [SerializeField]
    string[] Name;
    [SerializeField]
    bool[] left;

    [SerializeField]
    string enddialogue;
    [SerializeField]
    string endname;
    [SerializeField]
    int endportraitid;
    [SerializeField]
    int endemotionid;


    public TMP_Text dialoguebox;
    public TMP_Text namet;
    public TMP_Text lnamet;
    public GameObject namebox;
    public GameObject lnamebox;
    public Image portrait;
    public Image lportrait;
    public GameObject dialoguer;
    public bool dialoguable;
    public playerscript pl;


    private void Start()
    {
        dialoguecounter = 0;
    }

    public void Begin()
    {
        dialoguer.SetActive(true);
        portrait.sprite = selectsprite(0,0);
    }

    void Update()
    {
        if(dialoguable)
        {
            Time.timeScale = 0;
            if(Input.GetMouseButtonDown(0))
            {
                dialoguecounter++;
            }
            if(dialoguecounter==dialoguen || dialoguecounter==dialoguen+2)
            {
                Exit();
            }
            if(dialoguecounter<=dialoguen-1)
            {
                dialoguebox.text = dialogues[dialoguecounter];
                if (!left[dialoguecounter])
                {
                    namet.text = Name[dialoguecounter];
                    portrait.sprite = selectsprite(portraitid[dialoguecounter], emotionid[dialoguecounter]);
                    portrait.gameObject.SetActive(true);
                    lportrait.gameObject.SetActive(false);
                    namebox.gameObject.SetActive(true);
                    lnamebox.gameObject.SetActive(false);
                }
                else
                {
                    lnamet.text = Name[dialoguecounter];
                    lportrait.sprite = selectsprite(portraitid[dialoguecounter], emotionid[dialoguecounter]);
                    lportrait.gameObject.SetActive(true);
                    portrait.gameObject.SetActive(false);
                    lnamebox.gameObject.SetActive(true);
                    namebox.gameObject.SetActive(false);
                }
            }
            if(dialoguecounter==dialoguen+1)
            {
                dialoguebox.text = enddialogue;
                namet.text = endname;
                portrait.sprite = selectsprite(endportraitid, endemotionid);
            }
        }
    }

    void Exit()
    {
        Time.timeScale = 1;
        dialoguecounter = dialoguen + 1;
        dialoguable = false;
        dialoguer.SetActive(false);
        pl.dialoguing = false;
        pl.batterybar.SetActive(true);
        pl.Pause(false, false,true);
    }

    Sprite selectsprite(int id, int emotion)
    {
        Sprite result;

        if (id == 0)
        {
            result = Resources.Load<Sprite>("Sprites/nothingness");
        }
        else if (id == 1)
        {
            result = Resources.Load<Sprite>("Sprites/Josie/"+emotion);
        }
        else if (id == 2)
        {
            result = Resources.Load<Sprite>("Sprites/Static/"+emotion);
        }
        else
        {
            result = Resources.Load<Sprite>("Sprites/nothingness");
        }

        return result;
    }
}
