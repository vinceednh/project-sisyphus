using UnityEngine;

public class EnemyRush : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        speed = 8.0f;
        detectionRange = 5.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
