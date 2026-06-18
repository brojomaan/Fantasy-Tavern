using UnityEngine;

public class PhysicsComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    public bool Initialize()
    {
        if (rb == null) { Debug.LogError("ItemController::Initialize(): rb = null"); return false; }
        if (col == null) { Debug.LogError("ItemController::Initialize(): col = null"); return false; }

        return true;
    }

    public void SetKinematic(bool kinematic)
    {
        rb.isKinematic = kinematic;
    }

    public void SetCollider(bool isEnabled)
    {
        col.enabled = isEnabled;
    }

    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {
        if (!rb.isKinematic)
            rb.AddForce(force, mode);
    }
}
