using UnityEngine;

public class Enemy : Actor
{
    private GameObject player;
    [Header("BOSS")]
    [SerializeField] private Transform waveSpawnPoint;
    [SerializeField] private Projectile waveProjectile;
    [SerializeField] private int waveDamage = 5;
    [SerializeField] private IceJail jailPrefab;
    void Start()
    {
        base.Start();
        player = GameManager.Instance.player.gameObject;
    }

    
    void FixedUpdate()
    {
        //MovementUpdate(Vector3.left);
        //MeleeAttack();


        //animator.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));
    }

    public void LookAtPlayer()
    {
        if (!player)
            player = GameManager.Instance.player.gameObject;
        Vector3 direction = player.transform.position - (Vector3) rigidbody.position;
        
        if (direction.x > 0)
        {
            isOnRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction.x < 0)
        {
            isOnRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    void ShootWave()
    {
        var currentProjectile = Instantiate(waveProjectile, waveSpawnPoint.position, Quaternion.identity);
        currentProjectile.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, waveDamage);
        //AudioManager.Instance.Play(soundRangeRelease);
        //StartCoroutine(StartCooldown());
        //isMovable = true;
    }
    
    public void CastJail()
    {
        var currentProjectile = Instantiate(jailPrefab, new Vector3(player.transform.position.x, -5.65f, player.transform.position.z), Quaternion.identity);
        //currentProjectile.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, waveDamage);
        //AudioManager.Instance.Play(soundRangeRelease);
        //StartCoroutine(StartCooldown());
        //isMovable = true;
    }
}