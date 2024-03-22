using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    private StartOfTile startOfTile;
    private ObjectPooler objectPooler; 
    
    public int TilesCrossed;
    private Vector3 positionToSpawn;
    [SerializeField]private List<GameObject> activeTiles = new List<GameObject>();
    private int numberOfTiles = 3;
    private bool gameStarted = false;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        var position = transform.position;
        objectPooler.SpawnFromPool("startTile", position, Quaternion.identity);
        position +=
            Vector3.forward * objectPooler.ObjectToSpawn.transform.GetChild(0).GetChild(0).localScale.z;
        transform.position = position;
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if (TilesCrossed == 1 && gameStarted == false)
        {
            gameStarted = true;
            TilesCrossed = 0;
        }
        if (TilesCrossed >= 2)
        {
            SpawnTile();
            SpawnTile();
            if (activeTiles.Count > 3)
            {
                StartCoroutine(EraseTiles());
            }
        }
    }

    private void SpawnTile()
    {
        var position = transform.position;
        GameObject newTile = objectPooler.SpawnFromPool("tile", position, Quaternion.identity);
        activeTiles.Add(newTile);
        position +=
            Vector3.forward * objectPooler.ObjectToSpawn.transform.GetChild(0).GetChild(0).localScale.z;
        transform.position = position;
        //Spawn at start 1 tile then move manager to that pos and then add + 100 on z and then spawn another ....
        TilesCrossed = 0;
    }
    
    private IEnumerator EraseTiles()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 2; i++)
        {
            activeTiles[0].SetActive(false);
            activeTiles.RemoveAt(0);
        }
    }
}