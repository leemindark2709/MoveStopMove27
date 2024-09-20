using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaintainRotationUiNamePoint : MonoBehaviour
{
    public TextMeshProUGUI namePlayer;
    private Quaternion initialRotation;
    public TextMeshProUGUI point;

    void Start()
    {
        // Lưu lại rotation ban đầu của object
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Đặt lại rotation của object về giá trị ban đầu
        transform.rotation = initialRotation;
    }
}
