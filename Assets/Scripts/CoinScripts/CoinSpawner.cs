using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    private ObjectPooler objectPooler;
    public void Start()
    {
        objectPooler = ObjectPooler.Instance;
        objectPooler.SpawnFromPool("coin", transform.position, Quaternion.identity);    
    }
}
