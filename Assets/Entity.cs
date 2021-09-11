using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    EntityState state = EntityState.IDLE;
    public Vector3 targetPosition;
    public bool targetReached;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 distance;
    public float speed;
    public float turnSpeed = 0.1f;

    public Quaternion targetRotation;

    Transform directionView;

    Rigidbody body;
    public Vector3 prevPosition;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        this.prevPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public float timeCount = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (this.targetPosition == null) return;

        // this.direction = FlowFieldProvider.GetVector(transform.position).normalized;

        direction.y = 0;
        direction.Normalize();
        UpdateRotation();
        if (targetReached) return;
        if (direction == null) return;
        this.velocity = direction * Time.deltaTime * speed;
        this.distance = targetPosition - transform.position;
        Debug.Log("************");
        Debug.Log(distance.magnitude + ":" + velocity.magnitude);
        Debug.Log((transform.position - prevPosition).sqrMagnitude);
        Debug.Log("************");
        // if (distance.magnitude <= 1)
        // {
        //     Debug.Log("TARGET REACHED");
        //     targetReached = true;
        //     velocity = distance;
        // }
        this.prevPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // transform.position += velocity;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
    }

    void UpdateRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        if (targetRotation != this.targetRotation) { Debug.Log(0); timeCount = 0; }
        if (!targetRotation.Equals(this.targetRotation)) { Debug.Log(1); timeCount = 0; }
        this.targetRotation = targetRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeCount);
        timeCount = Mathf.Repeat(timeCount + Time.deltaTime * turnSpeed, 1);
    }

    float Repeat(float t, float m)
    {
        return Mathf.Clamp(t - Mathf.Floor(t / m) * m, 0, m);
    }
    float Lerp(float start, float end, float t)
    {
        return start * (1 - t) + end * t;
    }

    float AngleLerp(float a, float b, float t)
    {
        var dt = Repeat(b - a, 360);
        return Lerp(a, a + (dt > 180 ? dt - 360 : dt), t);
    }

    public void SetMove(Vector3 target)
    {
        this.targetReached = false;
        this.targetPosition = target;
        this.direction = (this.targetPosition - transform.position);
    }

    void SetState(EntityState state)
    {
        this.state = state;
    }
}
