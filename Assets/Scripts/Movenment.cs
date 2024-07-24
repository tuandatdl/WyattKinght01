using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Security.Cryptography;
using TreeEditor;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movenment : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; // LayerMask để xác định lớp nền
    [SerializeField] private bool isGrounded = false;

    [Header("Wall Jump")]
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;

    public float speed = 5f; //Toc do
    public float jumpPower = 10f; // Luc nhay
    public float doubleJumpPower = 8f; // Lực nhảy đôi
    private bool doubleJump;

    private float coyotaTime = 0.2f;
    private float coyataTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private float doubleTapTime;
    private KeyCode lastKeyCode;

    public float dashSpeed;
    private float dashCount;
    public float startDashCount;
    private int side;

    private Animator anim;
    private const float groundCheckRadius = 0.2f; // Bán kính của vòng tròn kiểm tra mặt đất
    
    private bool FacingRight = true;
    private float moveInput; //Gia tri dau vao
  
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashCount = startDashCount;
    }
   
    void Update()
    {
       moveInput = Input.GetAxisRaw("Horizontal"); //Lay gia tri dau vao cua truc X (A/D),(</>)
       UpdateAnimation();
       GroundCheck();
       Move(moveInput);
       Jump();
       Dash();
       Flip();
      
    }
     void FixedUpdate()
    {
        WallJump();   
    }

    //kiem tra mat dat co dung voi collider khac hay ko
        //2D collider co nam trong lop mat dat ko
        //neu co thi (isGrounded true) ko thi (isGrounded false)
    void GroundCheck()
    {
        // Giả sử ban đầu rằng nhân vật không chạm đất
        if(jumpTime < Time.time)
        {
            isGrounded = false;
        }
        // sử dụng hàm Physics2D.OverlapCircleAll để kiểm tra xem có bất kỳ collider nào nằm trong vòng tròn kiểm tra mặt đất không
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
        {
            isGrounded = true;
        }
    }
   
    void Move(float direction)
    {
        float xVelocity = direction * speed * 100 * Time.fixedDeltaTime; //tinh van toc
        Vector2 newVelocity = new Vector2(xVelocity, rb.velocity.y);
        rb.velocity = newVelocity;
    }
    void WallJump()
    {
        if(FacingRight)
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
        }
        else
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
        }
        if(WallCheckHit && !isGrounded && moveInput != 0)
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }
        if(isWallSliding) 
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
            anim.SetBool("WallSliding", true); // Kích hoạt animation bám tường
        }
        else
        {
            anim.SetBool("WallSliding", false); // Tắt animation bám tường
        }
    }
    void Jump()
    {
        if(isGrounded)
        {
            coyataTimeCounter = coyotaTime;
        }
        else
        {
            coyataTimeCounter -= Time.deltaTime;
        }
        if(isGrounded)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if(isGrounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        if(Input.GetButtonDown("Jump")) 
        {
            if(coyataTimeCounter > 0f && jumpBufferCounter > 0f && isGrounded || doubleJump || isWallSliding && Input.GetButtonDown("Jump"))
            {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            doubleJump = !doubleJump;
            jumpBufferCounter = 0f;
            }
        }
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyataTimeCounter = 0f;
        }
    }
    void Dash()
    {
        if(side == 0)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                side = 1;
                anim.SetBool("Dashing", true); // Kích hoạt animation dashing
            }
            else
            {
                doubleTapTime = Time.time + 0.5f;
            }
            lastKeyCode = KeyCode.A;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if(doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
            {
                side = 2;
                anim.SetBool("Dashing", true); // Kích hoạt animation dashing
            }
            else
            {
                doubleTapTime = Time.time + 0.5f;
            }
            lastKeyCode = KeyCode.D;
            }
        }
        else
        {
            if(dashCount <= 0)
            {
                side = 0;
                dashCount = startDashCount;
                rb.velocity = Vector2.zero;
                anim.SetBool("Dashing", false); // Tắt animation dashing
            }
            else
            {
                dashCount -= Time.deltaTime;

                if(side == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if(side == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
            }
        }
    }
    void Flip()
    {
        if(FacingRight && moveInput < 0f || !FacingRight && moveInput > 0)
        {
            // Đổi hướng player
            FacingRight = !FacingRight;
            // Lấy thang đo hiện tại của player
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            // Lấy lại thang đo mới cho player
            transform.localScale = localScale;
        }
    }
    void UpdateAnimation()
    {
         anim.SetFloat("Running", Mathf.Abs(moveInput));
         anim.SetBool("Jumping", !isGrounded);
    }
}
