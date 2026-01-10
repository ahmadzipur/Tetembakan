using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float chaseDistance = 6f;
    public float stopDistance = 1.5f;
    public float attackDistance = 8f;
    public float fireRate = 0.8f;

    public Transform firePoint;

    // Prefab gun dari pack (sudah ada fire + sound)
    public GameObject gunPrefab;

    private Animator anim;
    private float fireTimer;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
{
    if (!player) return;

    float dist = Vector3.Distance(transform.position, player.position);

    // bergerak ke player
    Vector3 dir = (player.position - transform.position).normalized;
    dir.y = 0;

    if (dist > stopDistance)
    {
        // gerak
        transform.position += dir * moveSpeed * Time.deltaTime;
        anim.SetBool("isRunning", true);

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), 5f * Time.deltaTime);
    }
    else
    {
        anim.SetBool("isRunning", false);
    }

    // tembak
    if (dist <= attackDistance)
    {
        anim.SetBool("isShooting", true);
        Shoot();
    }
    else
    {
        anim.SetBool("isShooting", false);
    }
}

    void Shoot()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0;

            if (!gunPrefab || !firePoint) return;

            // rotasi firePoint ke player
            Vector3 shootDir = (player.position - firePoint.position).normalized;
            firePoint.rotation = Quaternion.LookRotation(shootDir);

            // Spawn gun fire effect (Particle + Sound)
            // Jika gun prefab sudah ada muzzle flash particle + audio, cukup Instantiate
            GameObject fireEffect = Instantiate(gunPrefab, firePoint.position, firePoint.rotation);
            Destroy(fireEffect, 1f); // destroy setelah 1 detik
        }
    }
}
