using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public int jumptime;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    int jumptimeval;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumptimeval = jumptime;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump
        if(Input.GetButtonDown("Jump")&&jumptimeval>0){
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping",true);
            jumptimeval -= 1;
        }
            
        //not moving when not pressed horizontal keys
        if(!Input.GetButton("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }


        //Stop Speed
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        //Direction Sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        //Animation
        if(Mathf.Abs(rigid.velocity.x) < 1)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1))
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);


        //Landing Platform
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));

        if(rayHit.collider != null){
            if(rayHit.distance < 0.5f && rigid.velocity.y < 0){
                anim.SetBool("isJumping",false);
                jumptimeval = jumptime;
            }
            //Debug.Log(rayHit.collider.name);
        }
    }
}
