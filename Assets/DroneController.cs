using UnityEngine;

public class DroneController : MonoBehaviour
{
    private Transform target;
    public float speed = 5.0f;
    public float rotationSpeed = 2.0f;

    // --- NOVÉ PREMENNÉ PRE NÁHODNÝ POHYB ---
    [Header("Náhodný pohyb do strán")]
    public float strafeSpeed = 3.0f;        // Ako rýchlo sa bude hýbať do strany
    public float strafeInterval = 4.0f;     // Ako často (v sek.) sa rozhodne uhnúť (napr. každé 4 sekundy)
    public float strafeDuration = 1.5f;     // Ako dlho bude úhybný manéver trvať (napr. 1.5 sekundy)

    private Rigidbody rb;

    // Interné premenné na riadenie úhybného manévru
    private float timeUntilNextStrafe;
    private float strafeEndTime;
    private Vector3 strafeDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Dronu chýba Rigidbody komponent!", this);
        }

        // Nastavíme prvý úhybný manéver na náhodný čas v budúcnosti, aby nezačali všetky drony naraz
        timeUntilNextStrafe = Time.time + Random.Range(0, strafeInterval);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // --- HLAVNÝ POHYB K CIEĽU (zostáva rovnaký) ---
        Vector3 directionToTarget = (target.position - rb.position).normalized;
        Vector3 forwardVelocity = directionToTarget * speed;

        // Nastavíme základnú rýchlosť smerom k cieľu
        rb.linearVelocity = forwardVelocity;

        // --- LOGIKA PRE POHYB DO STRÁN ---
        
        // Je čas začať nový úhybný manéver?
        if (Time.time > timeUntilNextStrafe)
        {
            // Náhodne si vyberieme smer: doľava alebo doprava
            // transform.right je vektor ukazujúci doprava od drona
            strafeDirection = Random.value < 0.5f ? transform.right : -transform.right;

            // Nastavíme, kedy tento manéver skončí
            strafeEndTime = Time.time + strafeDuration;

            // Naplánujeme ďalší manéver
            timeUntilNextStrafe = Time.time + strafeInterval + Random.Range(-1f, 1f); // Pridáme trochu náhody do intervalu
        }

        // Ak práve prebieha úhybný manéver, pridáme bočnú rýchlosť
        if (Time.time < strafeEndTime)
        {
            // K existujúcej rýchlosti PRIČÍTAME bočný pohyb
            rb.linearVelocity += strafeDirection * strafeSpeed;
        }

        // --- ROTÁCIA (zostáva rovnaká) ---
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}