using UnityEngine; 
using System.Collections; 
 
public class Shell : MonoBehaviour 
{ 
 
    // The time in seconds before the shell is removed 
    public float m_MaxLifeTime = 2f; 
    // The amount of damage done if the explosion is centred on a tank 
    public float m_MaxDamage = 34f; 
    // The maximum distance away from the explosion tanks can be and are still affected 
    public float m_ExplosionRadius = 9; 
    // The amount of force added to a tank at the centre of the explosion 
    public float m_ExplosionForce = 60f;       
     
    private float CalculateDamage(Vector3 targetPosition) 
    { 
        // Create a vector from the shell to the target 
        Vector3 explosionToTarget = targetPosition - transform.position; 
 
        // Calculate the distance from the shell to the target 
        float explosionDistance = explosionToTarget.magnitude; 
 
        // Calculate the proportion of the maximum distance (the explosionRadius)    
        // the target is away 
        float relativeDistance =  
        (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius; 
 
        // Calculate damage as this proportion of the maximum possible damage 
        float damage = relativeDistance * m_MaxDamage; 
 
        // Make sure that the minimum damage is always 0 
        damage = Mathf.Max(0f, damage); 
 
        return damage; 
    }
    // Reference to the particls that will play on explosion 
    public ParticleSystem m_ExplosionParticles; 
    // Use this for initialization 
    
    
    private void Start() 
    { 
        // If it isn't destroyed by then, destroy the shell after it's lifetime 
        Destroy(gameObject, m_MaxLifeTime); 
    } 
 
private void OnCollisionEnter(Collision other) 
    { 
        // find the rigidbody of the collision object 
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>(); 
 
        // only tanks will have rigidbody scripts 
        if (targetRigidbody != null)            
        { 
            // Add an explosion force 
            targetRigidbody.AddExplosionForce(m_ExplosionForce,  
            transform.position, m_ExplosionRadius); 
 
            // find the TankHealth script associated with the rigidbody 
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>(); 
 
            if (targetHealth != null) 
            { 
                // Calculate the amount of damage the target should take  
                // based on it's distance from the shell. 
                //int damage = CalculateDamage(targetRigidbody.position); 
 
                // Deal this damage to the tank 
                //targetHealth.TakeDamage(damage); 
            } 
        } 
 

        // Unparent the particles from the shell 
        m_ExplosionParticles.transform.parent = null; 
 
        // Play the particle system 
        m_ExplosionParticles.Play(); 
 
        // Once the particles have finished, destroy the gameObject they are on 
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration); 
 
        // Destroy the shell 
        Destroy(gameObject); 
    }
        public void OnDeath() 
    { 
        // Turn the tank off 
        gameObject.SetActive(false); 
    } 
}