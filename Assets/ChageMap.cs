using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageMap : MonoBehaviour
{
    // Start is called before the first frame update

    public void OnButtonClick()
    {
        PlayerPrefs.SetString("IsMap", "Map");
        for (int i = 0; i < GameManager.Instance.ListMaps.Count; i++)
        {

            if (GameManager.Instance.ListMaps[i].name == PlayerPrefs.GetString("IsMap", "Map"))
            {
                PlayerPrefs.SetString("IsMap", GameManager.Instance.ListMaps[i + 1].name);
                GameManager.Instance.NumEnemySpawn = GameManager.Instance.ListMaps[i + 1].GetComponent<Map>().NumOfEnemy;
                GameManager.Instance.counyEnemy = GameManager.Instance.ListMaps[i + 1].GetComponent<Map>().NumOfEnemy;
                GameManager.Instance.numEnemyAlive = 0;
                GameManager.Instance.NumofEnemy = GameManager.Instance.ListMaps[i + 1].GetComponent<Map>().NumOfEnemy;
                GameManager.Instance.WinGame.gameObject.SetActive(false);
                GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = true;
                RestPlayer();
                break;

            }
        }
        for (int i = 0; i < GameManager.Instance.ListMaps.Count; i++)
        {

            if (GameManager.Instance.ListMaps[i].name == PlayerPrefs.GetString("IsMap", "Map"))
            {
                GameManager.Instance.ListMaps[i].gameObject.SetActive(true);
            }
            else
            {
                GameManager.Instance.ListMaps[i].gameObject.SetActive(false);

            }
        }

    }
    public void RestPlayer()
    {
        GameManager.Instance.Armature.localPosition = new Vector3(0, 0, 0);
        GameManager.Instance.Armature.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        GameManager.Instance.PLayer.Find("Canvas").localPosition = new Vector3(0.5f, 0.304f, 0);
        //GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.transform.localPosition = new Vector3(0.05999923f, 0.12f, -0.02399993f);
        GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
        GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius = 0.55f;
        GameManager.Instance.Armature.tag = "Playerr";
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().UIName.GetComponent<MaintainRotationUiNamePoint>().point.text = "0";
        GameManager.Instance.ReturnPositionRankAndSettinglmd();
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().NumOfDead = 1;
        GameManager.Instance.NumOfRevice = 2;
        GameObject.Find("MainCamera").transform.localPosition = new Vector3(5.587935e-08f, 1.689052f, 0.79f);
        GameObject.Find("Player2").transform.localPosition = new Vector3(5.587935e-08f, 0.4990517f, 2.24f);
        GameObject.Find("MainCamera").GetComponent<CameraFollow>().offset.z = -1.45f;
        GameObject.Find("MainCamera").GetComponent<CameraFollow>().offset.y = 1.19f;

    }
}
