using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviourPun, IPunObservable 
{
    private Rigidbody2D rig;
    private Animator anim;
    private Vector2 clientPos;
    
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Text nickName;

    private float movement; 
    public float speed;

    //Jumps varialveis
    private bool isJump = false;
    public float forceJump = 10F;

    public Transform canvas;


    void Awake()
    {

        if (photonView.IsMine)
        {
            GameManager.instance.localPlayer = this.gameObject;
            nickName.text = PhotonNetwork.NickName;
        }
        else
        {
            nickName.text = photonView.Owner.NickName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine && GameManager.instance.isAlive)
        {
            //minha movimentação
            ProcessInput();
        }
        else
        {
            //sicroniza outros players
            SmoothMovement();
        }

    }

    private void Update()
    {
        JumpPlayer();
    }


    #region myClient

    public void ProcessInput()
    {
        movement = Input.GetAxis("Horizontal");
        
        rig.velocity = new Vector2(movement * speed, rig.velocity.y * (Time.deltaTime));

    
        if(Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("shoot");
            photonView.RPC("Shoot", RpcTarget.Others);  
            Debug.Log("pow!!");

        }

        if(movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            canvas.eulerAngles = new Vector3(0, 0, 0);
            this.photonView.RPC("ChangeRight", RpcTarget.Others);
        }

        if(movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            canvas.eulerAngles = new Vector3(0, 0, 0);
            this.photonView.RPC("ChangeLeft", RpcTarget.Others);
        }

        if(movement == 0)
        {

        }

       
    }

   
    void JumpPlayer()
    {

        if (Input.GetButtonDown("Jump") && isJump)
        {
            rig.AddForce(new Vector2(rig.velocity.x, forceJump));
            //rig.velocity = new Vector2(rig.velocity.y,forceJump);
            photonView.RPC("Jump", RpcTarget.Others);
            isJump = false;

            Debug.Log("PULOU");
        }
    }
        
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

            }
        }
      


        if (photonView.IsMine)
        {
            if (collision.gameObject.layer == 6)
            {
                isJump = true;
            }
        }
       
    }


    //private void OnCollisionExit2D(Collision2D collision)
    //{

    //    if (photonView.IsMine)
    //    {
    //        if (collision.gameObject.layer == 6)
    //        {
    //            isJump = false;
    //        }
    //    }
        
    //}

    #endregion

    #region RPCs functions

    [PunRPC]
    private void ChangeLeft()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        canvas.eulerAngles = new Vector3(0, 0, 0);

    }
    
    [PunRPC]
    private void ChangeRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        canvas.eulerAngles = new Vector3(0, 0, 0);
    }
    
    [PunRPC]
    private void Shoot()
    {
        GameObject b = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
       


        if(movement < 0)
        {   
            b.GetComponent<PhotonView>().RPC("MoveLeft", RpcTarget.AllBuffered);
        }
      
        anim.SetTrigger("shoot");
    }

    [PunRPC]
    private void Jump()
    {
        JumpPlayer();

       // rig.velocity = new Vector2(rig.velocity.y, forceJump);
       // rig.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
        //rig.velocity = Vector2.up * forceJump;
    }

    #endregion


    #region othersClients
    private void SmoothMovement()
    {
        // transform.position = Vector3.Lerp(transform.position, clientPos, Time.fixedDeltaTime);

        rig.position = Vector2.MoveTowards(rig.position, clientPos, Time.fixedDeltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rig.position); 
            stream.SendNext(rig.velocity);
        }
        else
        {
            clientPos = (Vector2)stream.ReceiveNext();
            rig.velocity = (Vector2)stream.ReceiveNext();


            float lag =Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            clientPos += rig.velocity * lag;
        }

        //if (stream.IsWriting)
        //{
        //    stream.SendNext(transform.position);
        //}
        //else if (stream.IsReading)
        //{
        //    clientPos = (Vector2)stream.ReceiveNext();
        //}
    }
    #endregion
}
