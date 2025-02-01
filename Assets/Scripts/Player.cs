using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;



    private bool canDoubleJump = false;
    private bool isShieldActive = false;
    private bool hasUsedDoubleJump = false;

    public int maxHealth = 3; // Maximum health (lives)
    private int currentHealth; // Current health
    public GameObject Shield;
    public GameObject Booster;
    public GameObject Jumper;

    public float dodgeDuration = 0.5f; // Duration of the dodge action
    public float dodgeSpeed = 10f; // Speed of the dodge
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;
    private const float SWIPE_THRESHOLD = 50f; // Minimum swipe distance for detection
    public bool isStop;
    public bool canDodge = true;
    //private float platformYPosition; // Store the player's current platform Y position

    public static Player instance;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
        if (!instance)
            instance = this;
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        hasUsedDoubleJump = false;
        currentHealth = maxHealth; // Reset health when the player is enabled
        UpdateHealthUI();
        isDodging = false;
        //platformYPosition = transform.position.y;
    }
    private void Update()
    {
        // Handle dodge duration
        character.transform.position = new Vector3(-6, 0, 0);
        if (isStop)
            return;
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;

            // Clamp the player to the platform Y position during dodge
            direction.y = 0;
            //Vector3 newPosition = transform.position;
            //newPosition.y = platformYPosition;
            //character.Move(Vector3.right * dodgeSpeed * Time.deltaTime);

            if (dodgeTimer <= 0)
            {
                EndDodge();
            }
        }
        else
        {
            // Apply gravity if not dodging
            direction += gravity * Time.deltaTime * Vector3.down;
            character.Move(direction * Time.deltaTime);
        }

        // Detect swipe or tap for controls
        DetectTouchInput();
    }

    private void StartDodge()
    {
        if (isDodging) return;
        if (!canDodge) return;

        GameManager.Instance.playerSprite.sprites = GameManager.Instance.dodgeSprite;
        AudioManager.instance.SlideSound();

        //Time.timeScale = 0.5f;
        isDodging = true;
        dodgeTimer = dodgeDuration;

        // Store the current platform Y position
        //platformYPosition = transform.position.y;

        // Adjust the player's hitbox for dodge
        character.center = new Vector3(0, -0.5f, 0); // Lower hitbox

        // Trigger the dodge animation
        //animator.SetTrigger("Dodge");
        //animator.SetBool("IsDodging", true); // Optional: For extended dodge animations
    }

    private void EndDodge()
    {
        GameManager.Instance.playerSprite.sprites = GameManager.Instance.runSprite;
        //Time.timeScale = 1f;
        isDodging = false;
        character.center = new Vector3(0, 0, 0); // Reset hitbox position
        //animator.SetBool("IsDodging", false); // Reset dodge animation state
    }
    private bool hasSwipedDown = false;
    private void DetectTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPosition = touch.position; // Store the starting position
            }
            else if (touch.phase == TouchPhase.Moved) // Detect movement while finger is still on screen
            {
                Vector2 swipeDelta = touch.position - swipeStartPosition;

                // Detect swipe down
                if (swipeDelta.y < -SWIPE_THRESHOLD)
                {
                    StartDodge();
                    swipeStartPosition = touch.position; // Reset to avoid multiple calls
                    hasSwipedDown = true; // Mark that a swipe down has occurred
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!hasSwipedDown) // Only detect jump if there was NO downward swipe
                {
                    swipeEndPosition = touch.position;
                    Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

                    // Detect tap (small movement)
                    if (swipeDelta.magnitude < SWIPE_THRESHOLD)
                    {
                        PerformJump();
                    }
                }

                hasSwipedDown = false; // Reset for next input cycle
            }
        }
    }



    private void PerformJump()
    {
        if (character.isGrounded || !canDodge)
        {
            if (AudioManager.instance)
                AudioManager.instance.JumpSound();
            direction = Vector3.up * jumpForce;
            hasUsedDoubleJump = false; // Reset double jump when grounded
        }
        else if (canDoubleJump && !hasUsedDoubleJump)
        {
            if (AudioManager.instance)
                AudioManager.instance.JumpSound();
            direction = Vector3.up * jumpForce;
            hasUsedDoubleJump = true; // Mark that double jump is used
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Platform"))
        {

            if (isShieldActive)
            {
                // Destroy obstacle if shield is active
                Destroy(other.gameObject);
            }
            else
            {
                if (other.CompareTag("Platform"))
                {
                    AudioManager.instance.HitSound();
                    ShakeCamera();
                    // Trigger Game Over
                    //GameManager.Instance.GameOver();
                    TakeDamage(1);
                    other.transform.parent.gameObject.SetActive(false);
                    return;

                }
                if (character.isGrounded && !other.CompareTag("Platform"))
                {
                    AudioManager.instance.HitSound();
                    ShakeCamera();
                    // Trigger Game Over
                    //GameManager.Instance.GameOver();
                    TakeDamage(1);
                }

            }
        }
        if (other.CompareTag("Coins"))
        {
            other.GetComponent<AudioSource>().Play();
            GameManager.Instance.SetCoins(1, other.GetComponent<SpriteRenderer>());
        }
        if (other.CompareTag("CantDodge"))
        {
            canDodge = false;
        }



        // Handle picking up powerups
        //if (other.CompareTag("Powerup"))
        //{
        //    // Assuming powerups have unique tags: "SpeedBoost", "Shield", "DoubleJump"
        //    if (other.name == "SpeedBoost")
        //    {
        //        GameManager.Instance.ActivateSpeedBoost();
        //    }
        //    else if (other.name == "Shield")
        //    {
        //        GameManager.Instance.ActivateShield();
        //    }
        //    else if (other.name == "DoubleJump")
        //    {
        //        GameManager.Instance.ActivateDoubleJump();
        //    }

        //    Destroy(other.gameObject); // Remove the powerup from the scene
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CantDodge"))
        {
            canDodge = true;
        }
    }
    public void ShakeCamera(float duration = 0.5f, float strength = 1f, int vibrato = 10, float randomness = 90f)
    {
        // Shake the camera using DoTween
        Camera.main.transform.DOShakePosition(duration, strength, vibrato, randomness);
    }
    // Enable Shield
    public void EnableShield()
    {
        isShieldActive = true;
    }

    // Disable Shield
    public void DisableShield()
    {
        isShieldActive = false;
    }

    // Enable Double Jump
    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    // Disable Double Jump
    public void DisableDoubleJump()
    {
        canDoubleJump = false;
    }

    // Take damage and check for Game Over
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    // Update health UI (to be implemented in GameManager)
    private void UpdateHealthUI()
    {
        GameManager.Instance.UpdateHealthUI(currentHealth);
    }
}
