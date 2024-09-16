using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1Buy : MonoBehaviour
{
    // Start is called before the first frame update
    public int NumAbilityBottomShield;
    public void OnButtOnClick() {
        PlayerPrefs.SetInt("NumAbilityBottomShield", 0);
    }
}
