using UnityEngine;

public class Guy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private Material[] _activatedMaterial;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private Collider[] _childrenCollider;
    private Rigidbody[] _childrenRB;

    public void SetRunAnimation() => _animator.SetTrigger("Run");
    public void SetWinAnimation() => _animator.SetTrigger("Win");
    public void ChangeMaterial()
    {
        skinnedMeshRenderer.materials = _activatedMaterial;
    }

    private void Start()
    {
        _childrenCollider = GetComponentsInChildren<Collider>();
        _childrenRB = GetComponentsInChildren<Rigidbody>();
        RagdollActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            other.gameObject.SetActive(false);
            Player.Singleton.GetGem();
        }
        else if (other.CompareTag("Trap"))
        {
            Player.Singleton.DestroyGuy(this.gameObject);
            _particles.Play();
            RagdollActive(true);
        }
        else if (other.CompareTag("NPC"))
        {
            _particles.Play();
            Player.Singleton.AddGuy(other.gameObject);
        }
    }
    public void RagdollActive(bool active)
    {
        foreach (var collider in _childrenCollider)
           collider.enabled = active;
        foreach (var rb in _childrenRB)
        {
            rb.detectCollisions = active;
            rb.isKinematic = !active;
        }
        _animator.enabled = !active;
        _rb.detectCollisions = !active;
        _rb.isKinematic = active;
        _collider.enabled = !active;
        this.enabled = !active;
            
            
    }
}
