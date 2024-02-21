using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed;
    public float destroyTime = 2f;

    public bool isLeft;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("destroyBullet");
        //photonView.RPC("Destroy", RpcTarget.AllBuffered);
    }


    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeft)
        {
            transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
    }

    //[PunRPC]
    //private void Destroy()
    //{
    //    Destroy(gameObject, destroyTime);
    //}
     
    [PunRPC]
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
    
    [PunRPC]
    public void MoveLeft()
    {
        isLeft = true;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null)
        {
            if (target.CompareTag("Player"))
            {
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, 0.2f);
            }

            photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
        }
    }   

   
}
