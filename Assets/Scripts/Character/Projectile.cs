using TMPro.Examples;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explodeCooldown;
    private float direction;
    private bool alreadyHit;

    private BoxCollider2D boxCollider;
    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (alreadyHit == true)
        {
            return;
        }

        float movementChange = speed * Time.deltaTime * direction;
        transform.Translate(movementChange, 0, 0);

        if (IsOnScreen(transform.position))
        {
            Invoke("Deactivate", explodeCooldown);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Trap" || collision.tag == "Wall" || collision.tag == "Ground")
        {
            alreadyHit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");

            Invoke("Deactivate", 0.5f);
        }

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null && collision.tag != "Character")
        {
            damagable.TakeDamage(10);
        }
    }

    public void StartShooting(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        alreadyHit = false;
        boxCollider.enabled = true;

        float localScaleX = Mathf.Abs(transform.localScale.x) * _direction;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public static bool IsOnScreen(Vector3 position)
    {
        var camera = Camera.current;
        if (!camera)
        {
            return false;
        }
        var screenPoint = Camera.current.WorldToViewportPoint(position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

}
