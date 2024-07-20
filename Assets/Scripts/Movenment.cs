using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Movenment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer; // LayerMask để xác định lớp mặt đất
    private const float groundCheckRadius = 0.2f; // Bán kính của vòng tròn kiểm tra mặt đất
    public float speed = 5f; //Toc do
    public float jumpForce = 10f; // Luc nhay
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    //ham rieng tu
    private bool FacingRight = true;
    private bool dash;
    private float dashTime;
    [SerializeField] private bool isGrounded = false;
    private float moveInput; //Gia tri dau vao
    private bool jump;
  
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       moveInput = Input.GetAxisRaw("Horizontal"); //Lay gia tri dau vao cua truc X (A/D),(</>)
       //neu nhan nut jump nhan vat nhay
       if(Input.GetButtonDown("Jump"))
       {
         jump = true;
       }
       //nguoc lai
       else if(Input.GetButtonUp("Jump"))
       {
        jump = false;
       }
       UpdateAnimation();
       Flip(moveInput);
       
    }
    void FixedUpdate()
    {
        Move(moveInput);
        Jump(jump);
        Dash();
        GroundCheck();
    }
    //kiem tra mat dat co dung voi collider khac hay ko
        //2D collider co nam trong lop mat dat ko
        //neu co thi (isGrounded true) ko thi (isGrounded false)
    void GroundCheck()
    {
        // Giả sử ban đầu rằng nhân vật không chạm đất
        isGrounded = false;
        // sử dụng hàm Physics2D.OverlapCircleAll để kiểm tra xem có bất kỳ collider nào nằm trong vòng tròn kiểm tra mặt đất không
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
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
    void Jump(bool jumpFlag)
    {
        if(jumpFlag && isGrounded)
        {
            // Đặt isGrounded thành false để ngăn chặn việc nhảy lại ngay lập tức
            isGrounded = false;
            // Đặt jumpFlag thành false để reset trạng thái nhảy
            jumpFlag = false;
            // them luc nhảy 
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }
    void Dash()
    {
    }
    void Flip(float direction)
    {
        //nhin ben trai 
        if(FacingRight && direction < 0) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            FacingRight = false;
        }
        //nhin ben phai
        if(!FacingRight && direction > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            FacingRight = true;
        }
    }
    void UpdateAnimation()
    {
         anim.SetFloat("Running", Mathf.Abs(moveInput));
         anim.SetBool("Jumping", jump);
    }
}
