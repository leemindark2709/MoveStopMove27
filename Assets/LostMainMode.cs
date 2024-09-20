using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thêm để sử dụng UI

public class LostMainMode : MonoBehaviour
{
    public Slider slider;
    public int numofspawnHeader;
    private void OnEnable()
    {
        numofspawnHeader = 1;
    }
    private void Update()
    {
        // Đảm bảo rằng NumofEnemy không bằng 0 để tránh lỗi chia cho 0
        if (GameManager.Instance.NumofEnemy > 0&&GameManager.Instance.Armature.GetComponent<PlayerAttack>().isDead&&numofspawnHeader==1)
        {

            float remainingRatio = (GameManager.Instance.NumofEnemy - GameManager.Instance.counyEnemy) / (float)GameManager.Instance.NumofEnemy;
            Debug.Log(remainingRatio);
            slider.value = remainingRatio; // Gán giá trị cho slider
            numofspawnHeader = 0;
        }
        else
        {
            Debug.LogWarning("NumofEnemy is zero, cannot divide!");
        }
    }
}
