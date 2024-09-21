using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinMainMode : MonoBehaviour
{
    private void Update()
    {
      
        for (int i = 0; i < GameManager.Instance.ListMaps.Count; i++)
        {
            if (GameManager.Instance.ListMaps[i].name== PlayerPrefs.GetString("IsMap", "Map"))
            {
                transform.GetComponent<TextMeshProUGUI>().text = "Congratulation,you have Unlocked Zone" +(i+1);
            }

        }
    }
}
