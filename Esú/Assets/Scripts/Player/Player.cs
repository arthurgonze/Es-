using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //Attack animation control
    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    //movement states control
    private enum Actions
    {
        WALK,
        ATTACK,
        RUN,
        STEALTH,
        IDLE
    }

    //cached references
    private Rigidbody2D body;
    private Animator anim;
    private Direction direction;
    private Actions action;
    private Camera mainCamera;
    private GameObject emissorDeRuido;
    private bool isHiding;

    private RaycastHit2D itemHit;
    private RaycastHit2D softGroundHit;

    [SerializeField] List<GameObject> inventory;

    //[SerializeField] int life = 300;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float stealthSpeed = -2f;

    [SerializeField] int noNoise = 0;
    [SerializeField] int littleNoise = 1;
    [SerializeField] int normalNoise = 2;
    [SerializeField] int bigNoise = 4;

    [SerializeField] LayerMask whatIsItem;
    [SerializeField] LayerMask whatIsSoftGround;

    [SerializeField] float raycastItemDistance;


    // Use this for initialization
    void Start()
    {
        //Cached references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        emissorDeRuido = this.transform.Find("Emissor_Ruido").gameObject;

        //Initial action state
        action = Actions.IDLE;
        //Initial direction state
        direction = Direction.UP;
        //Initial hiding state
        isHiding = false;

        raycastItemDistance = 1f;

        inventory = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
        ControleEmissorRuido();
        Interaction();
        CheckRaycastHit();
        //Debug.Log("Acao: " + action.ToString() + "Direction: " + direction.ToString());
    }

    private void Interaction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(itemHit)
            {
                Player player = this;
                if(itemHit.collider.GetComponent<ItemsInterface>() != null)
                {
                    itemHit.collider.GetComponent<ItemsInterface>().Interact(ref player);
                }
                //Debug.Log("Item Detected");
            }
            if(softGroundHit)
            {
                Player player = this;
                if (softGroundHit.collider.GetComponent<ItemsInterface>() != null)
                {
                    softGroundHit.collider.GetComponent<ItemsInterface>().Interact(ref player);
                }
                //Debug.Log("Soft Ground Detected");
            }
        }
    }

    private void CheckRaycastHit()
    {
        if(direction == Direction.UP)
        {
            itemHit = Physics2D.Raycast(transform.position, Vector2.up, raycastItemDistance, whatIsItem);
            softGroundHit = Physics2D.Raycast(transform.position, Vector2.up, raycastItemDistance, whatIsSoftGround);
        }
        if(direction == Direction.DOWN)
        {
            itemHit = Physics2D.Raycast(transform.position, Vector2.down, raycastItemDistance, whatIsItem);
            softGroundHit = Physics2D.Raycast(transform.position, Vector2.down, raycastItemDistance, whatIsSoftGround);
        }
        if(direction == Direction.LEFT)
        {
            itemHit = Physics2D.Raycast(transform.position, Vector2.left, raycastItemDistance, whatIsItem);
            softGroundHit = Physics2D.Raycast(transform.position, Vector2.left, raycastItemDistance, whatIsSoftGround);
        }
        if(direction == Direction.RIGHT)
        {
            itemHit = Physics2D.Raycast(transform.position, Vector2.right, raycastItemDistance, whatIsItem);
            softGroundHit = Physics2D.Raycast(transform.position, Vector2.right, raycastItemDistance, whatIsSoftGround);
        }
    }

    private void Attack()
    {
        if (Input.GetKey("space"))
        {
            //update action state
            action = Actions.ATTACK;

            switch (direction)
            {
                //attack Up
                case Direction.UP:
                    anim.SetBool("Aup", true);
                    break;
                //attack Down
                case Direction.DOWN:
                    anim.SetBool("Adown", true);
                    break;
                //attack Right
                case Direction.RIGHT:
                    anim.SetBool("Aright", true);
                    break;
                //attack Left
                case Direction.LEFT:
                    anim.SetBool("Aleft", true);
                    break;
            }
        }else
        {
            if(action == Actions.ATTACK)
            {
                action = Actions.IDLE;
            }
            //default state attack animations
            anim.SetBool("Aup", false);
            anim.SetBool("Adown", false);
            anim.SetBool("Aright", false);
            anim.SetBool("Aleft", false);
        }

        //caso nao esteja atacando, volta para o estado default
        if (Input.GetKeyUp("space"))
        {
            action = Actions.IDLE;
        }
    }

    private void Move()
    {
        //so ira se mover se nao estiver atacando
        if(action != Actions.ATTACK)
        {
            //verifica o input do jogador para diferenciar se o mesmo vai andar,
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                action = Actions.WALK;
                Walk();
            }

            //correr
            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                action = Actions.RUN;
                Run();
            }

            //ou andar lentamente
            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && Input.GetKey(KeyCode.C))
            {
                action = Actions.STEALTH;
                Stealth();
            }

            //Caso esteja parado coloca sua acao e animacoes em default
            if((Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0))
            {
                action = Actions.IDLE;

                anim.SetBool("up", false);
                anim.SetBool("down", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
        }
    }

    //Walk player movement
    private void Walk()
    {
        //Move Up
        if (Input.GetAxis("Vertical") > 0)
        {
            //update position
            transform.position = new Vector2(transform.position.x, transform.position.y + walkSpeed * Time.deltaTime);

            //update move animation
            anim.SetBool("up", true);
            direction = Direction.UP;
        }
        else
        {
            anim.SetBool("up", false);
        }

        //Move Down
        if (Input.GetAxis("Vertical") < 0)
        {
            //update position
            transform.position = new Vector2(transform.position.x, transform.position.y - walkSpeed * Time.deltaTime);
            //transform.Translate(new Vector2(body.velocity.x, -walkSpeed * Time.deltaTime));

            //update move animation
            anim.SetBool("down", true);
            direction = Direction.DOWN;
        }
        else
        {
            anim.SetBool("down", false);
        }

        //Move Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            //update position
            //transform.Translate(new Vector2(-walkSpeed * Time.deltaTime, body.velocity.y));
            transform.position = new Vector2(transform.position.x - walkSpeed * Time.deltaTime, transform.position.y);

            //update move animation
            anim.SetBool("left", true);
            direction = Direction.LEFT;
        }
        else
        {
            anim.SetBool("left", false);
        }


        //Move Right
        if (Input.GetAxis("Horizontal") > 0)
        {
            //update position
            transform.position = new Vector2(transform.position.x + walkSpeed * Time.deltaTime, transform.position.y);
            //transform.Translate(new Vector2(walkSpeed * Time.deltaTime, body.velocity.y));

            //update move animation
            anim.SetBool("right", true);
            direction = Direction.RIGHT;
        }
        else
        {
            anim.SetBool("right", false);
        }
    }

    //Run player movement
    private void Run()
    {
        //Move Up
        if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            //update position
            transform.Translate(new Vector2(body.velocity.x, runSpeed * Time.deltaTime));

            //update move animation
            anim.SetBool("up", true);
            direction = Direction.UP;
        }
        else
        {
            anim.SetBool("up", false);
        }

        //Move Down
        if (Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            //update position
            transform.Translate(new Vector2(body.velocity.x, -runSpeed * Time.deltaTime));

            //update move animation
            anim.SetBool("down", true);
            direction = Direction.DOWN;
        }
        else
        {
            anim.SetBool("down", false);
        }

        //Move Left
        if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            //update position
            transform.Translate(new Vector2(-runSpeed * Time.deltaTime, body.velocity.y));

            //update move animation
            anim.SetBool("left", true);
            direction = Direction.LEFT;
        }
        else
        {
            anim.SetBool("left", false);
        }

        //Move Right
        if (Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            //update position
            transform.Translate(new Vector2(runSpeed * Time.deltaTime, body.velocity.y));

            //update move animation
            anim.SetBool("right", true);
            direction = Direction.RIGHT;
        }
        else
        {
            anim.SetBool("right", false);
        }
    }

    //Stealth player movement
    private void Stealth()
    {
        //Move Up
        if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.C))
        {
            //update position
            transform.Translate(new Vector2(body.velocity.x, stealthSpeed * Time.deltaTime));

            //update move animation
            anim.SetBool("up", true);
            direction = Direction.UP;
        }
        else
        {
            anim.SetBool("up", false);
        }

        //Move Down
        if (Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.C))
        {
            //update position
            transform.Translate(new Vector2(body.velocity.x, -stealthSpeed * Time.deltaTime));

            //update move animation
            anim.SetBool("down", true);
            direction = Direction.DOWN;
        }
        else
        {
            anim.SetBool("down", false);
        }

        //Move Left
        if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.C))
        {
            //update position
            transform.Translate(new Vector2(-stealthSpeed * Time.deltaTime, body.velocity.y));

            //update move animation
            anim.SetBool("left", true);
            direction = Direction.LEFT;
        }
        else
        {
            anim.SetBool("left", false);
        }

        //Move Right
        if (Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.C))
        {
            //update position
            transform.Translate(new Vector2(stealthSpeed * Time.deltaTime, body.velocity.y));

            //update move animation
            anim.SetBool("right", true);
            direction = Direction.RIGHT;
        }
        else
        {
            anim.SetBool("right", false);
        }
    }

    private void ControleEmissorRuido()
    {
        if(!isHiding)
        {
            switch (action)
            {
                case Actions.IDLE:
                    emissorDeRuido.transform.localScale = new Vector3(noNoise, noNoise, 0);
                    break;
                case Actions.WALK:
                    emissorDeRuido.transform.localScale = new Vector3(normalNoise, normalNoise, 0);
                    break;
                case Actions.RUN:
                    emissorDeRuido.transform.localScale = new Vector3(bigNoise, bigNoise, 0);
                    break;
                case Actions.STEALTH:
                    emissorDeRuido.transform.localScale = new Vector3(littleNoise, littleNoise, 0);
                    break;
            }
        }
        else
        {
            switch (action)
            {
                case Actions.IDLE:
                    emissorDeRuido.transform.localScale = new Vector3(noNoise, noNoise, 0);
                    break;
                case Actions.WALK:
                    emissorDeRuido.transform.localScale = new Vector3(littleNoise, littleNoise, 0);
                    break;
                case Actions.RUN:
                    emissorDeRuido.transform.localScale = new Vector3(normalNoise, normalNoise, 0);
                    break;
                case Actions.STEALTH:
                    emissorDeRuido.transform.localScale = new Vector3(noNoise, noNoise, 0);
                    break;
            }
        }
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Stealth" && collision.IsTouching(this.GetComponent<CircleCollider2D>()))
        {
            isHiding = true;
            this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            //Debug.Log("isHiding: " + isHiding);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stealth")
        {
            isHiding = false;
            this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            //Debug.Log("isHiding: " + isHiding);
        }
    }

    public bool IsItemInInventory(GameObject item)
    {
        if(item.gameObject != null)
        {
            Debug.Log("Game Object Tag: " + item.gameObject.tag.ToString());
            return inventory.Contains(item);
        }else
        {
            return false;
        }
    }
    public bool IsItemInInventory(string tag)
    {
        foreach(GameObject item in inventory)
        {
            if(item.tag == tag)
            {
                return true;
            }
        }
        return false;
    }

    public bool AddItemInInventory(GameObject item)
    {
        if (IsItemInInventory(item))
        {
            return false;
        }else
        {
            inventory.Add(item);
            return true;
        }
    }
}
