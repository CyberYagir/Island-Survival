using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public void ChangeLang(TMP_Dropdown tmp)
    {
        LangsList.SetLanguage(tmp.value, true);
    }
}
