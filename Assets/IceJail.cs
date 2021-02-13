using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceJail : MonoBehaviour
{
    public float captureRange = 1.45f;

    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CapturePlayer()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, captureRange);
        foreach (Collider2D target in hitTargets)
        {
            if (GameManager.Instance.actorsContainer.ContainsKey(target.gameObject) && target.GetComponent<Actor>().IsPlayer)
            {
                Player.Instance.TakeIceJail(damage);
                // actor.TakeHit(MeleeDamage, gameObject);
                // if (isPlayer)
                //     Energy.AddEnergy(1);
            }
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, captureRange);
    }
}
