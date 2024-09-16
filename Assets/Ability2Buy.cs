using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability2Buy : MonoBehaviour
{
    public int NumAbilityBottomSpeed;
    private void Update()
    {
        //if (PlayerPrefs.GetInt("GoldAbilityBottomSpeed") < GameManager.Instance.Gold)
        //{
        //    transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Siver;
        //}
        //if (PlayerPrefs.GetInt("GoldAbilityBottomSpeed") >= GameManager.Instance.Gold)
        //{
        //    transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Green;
        //}
    }
    public void OnClickButton()
    {
        //PlayerPrefs.SetInt("NumAbilityBottomSpeed", 0);
        if (PlayerPrefs.GetInt("GoldAbilityBottomSpeed")>=GameManager.Instance.Gold)
        {
            //PlayerPrefs.SetInt("NumAbilityBottomSpeed", PlayerPrefs.GetInt("NumAbilityBottomSpeed", 0) + 1);
            //PlayerPrefs.SetInt("GoldAbilityBottomSpeed", PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250) * 2);
        }

    }
}
