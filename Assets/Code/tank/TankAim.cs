using UnityEngine; 
using System.Collections; 
 
public class TankAim : MonoBehaviour 
{ 
    LayerMask m_LayerMask; 
    public Camera camera;
    private void Awake() 
    { 
        m_LayerMask = LayerMask.GetMask("Ground"); 
    } 
    
    // Update is called once per frame 
    private void Update() 
    { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
 
        RaycastHit hit; 
 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask)) 
        { 
            transform.LookAt(hit.point); 
        } 
    }
} 