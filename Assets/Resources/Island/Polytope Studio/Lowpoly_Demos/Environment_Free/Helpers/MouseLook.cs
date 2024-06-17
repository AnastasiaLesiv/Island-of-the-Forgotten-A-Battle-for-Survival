using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MouseLook : MonoBehaviour
{
    public float speed = 100f; // Швидкість обертання

    void Update()
    {
        // Отримуємо позицію курсора на екрані
        Vector3 cursorPos = Input.mousePosition;

        // Перетворюємо позицію курсора в позицію в просторі сцени
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(cursorPos);

        // Обертаємо об'єкт навколо вертикальної осі, щоб дивитися на курсор
        Vector3 lookDir = (worldPos - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, speed * Time.deltaTime);
    }
}
