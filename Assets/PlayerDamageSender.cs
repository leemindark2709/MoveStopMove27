using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerDameSender : MonoBehaviour
{
    public bool checkTree = true;
    public Transform targetTree; // Cây mục tiêu cần kiểm tra
    public float timeReturn;
    public bool check = false;
    public Transform enemy;
    public string NameWeapon;
    public string TypeWeapon;

    private static bool isWeaponCollided = false; // Cờ tĩnh để theo dõi va chạm của bất kỳ vũ khí nào

    private void OnTriggerEnter(Collider other)
    {
        if (isWeaponCollided)
        {
            // Nếu vũ khí đã va chạm, không xử lý va chạm nữa
            return;
        }

        // Kiểm tra nếu game object không thuộc cây targetTree thì mới xử lý
        if (!IsChildOf(other.transform, targetTree) && other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            isWeaponCollided = true; // Đánh dấu là đã va chạm

            scoreincrease();

            var otherEnemy = other.transform.parent.GetComponent<EnemyMoving>();
            if (otherEnemy != null)
            {
                Rigidbody rb = other.transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                other.transform.GetComponent<BoxCollider>().enabled = false;

                other.transform.parent.tag = "DieEnemy";
                otherEnemy.transform.Find("Armature").tag = "DieEnemy";
                otherEnemy.isDead = true;
                GameManager.Instance.numEnemyAlive -= 1;
                GameManager.Instance.counyEnemy -= 1;

                GameObject.Find("Num").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.counyEnemy.ToString();

                enemy = other.transform;
            }

            check = true;

            if (targetTree != null)
            {
                targetTree.localScale += new Vector3(0.01f, 0.01f, 0.01f);

                RectTransform circleTransform = targetTree.parent.Find("Canvas").Find("Circle").GetComponent<RectTransform>();
                if (circleTransform != null)
                {
                    circleTransform.sizeDelta *= 1.1f;
                }

                PlayerAttack playerAttack = targetTree.GetComponent<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.detectionRadius += playerAttack.detectionRadius * 0.1f;
                }

                transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

                CameraFollow cameraFollow = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.offset.y += 0.1f;
                    cameraFollow.offset.z -= 0.1f;
                }
            }
        }
        else if (!IsChildOf(other.transform, targetTree) && other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            scoreincrease();
            check = true;
            Destroy(other.gameObject);
            GameManager.Instance.numZombieAlive -= 1;
            GameManager.Instance.counyZombie -= 1;

            GameManager.Instance.Home.GetComponent<Home>().ZombieMode.GetComponent<ZombieMode>().CountZombieAlive.GetComponent<TextMeshProUGUI>().text =
                GameManager.Instance.counyZombie.ToString();
            if (transform.tag == "CopyWeapon")
            {
                Destroy(transform.gameObject);
            }
        }
    }

    // Hàm kiểm tra xem transform có phải là con của cây mục tiêu hay không
    private bool IsChildOf(Transform child, Transform parent)
    {
        if (child == null) return false;
        Transform current = child;
        while (current != null)
        {
            if (current == parent) return true;
            current = current.parent;
        }
        return false;
    }

    private void Update()
    {
        if (targetTree != null)
        {
            timeReturn = targetTree.GetComponent<PlayerAttack>().timeToReturn;
        }

        if (!checkTree)
        {
            StartCoroutine(CheckTreeStatus());
        }
    }

    public void scoreincrease()
    {
        // Tìm TextMeshProUGUI component và cập nhật giá trị của nó
        TextMeshProUGUI textComponent = targetTree.Find("UiNamePoint").Find("UIPoint").Find("Point").GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            float pointValue;
            string pointText = textComponent.text;
            if (float.TryParse(pointText, out pointValue))
            {
                pointValue += 1f; // Thay đổi giá trị tăng tùy theo yêu cầu
                textComponent.text = pointValue.ToString(); // Cập nhật giá trị của Text component
            }
        }
    }

    private IEnumerator CheckTreeStatus()
    {
        // Chờ 2 giây
        yield return new WaitForSeconds(timeReturn);

        // Kiểm tra nếu checkTree là false và targetTree là null thì xoá chính object này
        if (!checkTree && targetTree == null)
        {
            Destroy(gameObject);
        }
    }
}
