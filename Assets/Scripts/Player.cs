using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed = 10f; //移動速度

    [SerializeField]
    float jumpPower = 15f;

    [SerializeField]
    float decelerationRate = 0.99f;

    [SerializeField]
    float decelerationRateOnIce = 0.99f;

    float _DecelerationRate = 0.7f;

    [SerializeField]
    TextMeshProUGUI txt;

    [SerializeField]
    GameObject groundHitObject;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    AudioClip se_jump;

    [SerializeField]
    AudioClip se_inWater;

    [SerializeField]
    GameObject waterHitObject;

    [SerializeField]
    LayerMask waterLayer;

    [SerializeField]
    Game game;

    Rigidbody2D rb;  　//物理挙動

    Animator anim;　　//アニメーション

    SpriteRenderer sp; //画像反転

    AudioSource snd;

    bool Onground;
    bool prevOnGround;

    int numJump = 0;

    float inWater; //水中にいる時間

    float deadTime = 2f;　//死ぬ時間

    public Vector3 initialPosition;




    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        snd = gameObject.AddComponent<AudioSource>();

        initialPosition = transform.position;
        _DecelerationRate = decelerationRate;

        Reset();

    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CheckWater();
        Move();
        jump();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            anim.speed = 1;
            sp.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            anim.speed = 1;
            sp.flipX = true;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * _DecelerationRate, rb.velocity.y);
            anim.speed = 0;
        }

    }

    void jump()
    {
        //zキーが押された瞬間
        if (Input.GetKeyDown(KeyCode.Z))
        {//getkeyは押されている間　getkeydownは押した瞬間
            if (Onground)
            {
                float jumpRestraint = rb.velocity.y;
                float newVerticalVelocity = Mathf.Min(jumpRestraint + jumpPower, jumpPower);
                rb.velocity = new Vector2(rb.velocity.x, newVerticalVelocity);
                numJump++;　//ジャンプの回数
                snd.PlayOneShot(se_jump);
            }
            else if (numJump == 1)
            {
                float jumpRestraint = rb.velocity.y;
                float newVerticalVelocity = Mathf.Min(jumpRestraint + jumpPower, jumpPower);
                rb.velocity = new Vector2(rb.velocity.x, newVerticalVelocity);
                numJump++;
                snd.PlayOneShot(se_jump);

            }

        }

        if (!prevOnGround && Onground)
        {
            numJump = 0;
        }
    }

    void CheckGround()
    {
        prevOnGround = Onground;
        Onground = Physics2D.OverlapCircle(groundHitObject.transform.position, 0.3f, groundLayer);

        //txt.text = Onground.ToString();

        if (Onground)
        {
            txt.text = "Ground";
            GameObject col = Physics2D.OverlapCircle(groundHitObject.transform.position, 0.3f, groundLayer).gameObject;
            if (col.tag == "Ice")
            {
                _DecelerationRate = decelerationRateOnIce;
            }
            else
            {
                _DecelerationRate = decelerationRate;
            }

        }
        else { txt.text = "Air" + numJump; }
        //txt.textに続けて文字を追加
        txt.text += "\nWater:" + inWater.ToString("f1");

    }


    void CheckWater()
    {
        //水面と水面判定オブジェクトの設定
        if (Physics2D.OverlapCircle(waterHitObject.transform.position, 0.01f, waterLayer))
        {
            if (inWater == 0) { snd.PlayOneShot(se_inWater); }
            inWater += Time.deltaTime;
            if (inWater >= deadTime)
            {
                //ゲームオーバー処理
                rb.bodyType = RigidbodyType2D.Kinematic;

                rb.velocity = new Vector2(0, 0);
                anim.speed = 0;
                game.StartGameover();
            }
        }
        else
        {
            inWater = 0;
        }
    }

    public void Reset()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = initialPosition;


        rb.velocity = new Vector2(0, 0);
        numJump = 0;
        anim.speed = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(0, 0);
            anim.speed = 0;
            game.StartGameclear();

        }

    }





}
