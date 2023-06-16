using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float cameraFollowSpeed = 0.8f;

    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isJumping = false;
    private Vector3 Platform;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Kiểm tra nút chuột trái được nhấn
        if (Input.GetMouseButtonDown(0) && isJumping == false && Platform != Vector3.zero)
        {

            // Lấy vị trí x của chuột so với màn hình
            float mousePosX = Input.mousePosition.x;

            // Lấy kích thước chiều rộng của màn hình
            float screenWidth = Screen.width;

            // So sánh vị trí x của chuột với giữa màn hình
            if (mousePosX < screenWidth / 2)
            {
                // Nếu vị trí x nhỏ hơn giữa màn hình, quay sang bên trái
                FlipCharacter(false);
            }
            else
            {
                // Ngược lại, quay sang bên phải
                FlipCharacter(true);
            }
                // Thực hiện nhảy
                Jump();
            // Kéo camera theo nhân vật
            FollowPlayer();
        }

     
    }

    private void Jump()
    {
        if (isJumping)
        {

            transform.localScale = new Vector3(-1, 1, 1);
            Vector3 targetPosition = new Vector3(Platform.x, Platform.y + 0.8f, transform.position.z);
            float distance = Vector3.Distance(transform.position, targetPosition);
            float moveSpeed = distance / 0.2f; // Tính toán tốc độ di chuyển dựa trên khoảng cách và thời gian di chuyển mong muốn

            StartCoroutine(MoveToPosition(targetPosition, moveSpeed));
            transform.localScale = Vector3.one;
        }

        // Coroutine để di chuyển nhân vật đến vị trí mong muốn với tốc độ xác định
        IEnumerator MoveToPosition(Vector3 targetPosition, float moveSpeed)
        {
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    

        
    }

    private void FlipCharacter(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;

        // Quay nhân vật theo hướng mong muốn
        if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Quay sang bên phải
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Quay sang bên trái
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với bậc thang
        if (collision.gameObject.CompareTag("Stair"))
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            Platform = new Vector3(currentPlatformPos.x -
                0.554f, currentPlatformPos.y + 0.645f, 0);
            
        }
    }

}
