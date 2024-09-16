using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability3Buy : MonoBehaviour
{
    public int NumAbilityBottomRange;
    public void OnButtonClick()
    {
        PlayerPrefs.SetInt("NumAbilityBottomRange", 0);
    }
}