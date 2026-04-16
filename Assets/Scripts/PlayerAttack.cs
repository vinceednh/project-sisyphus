using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int damage = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            RaycastHit hit;
            int layerMask = ~LayerMask.GetMask("Player");
            if(Physics.Raycast(ray, out hit, 100f, layerMask)){
                Debug.Log("hit " + hit.collider.gameObject.name);
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if(enemyHealth != null){
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }


}
