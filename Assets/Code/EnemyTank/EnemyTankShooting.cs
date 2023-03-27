using UnityEngine; 
using System.Collections; 
 
public class EnemyTankShooting : MonoBehaviour 
{ 
 
    // Prefab of the Shell 
    public Rigidbody m_Shell; 
    // A child of the tank where the shells are spawned 
    public Transform m_FireTransform;        
 
    // The force given to the shell when firing 
    public float m_LaunchForce = 999999999f;     
    
    public float m_ShootDelay = 0f; 
 
    private bool m_CanShoot; 
    private float m_ShootTimer; 
 
 
    private void Awake() 
    { 
        m_CanShoot = false; 
        m_ShootTimer = m_ShootDelay; 
    }
        // Update is called once per frame 
    private void Update() 
    { 
        if (m_CanShoot == true) 
        { 
            m_ShootTimer -= Time.deltaTime; 
            if (m_ShootTimer <= 0) 
            { 
                m_ShootTimer = m_ShootDelay; 
                StartCoroutine(shoot());
                 
            } 
        } 
    } 
 
    private void Fire() 
    { 
        // Create an instance of the shell and store a reference to it's rigidbody 
        Rigidbody shellInstance = Instantiate(m_Shell, 
                                  m_FireTransform.position, 
                                  m_FireTransform.rotation) as Rigidbody; 
 
        // Set the shell's velocity to the launch force in the fire position's  
        // forward direction 
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; 
    } 
 
    private void OnTriggerEnter(Collider other) 
    { 
        if (other.tag == "Player") 
        { 
            m_CanShoot = true; 
        } 
    } 
 
    private void OnTriggerExit(Collider other) 
    { 
        if (other.tag == "Player") 
        { 
            m_CanShoot = false; 
        } 
    } 

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 4f));
        Fire();
    }
    
} 