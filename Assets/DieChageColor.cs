using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieChangeColor : MonoBehaviour
{
    public Material material; // Chọn material từ Inspector
    public Material material2;
    public float darkenAmount = 0.2f; // Giá trị để làm tối (0 - 1)
    public Transform initialShadingGroup1;
    public Transform Pants;
    private Color originalColor; // Lưu trữ màu gốc
    private Color originalColor2; // Lưu trữ màu gốc

    private void Start()
    {
        // Lấy material từ SkinnedMeshRenderer
        var renderer1 = initialShadingGroup1.GetComponent<SkinnedMeshRenderer>();
        var renderer2 = Pants.GetComponent<SkinnedMeshRenderer>();

        if (renderer1 != null && renderer1.materials.Length > 0)
        {
            material = renderer1.materials[0];
            originalColor = material.color; // Lưu màu gốc
        }

        if (renderer2 != null && renderer2.materials.Length > 0)
        {
            material2 = renderer2.materials[0];
            originalColor2 = material2.color; // Lưu màu gốc
        }
    }

    // Hàm làm tối màu
    public void Darken()
    {
        if (material != null)
        {
            Color darkenedColor = originalColor * (1 - darkenAmount); // Tính màu tối hơn
            material.color = darkenedColor; // Gán màu mới cho material
        }

        if (material2 != null)
        {
            Color darkenedColor2 = originalColor2 * (1 - darkenAmount); // Tính màu tối hơn
            material2.color = darkenedColor2; // Gán màu mới cho material
        }
    }

    // Hàm reset màu về màu gốc
    public void ResetColor()
    {
        if (material != null)
        {
            material.color = originalColor; // Gán lại màu gốc
        }

        if (material2 != null)
        {
            material2.color = originalColor2; // Gán lại màu gốc
        }
    }
}
