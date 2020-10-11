using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrenyTest : MonoBehaviour
{
    public TMP_InputField ipf;

    public void DoThing()
    {
        KnightClubAPI.ChangeCurrency(int.Parse(ipf.text), GameManager.user.username);
        ipf.text = "";
    }
}
