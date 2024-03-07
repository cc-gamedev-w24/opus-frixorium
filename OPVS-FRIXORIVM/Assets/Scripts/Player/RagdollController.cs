using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RagdollController : MonoBehaviour
{
    [SerializeField ] private Animator _animator;
    [SerializeField]
    private Transform _armatureRoot;
    private CharacterController _characterController;
    private Collider[] _colliders;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _colliders = GetComponentsInChildren<Collider>().Where(collider => collider.gameObject != gameObject).ToArray();
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        _characterController.enabled = false;
        _animator.enabled = false;
        foreach (var collider in _colliders)
        {
            collider.isTrigger = false;
            collider.attachedRigidbody.useGravity = true;
            collider.attachedRigidbody.velocity = _characterController.velocity;
        }
    }
    
    public void DisableRagdoll()
    {
        transform.position = _armatureRoot.position;
        _characterController.enabled = true;
        _animator.enabled = true;
        _animator.Rebind();
        _animator.Update(0f);
        foreach (var collider in _colliders)
        {
            collider.attachedRigidbody.useGravity = false;
            collider.isTrigger = true;
        }
    }
}
