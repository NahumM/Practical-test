using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public ForestBehaviour forestBehaviour;
    float speed = 1;
    [SerializeField] GameObject _destractionParticle;
    void Update()
    {
        TranslateForward();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trunk")) // Start destroying of trunk when enters.
        {
            Instantiate(_destractionParticle, transform.position, Quaternion.identity);
            forestBehaviour.DestroyTrunk(other.gameObject);
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Wall")) // Deactivates the bullet when it hits out of bounds walls
        {
            gameObject.SetActive(false);
        }
    }

    private void TranslateForward()
    {
        transform.Translate(Vector3.forward * speed, Space.Self);
    }

}
