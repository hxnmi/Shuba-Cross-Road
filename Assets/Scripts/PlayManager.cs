using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour
{
	[SerializeField] List<Terrain> terrainList;
	[SerializeField] List<Coin> coinList;
	[SerializeField] int initialGrassCount = 5;
	[SerializeField] int horizontalSize;
	[SerializeField] int backViewDistance = -4;
	[SerializeField] int forwardViewDistance = 15;
	
	Dictionary<int,Terrain> activeTerrainDict = new Dictionary<int, Terrain>(20);
	
	[SerializeField] int travelDistance;
	[SerializeField] int coin;
	
	public UnityEvent<int, int> OnUpdateTerrainLimit;
	
	public UnityEvent<int> OnScoreUpdate;
	
	private void Start()
	{

		for (int zPos = backViewDistance; zPos < initialGrassCount; zPos++)
		{
			var terrain = Instantiate(terrainList[0]);
			
			terrain.transform.position = new Vector3(0, 0, zPos);
			
			if(terrain is Grass grass)
				grass.SetTreePercentage(zPos < -1 ? 1 : 0);
				
			terrain.Generate(horizontalSize);
			
			activeTerrainDict[zPos] = terrain;
		}

		for (int zPos = initialGrassCount; zPos < forwardViewDistance; zPos++)
		{
			SpawnRandomTerrain(zPos);
		}OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
	}
	
	private Terrain SpawnRandomTerrain(int zPos)
	{	
		Terrain comparatorTerrain = null;
		int randomIndex;
		
		for (int z = - 1; z >= - 3; z--)
		{
			var checkPos = zPos + z;
			
			if(comparatorTerrain == null)
			{
				comparatorTerrain = activeTerrainDict[checkPos];
				continue;
			}
			else if(comparatorTerrain.GetType() != activeTerrainDict[checkPos].GetType())
			{
				randomIndex = Random.Range(0,terrainList.Count);
				return SpawnTerrain(terrainList[randomIndex], zPos);
			}
			else
			{
				continue;
			}
		}
		
		var candidateTerrain = new List<Terrain>(terrainList);
		
		for(int i = 0; i < candidateTerrain.Count; i++)
		{
			if(comparatorTerrain.GetType() == candidateTerrain[i].GetType())
			{
				candidateTerrain.Remove(candidateTerrain[i]);
				break;
			}
		}
		
		randomIndex = Random.Range(0,candidateTerrain.Count);
		return SpawnTerrain(candidateTerrain[randomIndex], zPos);
	}
	
	public Terrain SpawnTerrain(Terrain terrain, int zPos)
	{
		terrain = Instantiate(terrain);
		terrain.transform.localPosition = new Vector3(0, 0, zPos);
		terrain.Generate(horizontalSize);
		activeTerrainDict[zPos] = terrain;
		SpawnCoin(horizontalSize, zPos);
		return terrain;
	}
	
	public Coin SpawnCoin(int horizontalSize, int zPos, float probability = 0.2f)
	{
		if(probability == 0)
			return null;
			
		List<Vector3> spawnPosCandidateList = new List<Vector3>();
		for(int x = -horizontalSize/2; x <= horizontalSize/2; x++)
		{
			var spawnPos = new Vector3(x, 0, zPos);
			if(Tree.PositionSet.Contains(spawnPos) == false)
				spawnPosCandidateList.Add(spawnPos);
		}
			
			
		if(probability >= Random.value)
		{
			var index = Random.Range(0,coinList.Count);
			var spawnPosIndex = Random.Range(0,spawnPosCandidateList.Count);
			Instantiate(coinList[index],spawnPosCandidateList[spawnPosIndex],Quaternion.identity);
		}
		return null;
	}
	
	public void UpdateTravelDistance(Vector3 targetPosition)
	{
		if(targetPosition.z > travelDistance)
		{
			travelDistance = Mathf.CeilToInt(targetPosition.z);
			UpdateTerrain();
			OnScoreUpdate.Invoke(GetScore());
		}
	}
	
	public void AddCoin(int value=1)
	{
		this.coin += value;
		OnScoreUpdate.Invoke(GetScore());
	}
	
	private int GetScore()
	{
		return travelDistance + coin;
	}
	
	public void UpdateTerrain()
	{
		var destroyPos = travelDistance -1 + backViewDistance;
		Destroy(activeTerrainDict[destroyPos].gameObject);
		activeTerrainDict.Remove(destroyPos);
		
		var spawnPosition = travelDistance - 1 + forwardViewDistance;
		SpawnRandomTerrain(spawnPosition);
		
		OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
	}
}
