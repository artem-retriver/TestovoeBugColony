using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BugView : MonoBehaviour
{
    private Rigidbody _rb;
    
    public void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        Vector3 newPos = transform.position + direction * speed * Time.deltaTime;
        _rb.MovePosition(newPos);

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }
}