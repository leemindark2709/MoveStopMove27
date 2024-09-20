using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Ultimate : MonoBehaviour
{
    public List<Transform> PointSpawGiftBoxs = new List<Transform>();
    public GameObject GiftBox;
    public float spawnInterval = 10f;  // Thời gian giữa các lần spawn (10 giây)

    private bool isSpawning = false;  // Để đảm bảo coroutine chỉ chạy một lần

    void Start()
    {
        // Lấy tất cả các con của Transform hiện tại (gameObject mà script này được attach)
        //foreach (Transform child in transform)
        //{
        //    PointSpawGiftBoxs.Add(child);
        //}
    }

    void Update()
    {
        // Kiểm tra điều kiện của GameManager trước khi bắt đầu coroutine
        if (GameManager.Instance.isStart && !GameManager.Instance.Armature.GetComponent<PlayerAttack>().isDead && !isSpawning)
        {
            isSpawning = true;  // Đảm bảo chỉ khởi chạy coroutine một lần
            StartCoroutine(SpawnGiftBox());
        }
    }

    IEnumerator SpawnGiftBox()
    {
        while (!GameManager.Instance.EndGame && GameManager.Instance.isStart)  // Kiểm tra điều kiện game
        {
            yield return new WaitForSeconds(spawnInterval);  // Chờ 10 giây

            // Chọn ngẫu nhiên một điểm trong danh sách
            int randomIndex = Random.Range(0, PointSpawGiftBoxs.Count);
            Transform spawnPoint = PointSpawGiftBoxs[randomIndex];

            // Spawn GiftBox tại vị trí của điểm đã chọn
            Instantiate(GiftBox, spawnPoint.position, spawnPoint.rotation);
        }

        isSpawning = false;  // Reset cờ khi vòng lặp kết thúc
    }
}
