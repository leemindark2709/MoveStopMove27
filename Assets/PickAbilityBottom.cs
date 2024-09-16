using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickAbilityBottom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string NameAbilityButton;
    // Tham chiếu đến Image cần thay đổi màu sắc
    public Image imageAbility;
    public Sprite Green;
    public Sprite Siver;


    // Màu sắc khi nhấn
    public Color pressedColor = Color.red;

    // Màu sắc mặc định (khi không nhấn)
    private Color originalColor;

    void Start()
    {
        if (imageAbility != null)
        {
            // Lưu lại giá trị màu sắc gốc
            originalColor = imageAbility.color;
        }
    }

    // Hàm sẽ được gọi khi nhấn chuột xuống
    public void OnPointerDown(PointerEventData eventData)
    {
        if (imageAbility != null)
        {
            // Đổi màu khi nhấn nút
            SetImageColor(pressedColor);
            Debug.Log("Button pressed! Image color changed.");
        }
    }

    // Hàm sẽ được gọi khi thả nút chuột
    public void OnPointerUp(PointerEventData eventData)
    {
        if (imageAbility != null)
        {
           
            // Trả lại màu ban đầu khi thả nút
            SetImageColor(originalColor);
            Debug.Log("Button released! Image color reset.");
            GameManager.Instance.NameOfAbilityButtom = NameAbilityButton;
            if(NameAbilityButton=="Speed" && GameManager.Instance.IschoseAbilityButtom == false)
            {
                GameManager.Instance.NameOfAbilityButtom = "Speed";
                GameManager.Instance.PLayer.GetComponent<PlayerMovement>().moveSpeed *= 1.1f;
                GameManager.Instance.Gold -= 1000;
                GameManager.Instance.IschoseAbilityButtom = true;
            }
            if (NameAbilityButton == "Range" && GameManager.Instance.IschoseAbilityButtom == false)
            {
                GameManager.Instance.Gold -= 1000;
                GameManager.Instance.NameOfAbilityButtom = "Range";
                GameManager.Instance.PLayer.Find("Canvas").Find("Circle").localScale *= 1.1f;
                GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius *= 1.1f;
                CameraFollow cameraFollow = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
                cameraFollow.offset.y += 0.1f;
                cameraFollow.offset.z -= 0.1f;
                GameManager.Instance.IschoseAbilityButtom = true;
            }
        }
    }


    // Hàm để thay đổi màu của hình ảnh
    void SetImageColor(Color color)
    {
        imageAbility.color = color;
    }
    private void Update()
    {
        if (GameManager.Instance.IschoseAbilityButtom ==true)
        {
            imageAbility.sprite = Siver;
        }
        else if (GameManager.Instance.Gold>=1000)
        {
            imageAbility.sprite = Green;

        }
       
    }
}
