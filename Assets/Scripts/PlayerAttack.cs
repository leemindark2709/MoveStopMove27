using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{   public static PlayerAttack instance;
    public Animator anim;
    public bool isDead=false;
    public bool End=false;
    public ParticleSystem deathParticles;
    public bool IsRotate=false;
    public int numOfAttacks = 1; // Số lượng tấn công
    public float detectionRadius = 0.1f; // Bán kính phát hiện
    public Transform weapon; // Đối tượng vũ khí
   [SerializeField] public GameObject enemy; // Kẻ địch gần nhất
    private Transform originalWeaponParent; // Vị trí ban đầu của vũ khí
    public float timeToReturn; // Thời gian để vũ khí trở về
    public Transform enemyTarget; // Mục tiêu của kẻ địch
    public int NumOfDead;
    public List<GameObject> enemys;
    public Vector3 direction;
    private Coroutine returnCoroutine; // Tham chiếu đến coroutine trở về
    public float torqueAmount = 10;
    public Vector3 localPosition;
    public Quaternion localRotation ;
    [SerializeField]public Vector3 localScale;
    public bool isCollidingWithWall = false;
    public bool CanAttack;
    public GameObject weaponClone;
    public Transform UIName;
    public Transform ShieldAbility;
    public int NumShieldZombie;
    public int NumWeaponCopyZombie;
    public int NumWeaponCopyZombyInGame=0;
    public int NumWeaponReal=1;
    public int LevelUpPlayer = 1;
    public Transform UIPointObject;
    public int UiPoint;
    private void Awake()
    {
        instance = this;
    }
 

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("va cham");
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = false;
        }
    }

    // Optional: Method to get the collision status
    public bool IsCollidingWithWall()
    {
        return isCollidingWithWall;
    }
    void Start()
    {

        weapon.GetComponent<BoxCollider>().enabled = false;
        localRotation = weapon.localRotation;
        localPosition = weapon.localPosition;
        localScale = weapon.localScale;
        NumOfDead = 1;
        anim = GetComponent<Animator>(); // Lấy component Animator
        //weapon = FindDeepChild(transform, "Hammer"); // Tìm đối tượng vũ khí "Hammer"
        if (weapon == null)
        {
            Debug.LogWarning("Không tìm thấy vũ khí 'Hammer'.");
        }
        else
        {
            originalWeaponParent = weapon.parent; // Lưu trữ vị trí ban đầu của vũ khí
        }
    }
    public void UseAbilityShield()
    {
        transform.tag = "PlayerWithShield";
        ShieldAbility.gameObject.SetActive(true);
        StartCoroutine(WaitCancelShield());

    }
    IEnumerator WaitCancelShield()
    {
       
        yield return new WaitForSeconds(2f); // Đợi 2 giây
        NumShieldZombie -= 1;
        transform.tag = "Playerr";
        ShieldAbility.gameObject.SetActive(false);

    }
    public void EndAbility4()
    {
        NumWeaponReal = 1;
        NumWeaponCopyZombyInGame = 0;

    }

    void Update()
    {
        UiPoint = int.Parse(UIPointObject.GetComponent<TextMeshProUGUI>().text);

        //localScale = weapon.localScale;
        if (GetNthNumberInSequence(NumWeaponReal) ==GameManager.Instance.NumZomBieStart - GameManager.Instance.counyZombie)
        {
            Debug.Log("NumAbilityBottomMaxWeapon: " + PlayerPrefs.GetInt("NumAbilityBottomMaxWeapon", 0));
            if (NumWeaponReal <= PlayerPrefs.GetInt("NumAbilityBottomMaxWeapon",0))
            {
                NumWeaponCopyZombyInGame = NumWeaponReal;
                NumWeaponReal += 1;
            }
          
        }
        if (isDead)
        {
            GameManager.Instance.Armature.GetComponent<DieChangeColor>().Darken();
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isInteracting = false;
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isMoving = false;
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().direction= new Vector2(0,0);

            GameManager.Instance.Home.GetComponent<Home>().PanelEndGameZombie.GetComponent<PanelEndGameZombie>().GoldX3.text =
         ((GameManager.Instance.NumZomBieStart - GameManager.Instance.counyZombie)*3).ToString();   
            GameManager.Instance.Home.GetComponent<Home>().PanelEndGameZombie.GetComponent<PanelEndGameZombie>().Gold.text =
         (GameManager.Instance.NumZomBieStart - GameManager.Instance.counyZombie).ToString();  
            GameManager.Instance.Home.GetComponent<Home>().PanelWinGameZombie.GetComponent<PanelEndGameZombie>().GoldX3.text =
         ((GameManager.Instance.NumZomBieStart - GameManager.Instance.counyZombie)*3).ToString();   
            GameManager.Instance.Home.GetComponent<Home>().PanelWinGameZombie.GetComponent<PanelEndGameZombie>().Gold.text =
         (GameManager.Instance.NumZomBieStart - GameManager.Instance.counyZombie).ToString();
            //weapon.GetComponent<Rigidbody>().isKinematic = true;
            transform.gameObject.GetComponent<PlayerAttack>().enabled= false;
            //deathParticles.transform.position = transform.position;
            deathParticles.Play(); // Chạy Particle System

        }
        if (GameManager.Instance.counyEnemy==1)
        {
            GameManager.Instance.MovePositionRankAndSettinglmd();
            GameManager.Instance.WinGame.gameObject.SetActive(true);
            NumOfDead = 0; GameManager.Instance.TouchToContinue.Find("Canvas").Find("PanelRank").Find("Top").GetComponent<TextMeshProUGUI>().text = "#" + (GameManager.Instance.counyEnemy).ToString();
            GameManager.Instance.NumOfRevice -= 1;

            //PlayerAttack.instance.End = false;

            //transform.gameObject.SetActive(false);
            GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isInteracting = false;
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isMoving = false;
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().direction = new Vector2(0, 0);
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = false;
            GameManager.Instance.Armature.GetComponent<PlayerAttack>().anim.Play("Idel");


        }
        if (GameManager.Instance.counyZombie==0&&GameManager.Instance.Mode=="ZombieCity")
        {
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isInteracting = false;
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().isMoving = false;
            PlayerPrefs.SetString("Complete", "Yes");
            PlayerPrefs.Save();
            GameManager.Instance.Home.GetComponent<Home>().updateDay();
            GameManager.Instance.MovePositionUIZombieModelmd();
            GameManager.Instance.Home.GetComponent<Home>().PanelWinGameZombie.gameObject.SetActive(true);
            NumOfDead = 0;
            GameManager.Instance.Home.GetComponent<Home>().WinZombieMode.text = "YOU SURVIVED DAY " + (PlayerPrefs.GetInt("IsDay", 1)+1).ToString() + "!";  
            GameManager.Instance.NumOfRevice -= 1;

            //PlayerAttack.instance.End = false;
           
            //transform.gameObject.SetActive(false);
            GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = false;
            GameManager.Instance.Armature.GetComponent<PlayerAttack>().anim.Play("Idel");


        }
        if (isDead && NumOfDead == 1 && GameManager.Instance.NumOfRevice > 1 && GameManager.Instance.Mode != "ZombieCity")
        {
           
            GameManager.Instance.EndGame = true;
            Debug.Log("HereHere");
            NumOfDead = 0;
            GameManager.Instance.TouchToContinue.Find("Canvas").Find("PanelRank").Find("Top").GetComponent<TextMeshProUGUI>().text = "#" + (GameManager.Instance.counyEnemy).ToString();
            GameManager.Instance.NumOfRevice -= 1;

            //PlayerAttack.instance.End = false;
            anim.Play("Dead");
            //transform.gameObject.SetActive(false);
            GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = false;
            //GameManager.Instance.PLayer.GetComponent<PlayerAttack>().enabled = false;
        }
        if (isDead && NumOfDead == 1 && GameManager.Instance.NumOfRevice > 1&&GameManager.Instance.Mode== "ZombieCity")
        {
            Debug.Log("HereHere");
            
            GameManager.Instance.EndGame = true;
        
            NumOfDead = 0;

            GameManager.Instance.NumOfRevice -= 1;

            //PlayerAttack.instance.End = false;
            //anim.Play("Dead");
            //transform.gameObject.SetActive(false);
            GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = false;
            //GameManager.Instance.PLayer.GetComponent<PlayerAttack>().enabled = false
            GameManager.Instance.Home.GetComponent<Home>().updateDay();

        }
        if (isDead &&NumOfDead==1&& GameManager.Instance.NumOfRevice <=1)
        {
            if ( GameManager.Instance.Mode != "ZombieCity")
            {
          
                GameManager.Instance.MovePositionRankAndSettinglmd();
                GameManager.Instance.TouchToContinue.gameObject.SetActive(true);
                NumOfDead = 0; 
                GameManager.Instance.TouchToContinue.Find("Canvas").Find("PanelRank").Find("Top").GetComponent<TextMeshProUGUI>().text = "#" + (GameManager.Instance.counyEnemy).ToString();
                GameManager.Instance.NumOfRevice -= 1;

                //PlayerAttack.instance.End = false;
                anim.Play("Dead");
                //transform.gameObject.SetActive(false);
                GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
                GameManager.Instance.PLayer.GetComponent<PlayerMovement>().enabled = false;
            }
            else
            {
             
                PlayerPrefs.SetString("Complete","Yes");
                GameManager.Instance.Home.GetComponent<Home>().PanelEndGameZombie.gameObject.SetActive(true);
                NumOfDead = 0; 
                //GameManager.Instance.MovePositionUIZombieModelmd();
                GameManager.Instance.NumOfRevice -= 1;
                GameManager.Instance.PLayer.Find("Armature").tag = "DeadPlayer";
            }

            //GameManager.Instance.PLayer.GetComponent<PlayerAttack>().enabled = false;

        }
        enemy = CheckEnemy(); // Kiểm tra kẻ địch gần nhất

        if (!isDead&&  enemy != null && numOfAttacks > 0 && !PlayerMovement.instance.isMoving&&enemy.tag!="EnemyDie"&&CanAttack)
        {
            //CheckEnemy().transform.parent.Find("Canvas").Find("IsCheckEnemy").GetComponent<Image>().enabled = true;
            numOfAttacks = 0;
            Vector3 direction = (enemy.transform.position - weapon.position).normalized;
            Attack(direction); // Tấn công kẻ địch
          
            
            Debug.Log("Kẻ địch gần, tấn công");
        }
    }
    public int GetNthNumberInSequence(int n)
    {
        if (n == 1) return 8; // Số thứ 1 là 8
        int a_n = 8;
        int difference = 22;

        for (int i = 2; i <= n; i++)
        {
            a_n += difference;
            difference += 8; // Tăng khoảng cách thêm 8 mỗi lần
        }

        return a_n;
    }
    public GameObject CheckEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all objects tagged "Enemy"
       
        GameObject closestEnemy = null; // Closest enemy
        float closestDistance = detectionRadius; // Initialize closest distance with detection radius

        foreach (GameObject enemy in enemies)
        {
            // Check if the enemy has a Collider component
            if (enemy.GetComponent<Collider>() != null)
            {
                float distance = Vector3.Distance(transform.Find("Armature").position, enemy.transform.position); // Calculate distance
                if (distance <= detectionRadius && distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }
        }

        if (closestEnemy != null)
        {
            //Debug.Log("Found the closest enemy with a collider");
        }

        return closestEnemy;

    }

    public void Attack(Vector3 direction)
    {
        
        if (weapon == null)
        {

            Debug.LogWarning("Vũ khí chưa được gán.");
            return;
        }
        //if (enemy == null)
        //{
        //    numOfAttacks = 1;
        //    return;
        //}
       
        if (transform.GetComponent<UltimateChek>().HaveUltimate)
        {
            anim.SetFloat("AttackUltimate", 1);
            StartCoroutine(DelayedAttack(0.4f, direction)); // T    ạo độ trễ khi tấn công
        }
        else
        {

            anim.SetFloat("attack", 1); // Play attack animation
            StartCoroutine(DelayedAttack(0.35f, direction)); // T    ạo độ trễ khi tấn công

        }



    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(1f);
        CanAttack = true;
    }
    private IEnumerator WaitAttackZomie()
    {
        yield return new WaitForSeconds(0.1f);
        CanAttack = true;
    }

    private IEnumerator DelayedAttack(float delay,Vector3 direction)
    {
        //if (enemy == null)
        //{
        //    Debug.LogWarning("Kẻ địch không còn tồn tại");
        //    numOfAttacks = 1;
        //    yield break;
        //}

        enemyTarget = enemy.transform; // Lưu mục tiêu của kẻ địch

        //Vector3 direction = (enemy.transform.position - weapon.position).normalized; // Hướng tấn công
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Xoay để hướng về kẻ địch
        transform.rotation = lookRotation;

        // Bắt đầu thời gian trì hoãn trước khi tấn công
        float elapsedDelay = 0f;
        while (elapsedDelay < delay)
        {
            // Kiểm tra nếu nhân vật đang di chuyển trong thời gian chờ
            if (PlayerMovement.instance.isMoving)
            {
                if (transform.GetComponent<UltimateChek>().HaveUltimate)
                {
                    Debug.Log("Tấn công bị hủy do nhân vật đang di chuyển");
                    anim.SetFloat("AttackUltimate", 0);
                    anim.SetFloat("AttackUltilMoving", 1); // Đặt lại trạng thái hoạt ảnh tấn công

                    numOfAttacks = 1; // Đặt lại số lần tấn công
                }
                else
                {
                    Debug.Log("Tấn công bị hủy do nhân vật đang di chuyển");
                    anim.SetFloat("attack", 0);
                    anim.SetFloat("attackMoving", 1); // Đặt lại trạng thái hoạt ảnh tấn công

                    numOfAttacks = 1; // Đặt lại số lần tấn công
                }
              
                yield break; // Thoát khỏi coroutine
            }

            elapsedDelay += Time.deltaTime;
            yield return null;
        }


        //if (GameManager.Instance.Armature.GetComponent<PlayerAttack>().enemy != null  )
        //{
            //if (GameManager.Instance.Armature.GetComponent<PlayerAttack>().enemy.tag != "DieEnemy")
            //{
                PerformAttack(direction); // Thực hiện tấn công
            //}
           
        //}
        //else
        //{

        //    if (transform.GetComponent<UltimateChek>().HaveUltimate)
        //    {
             
        //        anim.SetFloat("AttackUltimate", 0);
              

        //        numOfAttacks = 1; // Đặt lại số lần tấn công
        //    }
        //    else
        //    {
        //        anim.SetFloat("attack", 0);
        //        numOfAttacks = 1;
            
        //    }


        
        //}
    }



    private void PerformAttack(Vector3 direction)
    {
        //if (enemyTarget == null) return;

        // Lấy Rigidbody của Weapon
        Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
        if (weaponRb != null)
        {
   
            string weaponType = weapon.gameObject.GetComponent<PlayerDameSender>().TypeWeapon;

            if (weaponType == "Hammer")
            {
                localRotation = weapon.localRotation; // Lưu trữ góc quay hiện tại của vũ khí
                //Vector3 direction = (enemyTarget.position - weapon.position).normalized;
                direction.y = 0.016f;

                if (weaponRb != null)
                {
                    float forceMagnitude = 0.85f; // Lực tác động lên vũ khí

                    if (!transform.GetComponent<UltimateChek>().HaveUltimate)
                    {
                        localPosition = weapon.localPosition; // Lưu trữ vị trí hiện tại của vũ khí
                        //localScale = weapon.localScale;
                        weapon.parent = null; // Đặt parent của vũ khí thành null

                        weaponRb.isKinematic = false; // Đặt isKinematic của Rigidbody thành false
                        weaponRb.velocity = Vector3.zero; // Đặt vận tốc thành 0
                        weaponRb.angularVelocity = Vector3.zero; // Đặt vận tốc góc thành 0

                         forceMagnitude = 0.85f; // Lực tác động lên vũ khí

                        float distance = detectionRadius; // Khoảng cách tấn công
                        float weaponSpeed = forceMagnitude; // Tốc độ vũ khí
                        timeToReturn = distance / weaponSpeed; // Tính thời gian trở về

                        weaponRb.AddForce(direction * forceMagnitude, ForceMode.Impulse); // Tác động lực lên vũ khí
                        weapon.gameObject.layer = LayerMask.NameToLayer("Default");

                        weapon.GetComponent<BoxCollider>().enabled = true;
                        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                        weapon.rotation = targetRotation;
                        weapon.Rotate(-90, 0, 0);
                    }
                    else
                    {
                        float distance = detectionRadius; // Khoảng cách tấn công
                        float weaponSpeed = forceMagnitude; // Tốc độ vũ khí
                        timeToReturn = distance / weaponSpeed; // Tính thời gian trở về
                        //StartCoroutine(ScaleWeaponOverTime(weapon, 2, timeToReturn)); // Tăng kích thước lên gấp 2 lần trong khoảng thời gian timeToReturn
                        localPosition = weapon.localPosition; // Lưu trữ vị trí hiện tại của vũ khí
                        localScale = weapon.localScale;
                        weapon.parent = null; // Đặt parent của vũ khí thành null

                        weaponRb.isKinematic = false; // Đặt isKinematic của Rigidbody thành false
                        weaponRb.velocity = Vector3.zero; // Đặt vận tốc thành 0
                        weaponRb.angularVelocity = Vector3.zero; // Đặt vận tốc góc thành 0

                        forceMagnitude = 1.25f; // Lực tác động lên vũ khí

                        weaponRb.AddForce(direction * forceMagnitude, ForceMode.Impulse); // Tác động lực lên vũ khí
                     
                        StartScaling(weapon.gameObject, timeToReturn);
                        weapon.gameObject.layer = LayerMask.NameToLayer("Default");

                        weapon.GetComponent<BoxCollider>().enabled = true;
                        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                        weapon.rotation = targetRotation;
                        weapon.Rotate(-90, 0, 0);
                    }


                    if (GameManager.Instance.MainAbility == "3Weapon")
                    {
                        // Tạo bản sao của vũ khí
                        GameObject WeaponCopy1 = Instantiate(weapon.gameObject);
                        GameObject WeaponCopy2 = Instantiate(weapon.gameObject);
                        WeaponCopy1.tag = "CopyWeapon";
                        WeaponCopy2.tag = "CopyWeapon";

                        // Áp dụng góc quay ban đầu của vũ khí gốc cho các bản sao
                        WeaponCopy1.transform.rotation = weapon.rotation;
                        WeaponCopy2.transform.rotation = weapon.rotation;

                        // Tính toán hướng gốc
                        Vector3 originalDirection = direction;

                        // Tính toán góc ném lệch trái (-45 độ)
                        Vector3 leftDirection = Quaternion.Euler(0, -85, 0) * originalDirection;

                        // Tính toán góc ném lệch phải (+45 độ)
                        Vector3 rightDirection = Quaternion.Euler(0, 85, 0) * originalDirection;

                        // Tác động lực lên WeaponCopy1 với góc ném lệch trái
                        WeaponCopy1.GetComponent<Rigidbody>().AddForce(leftDirection * forceMagnitude, ForceMode.Impulse);
                        StartCoroutine(RotateCopyWeaponAroundYAxis(timeToReturn - 0.12f, WeaponCopy1.transform));
                        WeaponCopy1.layer = LayerMask.NameToLayer("Default");
                        WeaponCopy1.GetComponent<BoxCollider>().enabled = true;

                        // Tác động lực lên WeaponCopy2 với góc ném lệch phải
                        WeaponCopy2.GetComponent<Rigidbody>().AddForce(rightDirection * forceMagnitude, ForceMode.Impulse);
                        StartCoroutine(RotateCopyWeaponAroundYAxis(timeToReturn - 0.12f, WeaponCopy2.transform));
                        WeaponCopy2.layer = LayerMask.NameToLayer("Default");
                        WeaponCopy2.GetComponent<BoxCollider>().enabled = true;

                        // Gọi coroutine để xóa các bản sao sau timeToReturn
                        StartCoroutine(DestroyAfterTime(WeaponCopy1, timeToReturn));
                        StartCoroutine(DestroyAfterTime(WeaponCopy2, timeToReturn));
                    }
                    Ability4(direction);

                    if (!transform.GetComponent<UltimateChek>().HaveUltimate)
                    {
                        StartCoroutine(RotateWeaponAroundYAxis(timeToReturn - 0.12f, weapon)); // Thực hiện quay quanh trục Y
                    }
               
                }
                StartCoroutine(WaitForAttackToEnd());
            }

            else if (weaponType == "Knife")
            {
                // Cấu hình và tấn công với Knife
                //Vector3 direction = (enemyTarget.position - weapon.position).normalized;
                direction.y = 0.016f;

                weapon.parent = null;
                weapon.gameObject.GetComponent<PlayerDameSender>().checkTree = false;

                weaponRb.isKinematic = false;
                weaponRb.velocity = Vector3.zero;
                weaponRb.angularVelocity = Vector3.zero;

                float forceMagnitude = 0.87f;
                float distance = Vector3.Distance(weapon.position, enemyTarget.position);
                float weaponSpeed = forceMagnitude;
                timeToReturn = distance / weaponSpeed;

                weaponRb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
                weapon.gameObject.layer = LayerMask.NameToLayer("Default");

                // Xoay vũ khí hướng về mục tiêu
                Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
                weapon.rotation = targetRotation;
                weapon.Rotate(90, 0, 0);

                weapon.GetComponent<BoxCollider>().enabled = true;
            }
         
            // Bắt đầu Coroutine để vũ khí quay về vị trí ban đầu sau khi tấn công
            StartCoroutine(ReturnToParentAfterDelay(timeToReturn, localPosition, localRotation, localScale));
        }
    }
    public void Ability4(Vector3 direction)
    {
        for (int i = 0; i < NumWeaponCopyZombyInGame; i++)
        {
            if (i % 2 == 0)
            {
                // Tạo bản sao của vũ khí
                GameObject WeaponCopy1 = Instantiate(weapon.gameObject);
                //GameObject WeaponCopy2 = Instantiate(weapon.gameObject);
                WeaponCopy1.tag = "CopyWeapon";
                //WeaponCopy2.tag = "CopyWeapon";

                // Áp dụng góc quay ban đầu của vũ khí gốc cho các bản sao
                WeaponCopy1.transform.rotation = weapon.rotation;
                /*  WeaponCopy2.transform.rotation = weapon.rotation*/
                ;

                // Tính toán hướng gốc
                Vector3 originalDirection = direction;

                // Tính toán góc ném lệch trái (-45 độ)
                Vector3 leftDirection = Quaternion.Euler(0, -30f * (i / 2 + 1), 0) * originalDirection;

                // Tính toán góc ném lệch phải (+45 độ)
                //Vector3 rightDirection = Quaternion.Euler(0, 85, 0) * originalDirection;

                // Tác động lực lên WeaponCopy1 với góc ném lệch trái
                WeaponCopy1.GetComponent<Rigidbody>().AddForce(leftDirection * 0.85f, ForceMode.Impulse);
                StartCoroutine(RotateCopyWeaponAroundYAxis(timeToReturn - 0.12f, WeaponCopy1.transform));
                WeaponCopy1.layer = LayerMask.NameToLayer("Default");
                WeaponCopy1.GetComponent<BoxCollider>().enabled = true;
                StartCoroutine(DestroyAfterTime(WeaponCopy1, timeToReturn));
            }
            else
            {
                // Tạo bản sao của vũ khí
                GameObject WeaponCopy1 = Instantiate(weapon.gameObject);
                //GameObject WeaponCopy2 = Instantiate(weapon.gameObject);
                WeaponCopy1.tag = "CopyWeapon";
                //WeaponCopy2.tag = "CopyWeapon";

                // Áp dụng góc quay ban đầu của vũ khí gốc cho các bản sao
                WeaponCopy1.transform.rotation = weapon.rotation;
                /*  WeaponCopy2.transform.rotation = weapon.rotation*/
                ;

                // Tính toán hướng gốc
                Vector3 originalDirection = direction;

                // Tính toán góc ném lệch trái (-45 độ)
                Vector3 leftDirection = Quaternion.Euler(0, 30f * (i + 1 / 2), 0) * originalDirection;

                // Tính toán góc ném lệch phải (+45 độ)
                //Vector3 rightDirection = Quaternion.Euler(0, 85, 0) * originalDirection;

                // Tác động lực lên WeaponCopy1 với góc ném lệch trái
                WeaponCopy1.GetComponent<Rigidbody>().AddForce(leftDirection * 0.7f, ForceMode.Impulse);
                StartCoroutine(RotateCopyWeaponAroundYAxis(timeToReturn - 0.12f, WeaponCopy1.transform));
                WeaponCopy1.layer = LayerMask.NameToLayer("Default");
                WeaponCopy1.GetComponent<BoxCollider>().enabled = true;
                StartCoroutine(DestroyAfterTime(WeaponCopy1, timeToReturn));
            }


        }
    }
    public void StartScaling(GameObject obj, float duration)
    {
        StartCoroutine(ScaleOverTime(obj, duration));
    }
    private IEnumerator ScaleOverTime(GameObject obj, float duration)
    {
        Vector3 originalScale = obj.transform.localScale; // Kích thước ban đầu của object
        Vector3 targetScale = originalScale * 2; // Kích thước mục tiêu (gấp 2 lần)
        float elapsedTime = 0f;

        // Tăng kích thước từ từ trong khoảng thời gian `duration`
        while (elapsedTime < duration&&GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //// Đảm bảo kích thước đạt được đúng giá trị mục tiêu khi hoàn thành
        //obj.transform.localScale = targetScale;
    }
    private IEnumerator DestroyAfterTime(GameObject weaponCopy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (weaponCopy != null)
        {
            Destroy(weaponCopy);
        }
    }

    private IEnumerator WaitForAttackToEnd()
    {
        // Đợi cho đến khi animation "Attack" hoàn thành
        yield return new WaitForSeconds(2f);
        //anim.Play(""); // Chơi animation "Idle" sau khi animation "Attack" hoàn thành
    }


    public IEnumerator ReturnToParentAfterDelay(float delay, Vector3 originalPosition, Quaternion originalRotation, Vector3 originalScale )
    {
     

        if (isDead)
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
            weapon.GetComponent<Rigidbody>().isKinematic = true;
        }

        Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
        float elapsedTime = 0f;

        // Vòng lặp chờ
        while (elapsedTime < delay)
        {
            // Tạo bản sao vũ khí khi thời gian gần kết thúc, chỉ tạo 1 lần
            if (elapsedTime >= delay - 0.25f && weaponClone == null)
            {
                weaponClone = Instantiate(weapon.gameObject, originalPosition, originalRotation);
                weaponClone.transform.parent = originalWeaponParent;

                Rigidbody weaponCloneRb = weaponClone.GetComponent<Rigidbody>();
                weaponClone.GetComponent<BoxCollider>().enabled = false;
                if (weaponCloneRb != null)
                {
                    weaponCloneRb.isKinematic = false;
                }

                weaponClone.transform.localPosition = originalPosition;
                weaponClone.transform.localRotation = originalRotation;
                weaponClone.transform.localScale = originalScale;
                //GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate = false;
                IsRotate = false;
            }

            // Kiểm tra điều kiện để dừng và reset vũ khí
            bool ischeck = weapon.GetComponent<PlayerDameSender>().check;
            if (ischeck && GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate == false)
            {
                anim.SetFloat("attack", 0);
                anim.SetFloat("AttackUltimate,", 0);
                weapon.parent = originalWeaponParent;
                weapon.localPosition = originalPosition;
                weapon.localRotation = originalRotation;
                weapon.localScale = originalScale;
                //GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate = false;
                Debug.Log("okokokoko" + originalScale);
                weapon.GetComponent<BoxCollider>().enabled = false;
                weapon.gameObject.layer = LayerMask.NameToLayer("Playerr");
                IsRotate = false;
                if (weaponClone!=null)
                {
                    Destroy(weaponClone);

                }
                if (GameManager.Instance.Mode != "ZombieCity")
                {
                    weapon.localScale *= 1.1f; // Tăng scale lên 10% theo cả 3 trục
                }

                if (weaponRb != null)
                {
                    weaponRb.isKinematic = true;
                }
                
                numOfAttacks = 1;
                weapon.GetComponent<PlayerDameSender>().check = false;
                CanAttack = false;
                StartCoroutine(WaitAttackZomie());
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Xóa vũ khí gốc và thay thế bằng bản sao
        Destroy(weapon.gameObject);
        if (GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate)
        {
            anim.SetFloat("AttackUltimate", 0);
            GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.localScale /= 1.5f;
            GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius /= 1.5f;
        }
        else
        {
            anim.SetFloat("attack", 0);
        }
     
            weapon.GetComponent<PlayerDameSender>().check = false;
    

        GameManager.Instance.Armature.GetComponent<UltimateChek>().HaveUltimate = false;
        weapon = weaponClone.transform;

        if (weaponClone.GetComponent<Rigidbody>() != null)
        {
            weaponClone.GetComponent<Rigidbody>().isKinematic = true;
            //weaponClone.transform.localScale = weapon.localScale
        }
        weaponClone = null;
      
   

        numOfAttacks = 1;
        CanAttack = false;
        weapon.GetComponent<PlayerDameSender>().check = false;
        //weaponClone.GetComponent<PlayerDameSender>().check = false;
        StartCoroutine(WaitAttack());
    }

    private IEnumerator RotateWeaponAroundYAxis(float duration,Transform weapon)
 
    {
        IsRotate = true;
        float elapsedTime = 0f;

        // Tính toán tổng số góc cần quay để hoàn thành 10 vòng (360 độ * 10 vòng)
        float totalRotation = 2*360f;

        // Tính toán tốc độ quay (góc quay mỗi giây)
        float rotationSpeed = totalRotation / duration;

        while (elapsedTime < duration && IsRotate)
        {
            if (weapon!=null)
            {
                weapon.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
            }
            // Quay vũ khí quanh trục Y (hoặc trục bạn muốn)
          

            elapsedTime += Time.deltaTime;
            yield return null; // Đợi đến khung hình tiếp theo
        }

        // Đặt lại góc quay hoặc không, tùy vào yêu cầu của bạn
        IsRotate = false; // Dừng quay sau khi hoàn thành
    }
    private IEnumerator RotateCopyWeaponAroundYAxis(float duration,Transform weapon)
 
    {
       
        float elapsedTime = 0f;

        // Tính toán tổng số góc cần quay để hoàn thành 10 vòng (360 độ * 10 vòng)
        float totalRotation = 2*360f;

        // Tính toán tốc độ quay (góc quay mỗi giây)
        float rotationSpeed = totalRotation / duration;

        while (elapsedTime < duration)
        {
            if (weapon!=null)
            {
                weapon.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
            }
            // Quay vũ khí quanh trục Y (hoặc trục bạn muốn)
          

            elapsedTime += Time.deltaTime;
            yield return null; // Đợi đến khung hình tiếp theo
        }

        // Đặt lại góc quay hoặc không, tùy vào yêu cầu của bạn
        
    }



    Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent) // Duyệt qua tất cả các con của parent
        {
            if (child.name == name) // Nếu tìm thấy đối tượng có tên là name
            {
                return child;
            }
            
            Transform found = FindDeepChild(child, name); // Tìm đối tượng có tên là name trong con của parent
            if (found != null)
            {
                return found;
            }
        }
        return null; // Trả về null nếu không tìm thấy
    }
}
