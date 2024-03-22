using UnityEngine;

public class CoinScript : MonoBehaviour, IPooledObject
{
    [SerializeField] private Rigidbody rigidBody;
    private CoinManager coinManager;
    private AudioManager audioManager;
    public string CollisionObjectName = "Player";

    public void OnObjectSpawn()
    {
        coinManager = FindObjectOfType<CoinManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        rigidBody.AddTorque(0, 1, 0, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CollisionObjectName))
        {
            audioManager.PlayCoinSound();
            gameObject.SetActive(false);
            coinManager.CoinCount++;
        }
    }
}
