using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Light LanternLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanternLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            LanternLight.range += 1;
        } else if (Input.GetKeyDown("2"))
        {
            LanternLight.range -= 1;
        }
    }
}
