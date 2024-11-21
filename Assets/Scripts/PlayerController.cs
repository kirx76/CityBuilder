using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // Получаем направление движения на основе входных данных
        var direction = new Vector3(horizontal, 0, vertical).normalized;

        if (!(direction.magnitude >= 0.1f))
            return;

        // Рассчитываем угол относительно камеры
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        var moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        // Двигаем персонажа
        transform.Translate(moveDirection * (speed * Time.deltaTime), Space.World);
    }
}
