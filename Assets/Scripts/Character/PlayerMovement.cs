using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundlayer;
    public Rigidbody2D body;
    public Animator anim;
    private BoxCollider2D boxCollider;
    private bool grounded; // Use Grounded method instead

    public Animator GetAnimator()
    {
        return anim;
    }

    public void SetSpeed(float Speed) 
    {
        speed = Speed; 
    }

    public void SetjumpForce(float JumpForce)
    {
        jumpForce = JumpForce;
    }

    public void SetAnimator(RuntimeAnimatorController animatorController)
    {
        anim.runtimeAnimatorController = animatorController;
    }


    private void Awake()
    {
        //Grab references for rigibody & animator from game objects
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Flip player when moving left or right, keeping the original scale
        if (horizontalInput > 0.01F)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (horizontalInput < -0.01F)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Input.GetKey(KeyCode.UpArrow) && grounded)  // use Grounded method
            Jump();
       

        //Set animator parameters 
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    public void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)  // fix bug when jump fails
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

    public bool Grounded()
    {
        RaycastHit2D raycasthit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycasthit.collider != null;
    }
}
