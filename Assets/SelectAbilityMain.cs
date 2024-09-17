using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAbilityMain : MonoBehaviour
{
    public Transform PanelReadyGoZombie;
    // Start is called before the first frame update
    public void OnButtonClick()
    {
    GameManager.Instance.Armature.GetComponent<PlayerAttack>().NumWeaponCopyZombie = 0;
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().NumShieldZombie = PlayerPrefs.GetInt("NumAbilityBottomShield", 0);
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().NumWeaponCopyZombie = PlayerPrefs.GetInt("NumAbilityBottomMaxWeapon");
        Debug.Log(GameManager.Instance.MainAbility);
        Debug.Log(GameManager.Instance.Home.GetComponent<Home>().PanelStartZombileMode.GetComponent<ZombieMode>().AbilityRandom1.name);
            
        if (GameManager.Instance.MainAbility == "Speed")
        {
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().moveSpeed *= 1.1f;
        }
        GameManager.Instance.MainAbility= GameManager.Instance.Home.GetComponent<Home>().PanelStartZombileMode.
            GetComponent<ZombieMode>().AbilityRandom1.GetComponent<Ability>().NameAbility;
        GameManager.Instance.Home.GetComponent<Home>().AbilityBottomPanel.gameObject.SetActive(false);
        PanelReadyGoZombie.gameObject.SetActive(true);
        //GameManager.Instance.SpawnZombie();
        //GameManager.Instance.SpawnZombie();
        //GameManager.Instance.SpawnZombie();
        GameManager.Instance.Mode = "ZombieCity";
        GameManager.Instance.IsStartZomBie = true;
        GameManager.Instance.Home.GetComponent<Home>().ZombieModePanel.gameObject.SetActive(false);
        GameManager.Instance.Home.GetComponent<Home>().Coin.gameObject.SetActive(false);
        GameManager.Instance.Home.GetComponent<Home>().PauseZombie.gameObject.SetActive(true);
        foreach (Transform zombie in GameManager.Instance.Zombies)
        {
            //zombie.GetComponent<ZombieMoving>().zombieSpeed = 0.5f;
            //zombie.GetComponent<ZombirManager>().Walk();
            //zombie.GetComponent<ZombirManager>().Wait();        
        }
    }
}
