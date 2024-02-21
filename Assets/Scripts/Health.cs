using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviourPun
{

    public Image fillHealth;

    public float health = 1;

    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private BoxCollider2D boxColide;
    private Player player;


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxColide = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
    }

    #region RPC Functions    

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillHealth.fillAmount -= damage;
        health = fillHealth.fillAmount;
        CheckHealth();
    }

    [PunRPC]
    public void Die()
    {
        rig.gravityScale = 0f;
        boxColide.enabled = false;
        sprite.enabled = false;
        player.canvas .gameObject.SetActive(false);
    }
    
    [PunRPC]
    public void Revive()
    {
        rig.gravityScale = 4f;
        boxColide.enabled = true;
        sprite.enabled = true;
        player.canvas.gameObject.SetActive(true);
        fillHealth.fillAmount = 1;
        health = 1;

    }
    
    #endregion


    public void CheckHealth()
    {
        if(photonView.IsMine && health <= 0.1f)
        {
            //personagem morreu
            photonView.RPC("Die", RpcTarget.AllBuffered);
            GameManager.instance.EnableRespawn();
        }
    }
}
