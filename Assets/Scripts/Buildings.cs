using UnityEngine;

public class Buildings : MonoBehaviour
{
    public float speedMultiplier = 1f; // Adjust speed relative to game speed
    public float resetPositionX = -10f; // Position at which the building resets
    public float startPositionX = 10f; // Starting position for reset

    private void Update()
    {
        if (Player.instance)
        {
            if (Player.instance.isStop)
            {
                return;
            }
        }
        // Move the building based on the game speed
        float speed = GameManager.Instance.gameSpeed * speedMultiplier;
        transform.position += speed * Time.deltaTime * Vector3.left;

        // Reset the building's position when it moves out of view
        if (transform.position.x <= resetPositionX)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        // Move the building back to the starting position
        Vector3 position = transform.position;
        position.x = startPositionX;
        transform.position = position;
    }
}
