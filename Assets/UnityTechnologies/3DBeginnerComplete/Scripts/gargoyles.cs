using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

// audio stuff
    public AudioSource detectionSound;
    private bool isSoundPlaying = false;

// particle stuff
    public ParticleSystem detectionParticles;
    private bool isParticlePlaying = false;

// tracking detection stuff
    public float viewAngle = 110f;
    public float viewDistance = 2f;
    public Vector3 _angles = new Vector3(0.0f, 0.0f, 0.0f);
    public const float INV_PERIOD_SECONDS = 1.0f/5.0f;
    public const float MIN_ANG_VEL_DEG = 10.0f;
    public const float MAX_ANG_VEL_DEG = 1000.0f;
    public float _alpha = 0.0f; // normalized to [0.0, 1.0] scale
    private float detectionTimer = 0f;
    public float detectionTime = 2f; // Seconds needed to detect the player

    void Update() // add sound effect for detection
    {
        GetComponent<Animator>().applyRootMotion = false;
        Vector3 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        bool playerInSight = false;

        if (distanceToPlayer < viewDistance)
        {
            Vector3 directionToPlayer = toPlayer.normalized;
            Vector3 forward = transform.forward;

            float dot = Vector3.Dot(forward, directionToPlayer);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle < viewAngle * 0.5f)
            {
                Ray ray = new Ray(transform.position + Vector3.up, directionToPlayer);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, viewDistance))
                {
                    if (hit.transform == player)
                    {
                        playerInSight = true;
                    }
                }
            }
        }

        if (playerInSight)
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= detectionTime)
            {
                gameEnding.CaughtPlayer();
            }

            _alpha += Time.fixedDeltaTime * INV_PERIOD_SECONDS;
            if (_alpha > 1.0f) {
                _alpha -= 1.0f;
            }

            float interpAngVelDeg = 
                (1.0f - _alpha) * MIN_ANG_VEL_DEG +
                _alpha * MAX_ANG_VEL_DEG;

            transform.Rotate(Vector3.up * interpAngVelDeg * Time.deltaTime);

            Vector3 d = player.position - transform.position;

            d.Normalize();

            float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(Vector3.forward, d));

            Vector3 cross = Vector3.Cross(Vector3.forward, d);
            if (cross.y < 0.0f) {
                angle = -angle;
            }

            _angles.y = angle;
            transform.eulerAngles = _angles;

            if (!isSoundPlaying)
            {
                detectionSound.Play();
                isSoundPlaying = true;
            }

            // Play particle effect
            if (!isParticlePlaying)
            {
                detectionParticles.Play();
                isParticlePlaying = true;
            }
        }
        else
        {
            detectionTimer = Mathf.Max(0f, detectionTimer - Time.deltaTime);
            
            if (isSoundPlaying)
            {
                detectionSound.Stop();  // Optional: Stop it if looping
                isSoundPlaying = false;
            }
            if (isParticlePlaying)
            {
                detectionParticles.Stop();
                isParticlePlaying = false;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Vector3 leftRay = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward * viewDistance;
        Vector3 rightRay = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward * viewDistance;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }

}