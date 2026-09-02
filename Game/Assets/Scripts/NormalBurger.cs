using UnityEngine;

public class NormalBurger : MonoBehaviour
{
    [SerializeField] float downForce = 0.01f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(0f,downForce*Time.deltaTime,0f);
    }
}
