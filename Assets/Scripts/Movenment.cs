using UnityEngine;

public class Movenment : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; // LayerMask de xac dinh mat dat
    [SerializeField] private bool isGrounded = false; // Kiem tra xem nhan vat co dung tren mat dat hay khong

    [SerializeField] private AudioSource audioSource; // AudioSource component
    [SerializeField] private AudioClip moveSound; // Âm thanh di chuyển
    [SerializeField] private AudioClip jumpSound; // Âm thanh nhảy
    [SerializeField] private AudioClip dashSound; // Âm thanh dash
    [SerializeField] private float moveVolume = 0.5f; // Âm lượng di chuyển
    [SerializeField] private float jumpVolume = 0.5f; // Âm lượng nhảy
    [SerializeField] private float dashVolume = 0.5f; // Âm lượng dash

    [Header("Wall Jump")]
    public float wallJumpTime = 0.2f; // Thoi gian co the bam tuong truoc khi nhay
    public float wallSlideSpeed = 0.3f; // Toc do truot xuong khi bam tuong
    public float wallDistance = 0.5f; // Khoang cach toi tuong de xac dinh viec bam tuong
    private bool isWallSliding = false; // Kiem tra xem nhan vat co dang bam tuong hay khong
    private RaycastHit2D wallCheckHit; // Kiem tra va cham voi tuong
    private float jumpTime; // Thoi gian co the nhay tuong

    public float speed = 5f; // Toc do di chuyen
    public float jumpPower = 10f; // Luc nhay
    public float doubleJumpPower = 8f; // Luc nhay doi
    private bool canDoubleJump = true; // Cho phep nhay doi

    [SerializeField] private float coyoteTime = 0.5f; // Thoi gian co the nhay sau khi roi khoi mat dat
    private float coyoteTimeCounter; // Bo dem thoi gian coyote

    [SerializeField] private float jumpBufferTime = 0.6f; // Thoi gian bo dem cho viec nhay
    private float jumpBufferCounter; // Bo dem thoi gian nhay

    private float doubleTapTime; // Thoi gian giua 2 lan bam phim de xac dinh viec dash
    private KeyCode lastKeyCode; // Phim vua duoc nhan

    public float dashSpeed; // Toc do khi dash
    private float dashCount; // Dem nguoc thoi gian dash
    public float startDashCount; // Gia tri ban dau cua dem nguoc thoi gian dash
    private int side; // Huong cua dash

    [SerializeField] private GameObject effectDash;
    [SerializeField] private GameObject effectJump;

    private Animator anim; // Bien Animator de dieu khien cac hoat anh
    private const float groundCheckRadius = 0.2f; // Ban kinh cua vung kiem tra mat dat

    private bool facingRight = true; // Xac dinh huong nhin cua nhan vat
    private float moveInput; // Dau vao di chuyen theo truc ngang

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Lay thanh phan Rigidbody2D cua nhan vat
        anim = GetComponent<Animator>(); // Lay thanh phan Animator cua nhan vat
        dashCount = startDashCount; // Khoi tao gia tri ban dau cho dem nguoc thoi gian dash
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // Lay gia tri dau vao theo truc ngang (A/D hoac mui ten trai/phai)
        UpdateAnimation(); // Cap nhat cac hoat anh
        GroundCheck(); // Kiem tra mat dat
        Move(moveInput); // Xu ly di chuyen
        Jump(); // Xu ly nhay
        Dash(); // Xu ly dash
        Flip(); // Xu ly quay huong
    }

    void FixedUpdate()
    {
        WallJump(); // Xu ly bam tuong va nhay tuong
    }

    void GroundCheck()
    {
        // Kiem tra xem nhan vat co dung tren mat dat hay khong
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = colliders.Length > 0;

        // Reset thoi gian coyote va bo dem nhay neu dang dung tren mat dat
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Dat lai bo dem thoi gian coyote
            jumpBufferCounter = jumpBufferTime; // Dat lai bo dem thoi gian nhay
            canDoubleJump = true; // Reset kha nang nhay doi
        }
        else
        {
            // Giam cac bo dem neu khong dung tren mat dat
            coyoteTimeCounter -= Time.deltaTime;
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void Move(float direction)
    {
        float xVelocity = direction * speed * 100 * Time.fixedDeltaTime; // Tinh van toc theo truc ngang
        Vector2 newVelocity = new Vector2(xVelocity, rb.velocity.y); // Tao vector van toc moi
        rb.velocity = newVelocity; // Cap nhat van toc cua Rigidbody2D
        
        // Phát âm thanh di chuyển
        if (direction != 0 && !audioSource.isPlaying)
        {
            audioSource.clip = moveSound;
            audioSource.volume = moveVolume;
            audioSource.Play();
        }
    }

    void WallJump()
    {
        // Kiem tra xem nhan vat co cham vao tuong hay khong
        if (facingRight)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, Vector2.right, wallDistance, groundLayer); // Kiem tra va cham tuong phai
        }
        else
        {
            wallCheckHit = Physics2D.Raycast(transform.position, Vector2.left, wallDistance, groundLayer); // Kiem tra va cham tuong trai
        }

        if (wallCheckHit && !isGrounded && moveInput != 0)
        {
            isWallSliding = true; // Xac nhan nhan vat dang bam tuong
            jumpTime = Time.time + wallJumpTime; // Dat thoi gian co the bam tuong
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false; // Xac nhan nhan vat khong con bam tuong
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue)); // Giam van toc khi truot tuong
            anim.SetBool("WallSliding", true); // Bat hoat anh truot tuong
        }
        else
        {
            anim.SetBool("WallSliding", false); // Tat hoat anh truot tuong
        }
    }

    void Jump()
    {
        // Xu ly nhay va nhay doi
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || (isWallSliding && Input.GetButtonDown("Jump")))
            {
                // Thuc hien nhay binh thuong
                rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Dat van toc theo truc Y de nhay
                canDoubleJump = true; // Reset kha nang nhay doi khi nhay binh thuong
                jumpBufferCounter = 0f; // Dat lai bo dem thoi gian nhay
                DashJump();

                // Phát âm thanh nhảy
                audioSource.clip = jumpSound;
                audioSource.volume = jumpVolume;
                audioSource.Play();
            }
            else if (canDoubleJump)
            {
                // Thuc hien nhay doi
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpPower); // Dat van toc theo truc Y de nhay doi
                canDoubleJump = false; // Su dung nhay doi
                DashJump();

                // Phát âm thanh nhảy đôi
                audioSource.clip = jumpSound;
                audioSource.volume = jumpVolume;
                audioSource.Play();
            }
        }

        // Kiem tra viec tha nut nhay
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Giam van toc de tao hieu ung nhay thap hon
            coyoteTimeCounter = 0f; // Dat lai bo dem thoi gian coyote
        }
    }

    void Dash()
    {
        if (side == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
                {
                    side = 1; // Dat huong dash sang trai
                    anim.SetBool("Dashing", true); // Bat hoat anh dash
                    DashEffect();

                    // Phát âm thanh dash
                    audioSource.clip = dashSound;
                    audioSource.volume = dashVolume;
                    audioSource.Play();
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f; // Dat thoi gian cho lan bam phim tiep theo
                }
                lastKeyCode = KeyCode.A; // Luu phim vua bam
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
                {
                    side = 2; // Dat huong dash sang phai
                    anim.SetBool("Dashing", true); // Bat hoat anh dash
                    DashEffect();
                    
                    // Phát âm thanh dash
                    audioSource.clip = dashSound;
                    audioSource.volume = dashVolume;
                    audioSource.Play();
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f; // Dat thoi gian cho lan bam phim tiep theo
                }
                lastKeyCode = KeyCode.D; // Luu phim vua bam
            }
        }
        else
        {
            if (dashCount <= 0)
            {
                side = 0; // Khong con dash
                dashCount = startDashCount; // Dat lai gia tri dem nguoc thoi gian dash
                rb.velocity = Vector2.zero; // Dat van toc ve 0
                anim.SetBool("Dashing", false); // Tat hoat anh dash
            }
            else
            {
                dashCount -= Time.deltaTime; // Giam dem nguoc thoi gian dash

                if (side == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed; // Dat van toc theo huong trai khi dash
                }
                else if (side == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed; // Dat van toc theo huong phai khi dash
                }
            }
        }
        
    }
    private void DashEffect()
    {
        GameObject effect = Instantiate(effectDash, transform.position, Quaternion.identity); // Tao hieu ung tan cong
        RotateEffect(effect); // Xoay hieu ung theo huong cua player
    }
    private void DashJump()
    {
        GameObject effect = Instantiate(effectJump, transform.position, Quaternion.identity); // Tao hieu ung tan cong
        RotateEffect(effect); // Xoay hieu ung theo huong cua player
    }

    // Ham xoay hieu ung de phu hop voi huong cua player
    private void RotateEffect(GameObject effect)
    {
        Vector3 playerDirection = transform.localScale; // Lay huong cua player

        // Neu player quay sang trai, flip hieu ung
        if (playerDirection.x < 0)
        {
            effect.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            effect.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Flip()
    {
        // Xu ly quay huong nhin cua nhan vat
        if (facingRight && moveInput < 0f || !facingRight && moveInput > 0)
        {
            facingRight = !facingRight; // Thay doi huong nhin
            Vector3 localScale = transform.localScale; // Lay ty le hien tai cua nhan vat
            localScale.x *= -1f; // Nhan ty le theo chieu ngang voi -1 de quay nguoc
            transform.localScale = localScale; // Cap nhat ty le cua nhan vat
        }
    }

    void UpdateAnimation()
    {
        anim.SetFloat("Running", Mathf.Abs(moveInput)); // Cap nhat hoat anh chay
        anim.SetBool("Jumping", !isGrounded); // Cap nhat hoat anh nhay
    }
}
