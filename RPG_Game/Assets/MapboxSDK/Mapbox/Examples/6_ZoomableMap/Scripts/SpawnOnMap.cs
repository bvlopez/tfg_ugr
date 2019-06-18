namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
	using UnityEngine.UI;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		[SerializeField]		
		private GameObject filterManager;
		FilterManager filter;	
		private bool isReady;

		void Start()
		{
			/*
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}*/
			filter = filterManager.GetComponent<FilterManager>();
			isReady = false;
		}

		private void Update()
		{	
			if(isReady) {	
				int count = _spawnedObjects.Count;
				for (int i = 0; i < count; i++)
				{
					var spawnedObject = _spawnedObjects[i];
					var location = _locations[i];
					spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
					spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				}
			}
		}

		// Poner mas parametros para los datos del spawn
		public void setPlayersSpawnsCoordinates(List<string> coordinates, List<int> players, List<string> factions, List<string> names) {
			GameObject[] playersSpawns = GameObject.FindGameObjectsWithTag("PlayerSpawn");
			GameManager gameManager = GameManager.instance;
			// Se eliminan los spawns anteriores
   			foreach(GameObject player in playersSpawns) {
   				GameObject.Destroy(player);
				isReady = false;  
			}
			if(filter.canShowPlayers()) {
				_locationStrings = new string[coordinates.Count];
				for(int i = 0; i < _locationStrings.Length; i++) {
					_locationStrings[i] = coordinates[i];
				}			
				_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.GetComponent<PlayerSpawner>().setPlayer(players[i], factions[i], names[i]);
					if(instance.GetComponent<PlayerSpawner>().getFaction() == gameManager.getPlayer().getFaction()) {
						instance.GetComponent<SpriteRenderer>().color = Color.green;					
					}
					else {
						instance.GetComponent<SpriteRenderer>().color = Color.red;
					}
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
				isReady = true;
			}
		}

		public void setShopsSpawnsCoordinates(List<string> coordinates, List<int> shops) {
			GameObject[] shopsSpawns = GameObject.FindGameObjectsWithTag("ShopSpawn");
			// Se eliminan los spawns anteriores
   			foreach(GameObject shop in shopsSpawns) {
   				GameObject.Destroy(shop);
				isReady = false; 
			}
			if(filter.canShowShops()) {
				_locationStrings = new string[coordinates.Count];
				for(int i = 0; i < _locationStrings.Length; i++) {
					_locationStrings[i] = coordinates[i];
				}			
				_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.GetComponent<ShopSpawner>().setShop(shops[i]);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
				isReady = true;
			}
		}

		public void setGuildsSpawnsCoordinates(List<string> coordinates, List<string> guilds) {
			GameObject[] guildsSpawns = GameObject.FindGameObjectsWithTag("GuildSpawn");
			// Se eliminan los spawns anteriores
   			foreach(GameObject guild in guildsSpawns) {
   				GameObject.Destroy(guild);
				isReady = false; 
			}
			if(filter.canShowGuilds()) {
				_locationStrings = new string[coordinates.Count];
				for(int i = 0; i < _locationStrings.Length; i++) {
					_locationStrings[i] = coordinates[i];
				}			
				_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.GetComponent<GuildSpawner>().setGuild(guilds[i]);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
				isReady = true;
			}
		}

		public void setBossesSpawnsCoordinates(List<string> coordinates, List<int> bosses, List<int> points, int user_points) {
			GameObject[] bossesSpawns = GameObject.FindGameObjectsWithTag("BossSpawn");
			GameObject.Find("BraveryPoints").GetComponent<Text>().text = user_points.ToString();
			// Se eliminan los spawns anteriores
   			foreach(GameObject boss in bossesSpawns) {
   				GameObject.Destroy(boss);
				isReady = false; 
			}
			if(filter.canShowBosses()) {
				_locationStrings = new string[coordinates.Count];
				for(int i = 0; i < _locationStrings.Length; i++) {
					_locationStrings[i] = coordinates[i];
				}			
				_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.GetComponent<BossSpawner>().setBoss(bosses[i]);
					instance.GetComponent<BossSpawner>().setBraveryPoints(points[i]);					
					if(!instance.GetComponent<BossSpawner>().checkPoints(user_points)) {
						instance.GetComponent<SpriteRenderer>().color = Color.red;
					}
					instance.transform.GetChild(0).GetComponent<TextMesh>().text = points[i].ToString();
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
				isReady = true;
			}
		}
	}
}