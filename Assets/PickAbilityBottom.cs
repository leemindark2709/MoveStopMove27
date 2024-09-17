using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PickAbilityBottom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string NameAbilityButton;
    // Tham chiếu đến Image cần thay đổi màu sắc
    public Image imageAbility;
    public Sprite Green;
    public Sprite Siver;
    public TextMeshProUGUI text;
    public Vector3 originalCircleScale;
    public float originalDetectionRadius;
    public Vector3 originalCameraOffset;
    public float originalMoveSpeed;
    public Transform ButtonUpSkill;
    public Transform MaxLevel;


    // Màu sắc khi nhấn
    public Color pressedColor = Color.red;

    // Màu sắc mặc định (khi không nhấn)
    private Color originalColor;

    void Start()
    {
        PlayerPrefs.SetInt("NumAbilityBottomRange", 0);
        PlayerPrefs.SetInt("NumAbilityBottomSpeed", 0);
        PlayerPrefs.SetInt("GoldAbilityBottomRange", 250);
        PlayerPrefs.SetInt("GoldAbilityBottomSpeed", 250);
        PlayerPrefs.SetInt("GoldAbilityBottomShield", 1000);
        PlayerPrefs.SetInt("NumAbilityBottomShield", 0);
        PlayerPrefs.SetInt("GoldAbilityBottomMaxWeapon", 1000);
        PlayerPrefs.SetInt("NumAbilityBottomMaxWeapon", 0);
       
        PlayerPrefs.SetInt("CountGold", 2000000);
        if (imageAbility != null)
        {
            // Lưu lại giá trị màu sắc gốc
            originalColor = imageAbility.color;
        }
        originalCircleScale = GameManager.Instance.PLayer.Find("Canvas").Find("Circle").localScale;
        originalDetectionRadius = GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius;
        originalMoveSpeed = GameManager.Instance.PLayer.GetComponent<PlayerMovement>().moveSpeed;
        CameraFollow cameraFollow = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
        originalCameraOffset = cameraFollow.offset;
    }
    public void ResetToOriginalSize()
    {
        // Lấy đối tượng và các thành phần liên quan
        GameObject circle = GameManager.Instance.PLayer.Find("Canvas").Find("Circle").gameObject;
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius = originalDetectionRadius;

        CameraFollow cameraFollow = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
        cameraFollow.offset = originalCameraOffset;

        // Đưa kích thước của Circle về giá trị ban đầu
        circle.transform.localScale = originalCircleScale;
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
        //Debug.Log(PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250));Debug.Log(PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250));
        if (imageAbility != null)
        {
            
            // Trả lại màu ban đầu khi thả nút
            SetImageColor(originalColor);
            Debug.Log("Button released! Image color reset.");
            GameManager.Instance.NameOfAbilityButtom = NameAbilityButton;
            if (NameAbilityButton == "Speed")
            {
                if (PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250) > GameManager.Instance.Gold|| PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250)> 32000)
                {
                    Debug.Log("Not enough gold for Speed ability.");
                    return;  // Dừng lại nếu không đủ vàng
                }

                // Nếu đủ vàng, thực hiện hành động
                GameManager.Instance.Gold -= PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250);
                GameManager.Instance.IschoseAbilityButtom = true;
                PlayerPrefs.SetInt("NumAbilityBottomSpeed", PlayerPrefs.GetInt("NumAbilityBottomSpeed", 0)+1);
                PlayerPrefs.SetInt("GoldAbilityBottomSpeed", PlayerPrefs.GetInt("GoldAbilityBottomSpeed",250)*2);
                IncreaseSpeed(PlayerPrefs.GetInt("NumAbilityBottomSpeed", 0));
                Debug.Log("Speed ability activated.");
            }
            if (NameAbilityButton == "Shield")
            {
                if (PlayerPrefs.GetInt("GoldAbilityBottomShield", 1000) > GameManager.Instance.Gold || PlayerPrefs.GetInt("GoldAbilityBottomShield", 250) > 16000)
                {
                    Debug.Log("Not enough gold for Speed ability.");
                    return;  // Dừng lại nếu không đủ vàng
                }
                GameManager.Instance.Gold -= PlayerPrefs.GetInt("GoldAbilityBottomShield", 1000);
                GameManager.Instance.IschoseAbilityButtom = true;
                PlayerPrefs.SetInt("NumAbilityBottomShield", PlayerPrefs.GetInt("NumAbilityBottomShield", 0) + 1);
                PlayerPrefs.SetInt("GoldAbilityBottomShield", PlayerPrefs.GetInt("GoldAbilityBottomShield", 10000) * 2);
                IncreaseSpeed(PlayerPrefs.GetInt("NumAbilityBottomSpeed", 0));
            }
            // Điều kiện cho "Range"
            if (NameAbilityButton == "Range")
            {
                if (PlayerPrefs.GetInt("GoldAbilityBottomRange", 250) > GameManager.Instance.Gold||PlayerPrefs.GetInt("GoldAbilityBottomRange", 250)> 32000)
                {
              
                    Debug.Log("Not enough gold for Range ability.");
                    return;  // Dừng lại nếu không đủ vàng
                }

                // Nếu đủ vàng, thực hiện hành động
                PlayerPrefs.SetInt("NumAbilityBottomRange", PlayerPrefs.GetInt("NumAbilityBottomRange", 0) + 1);
                Debug.Log(PlayerPrefs.GetInt("NumAbilityBottomRange", PlayerPrefs.GetInt("NumAbilityBottomRange", 0) + 1));
                GameManager.Instance.Gold -= PlayerPrefs.GetInt("GoldAbilityBottomRange", 250);
                GameManager.Instance.NameOfAbilityButtom = "Range";
                GameObject circle = GameManager.Instance.PLayer.Find("Canvas").Find("Circle").gameObject;
                PlayerPrefs.SetInt("GoldAbilityBottomRange", PlayerPrefs.GetInt("GoldAbilityBottomRange", 250)*2);
                // Lấy giá trị NumAbilityBottomRange từ PlayerPrefs
                int numAbility = PlayerPrefs.GetInt("NumAbilityBottomRange", 0);

                // Giá trị tăng cho mỗi lần là 10% giá trị ban đầu
                IncreaseSize(numAbility);
            }
            if (NameAbilityButton == "MaxWeapon")
            {
                if (PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000) > GameManager.Instance.Gold || PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000) > 16000)
                {
                    Debug.Log("Not enough gold for Speed ability.");
                    return;  // Dừng lại nếu không đủ vàng
                }
                GameManager.Instance.Gold -= PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000);
                GameManager.Instance.IschoseAbilityButtom = true;
                PlayerPrefs.SetInt("NumAbilityBottomMaxWeapon", PlayerPrefs.GetInt("NumAbilityBottomMaxWeapon", 0) + 1);
                PlayerPrefs.SetInt("GoldAbilityBottomMaxWeapon", PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 10000) * 2);
            }
        }
    }


    // Hàm để thay đổi màu của hình ảnh
    void SetImageColor(Color color)
    {
        imageAbility.color = color;
    }
    /// <summary>
    /// //////////////Set ve ban dau
    /// </summary>
    /// <param name="numAbility"></param>
    /// 


    /////////////////tăng tốc độ 
    public void IncreaseSpeed(int numAbility)
    {
        float speedFactor = 1 + numAbility * 0.1f;
        GameManager.Instance.PLayer.GetComponent<PlayerMovement>().moveSpeed = originalMoveSpeed * speedFactor;
        Debug.Log("Increased speed based on numAbility.");
    }

    // Hàm để reset tốc độ về giá trị ban đầu
    public void ResetSpeedToOriginal()
    {
        GameManager.Instance.PLayer.GetComponent<PlayerMovement>().moveSpeed = originalMoveSpeed;
        Debug.Log("Speed reset to original value.");
    }


    ///////////////////////tăng kích thước //////////////////////
    public void IncreaseSize(int numAbility)
    {
        // Tính toán hệ số dựa trên số lần sử dụng (1 + 0.1 * numAbility)
        float scaleFactor = 1 + numAbility * 0.1f;

        // Thay đổi kích thước của Circle dựa trên giá trị ban đầu
        GameObject circle = GameManager.Instance.PLayer.Find("Canvas").Find("Circle").gameObject;
        circle.transform.localScale = originalCircleScale * scaleFactor;

        // Thay đổi bán kính phát hiện của PlayerAttack dựa trên giá trị ban đầu
        GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius = originalDetectionRadius * scaleFactor;

        // Thay đổi offset của Camera dựa trên giá trị ban đầu
        CameraFollow cameraFollow = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
        cameraFollow.offset = originalCameraOffset + new Vector3(0, numAbility * 0.1f, -numAbility * 0.1f);

        Debug.Log("Increased size based on numAbility.");
    }

    private void Update()
    {


        //////////////Ability1//////////////////////////////////////////////////
        if (NameAbilityButton == "Speed" &&PlayerPrefs.GetInt("GoldAbilityBottomSpeed",250) <=GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Green;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250).ToString();
        }
        if (NameAbilityButton == "Speed" && PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250) > GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Siver;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250).ToString();
        }
        if (NameAbilityButton == "Speed" && PlayerPrefs.GetInt("GoldAbilityBottomSpeed", 250) >= 32000)
        {
            ButtonUpSkill.gameObject.SetActive(false);
            MaxLevel.gameObject.SetActive(true);
        }
        //////////////Ability2//////////////////////////////////////////////////
        Debug.Log(PlayerPrefs.GetInt("GoldAbilityBottomRange", 250));

        if (NameAbilityButton == "Range" &&PlayerPrefs.GetInt("GoldAbilityBottomRange",250) <=GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Green;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomRange", 250).ToString();
        }
        if (NameAbilityButton == "Range" && PlayerPrefs.GetInt("GoldAbilityBottomRange", 250) > GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Siver;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomRange", 250).ToString();
            Debug.Log(PlayerPrefs.GetInt("GoldAbilityBottomRange", 250));
            if (NameAbilityButton == "Range" && PlayerPrefs.GetInt("GoldAbilityBottomRange", 250) >= 32000)
            {
             
                ButtonUpSkill.gameObject.SetActive(false);
                MaxLevel.gameObject.SetActive(true);
            }
        }
        //////////////Ability3//////////////////////////////////////////////////

        if (NameAbilityButton == "Shield" &&PlayerPrefs.GetInt("GoldAbilityBottomShield",1000) <=GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Green;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomShield", 1000).ToString();
        }
        if (NameAbilityButton == "Shield" && PlayerPrefs.GetInt("GoldAbilityBottomShield", 1000) > GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Siver;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomShield", 1000).ToString();
        }
        if (NameAbilityButton == "Shield" && PlayerPrefs.GetInt("GoldAbilityBottomShield", 250) >= 16000)
        {
            ButtonUpSkill.gameObject.SetActive(false);
            MaxLevel.gameObject.SetActive(true);
        }
        //////////////Ability4//////////////////////////////////////////////////
        if (NameAbilityButton == "MaxWeapon" && PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000) <= GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Green;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000).ToString();
        }
        if (NameAbilityButton == "MaxWeapon" && PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000) > GameManager.Instance.Gold)
        {
            transform.GetComponent<PickAbilityBottom>().imageAbility.GetComponent<Image>().sprite = transform.GetComponent<PickAbilityBottom>().Siver;
            text.text = PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 1000).ToString();
        }
        if (NameAbilityButton == "MaxWeapon" && PlayerPrefs.GetInt("GoldAbilityBottomMaxWeapon", 250) >= 16000)
        {
            ButtonUpSkill.gameObject.SetActive(false);
            MaxLevel.gameObject.SetActive(true);
        }

    }
}
