using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isFacingRight = true;

    private void Update()
    {
        // Kiểm tra nút chuột trái được nhấn
        if (Input.GetMouseButtonDown(0))
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
            transform.rotation = Quaternion.Euler(0f, -180f, 0f); // Quay sang bên trái
        }
    }

}
