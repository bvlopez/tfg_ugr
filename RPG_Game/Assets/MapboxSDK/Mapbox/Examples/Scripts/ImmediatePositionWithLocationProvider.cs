namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using Mapbox.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.UI;

	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{
		//[SerializeField]
		//private UnifiedMap _map;

		bool _isInitialized;
		[SerializeField]
		private float distanceWithoutEncounter;
		private Vector2d lastPosition;		
		[SerializeField]
		private GameObject camera;
		[SerializeField]
		private Slider zoomSlider;

		ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}

		Vector3 _targetPosition;

		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
			StartCoroutine(UpdateUserCoordinates());
		}

		void LateUpdate()
		{
			if (_isInitialized)
			{
				var map = LocationProviderFactory.Instance.mapManager;
				transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);								
			}
		}

		private float CalculateDistance(float lat_1, float long_1, float lat_2, float long_2)
		{
			int R = 6371;
			var lat_rad_1 = Mathf.Deg2Rad * lat_1;
			var lat_rad_2 = Mathf.Deg2Rad * lat_2;
			var d_lat_rad = Mathf.Deg2Rad * (lat_2 - lat_1);
			var d_long_rad = Mathf.Deg2Rad * (long_2 - long_1);
			var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
			var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
			var total_dist = R * c * 1000; // Se pasa a metros
			return total_dist;
		}

		public IEnumerator UpdateUserCoordinates()
    	{	
			GameManager gameManager = GameManager.instance;
			while(true) {			
				Vector2d position = LocationProvider.CurrentLocation.LatitudeLongitude;				
                JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
				json.AddField("latitude", position.x.ToString().Replace(",", "."));
				json.AddField("longitude", position.y.ToString().Replace(",", "."));						
				StartCoroutine(gameManager.getServerConnection().postRequest(json, "shops_coordinates", getShopsResponse));	
				StartCoroutine(gameManager.getServerConnection().postRequest(json, "guilds_coordinates", getGuildsResponse));
				json.AddField("email", gameManager.getUser().getEmail());				
				StartCoroutine(gameManager.getServerConnection().postRequest(json, "bosses_coordinates", getBossesResponse));
				StartCoroutine(gameManager.getServerConnection().postRequest(json, "update_coordinates", getResponse));
				json.AddField("user_password", gameManager.getUser().getPassword());
				StartCoroutine(gameManager.getServerConnection().postRequest(json, "check_notifications", checkNotifications));

				if(position.x != 0 && position.y != 0 && lastPosition.x != 0 && lastPosition.y != 0) {
					// Se cuenta la distancia recorrida
					gameManager.addDistanceTraveled(CalculateDistance((float)lastPosition.x, (float)lastPosition.y, (float)position.x, (float)position.y));					
					distanceWithoutEncounter += CalculateDistance((float)lastPosition.x, (float)lastPosition.y, (float)position.x, (float)position.y);					
					float rand1 = Random.Range(1, 100);
					float rand2 = 100.0f - (distanceWithoutEncounter*Random.Range(0.6f, 3.0f));
					Debug.Log(rand1 + " " + rand2);
					if(rand1 > rand2) {
						distanceWithoutEncounter = 0;
						gameManager.changeScene("BattleScene");
					}
				}
				lastPosition = position;
				yield return new WaitForSeconds(5);
			}
    	}

		public void changeZoom() {
			camera.transform.position = new Vector3(camera.transform.position.x, zoomSlider.value * 2, camera.transform.position.z);
		}

		public void increaseZoom() {
			if((zoomSlider.value + 10) <= zoomSlider.maxValue) {
				zoomSlider.value = zoomSlider.value + 10;
			}
		}

		public void decreaseZoom() {
			if((zoomSlider.value - 10) >= zoomSlider.minValue) {
				zoomSlider.value = zoomSlider.value - 10;
			}
		}		

		public void checkNotifications(JSONObject json) {			
			// Inicia la batalla contra el jugador
			if(json.GetField("online_battle").str == "yes") {
				GameManager gameManager = GameManager.instance;
				gameManager.getOnlinePlayer(json);
			}
		}

		// Gestiona los jugadores cercanos
		public void getResponse(JSONObject json) {
			List<string> coordinates = new List<string>();
			GameObject playerSpawnerManager = GameObject.Find("PlayerSpawnerManager");
			SpawnOnMap spawnOnMap = playerSpawnerManager.GetComponent<SpawnOnMap>();			
			JSONObject array = json.GetField("players_coordinates");	
			foreach(JSONObject j in array.list) {
				coordinates.Add(j.GetField("coordinates").str);				
			}	

			List<int> players_id = new List<int>();
			JSONObject id_array = json.GetField("players_id");	
			foreach(JSONObject j in id_array.list) {
				players_id.Add(int.Parse(j.GetField("player_id").str));				
			}	

			List<string> players_factions = new List<string>();
			JSONObject factions_array = json.GetField("players_factions");	
			foreach(JSONObject j in factions_array.list) {				
				players_factions.Add(j.GetField("player_faction").str);				
			}

			List<string> players_names = new List<string>();
			JSONObject names_array = json.GetField("players_names");	
			foreach(JSONObject j in names_array.list) {				
				players_names.Add(j.GetField("player_name").str);				
			}	

			spawnOnMap.setPlayersSpawnsCoordinates(coordinates, players_id, players_factions, players_names);
		}

		// gestiona las tiendas cercanas
		public void getShopsResponse(JSONObject json) {
			List<string> coordinates = new List<string>();
			GameObject shopSpawnerManager = GameObject.Find("ShopSpawnerManager");
			SpawnOnMap spawnOnMap = shopSpawnerManager.GetComponent<SpawnOnMap>();			
			JSONObject array = json.GetField("shops_coordinates");	
			foreach(JSONObject j in array.list) {
				coordinates.Add(j.GetField("coordinates").str);				
			}

			List<int> shops_id = new List<int>();
			JSONObject id_array = json.GetField("shops_id");	
			foreach(JSONObject j in id_array.list) {
				shops_id.Add(int.Parse(j.GetField("id").str));				
			}				
			spawnOnMap.setShopsSpawnsCoordinates(coordinates, shops_id);
		}

		public void getGuildsResponse(JSONObject json) {
			List<string> coordinates = new List<string>();
			GameObject guildSpawnerManager = GameObject.Find("GuildSpawnerManager");
			SpawnOnMap spawnOnMap = guildSpawnerManager.GetComponent<SpawnOnMap>();			
			JSONObject array = json.GetField("guilds_coordinates");	
			foreach(JSONObject j in array.list) {
				coordinates.Add(j.GetField("coordinates").str);				
			}

			List<string> guilds_names = new List<string>();
			JSONObject names_array = json.GetField("guilds_names");	
			foreach(JSONObject j in names_array.list) {
				guilds_names.Add(j.GetField("name").str);				
			}				
			spawnOnMap.setGuildsSpawnsCoordinates(coordinates, guilds_names);
		}

		public void getBossesResponse(JSONObject json) {
			List<string> coordinates = new List<string>();
			GameObject bossSpawnerManager = GameObject.Find("BossSpawnerManager");
			SpawnOnMap spawnOnMap = bossSpawnerManager.GetComponent<SpawnOnMap>();			
			JSONObject array = json.GetField("bosses_coordinates");	
			foreach(JSONObject j in array.list) {
				coordinates.Add(j.GetField("coordinates").str);				
			}

			List<int> bravery_points_array = new List<int>();
			JSONObject points_array = json.GetField("bosses_bravery_points");	
			foreach(JSONObject j in points_array.list) {
				bravery_points_array.Add(int.Parse(j.GetField("bravery_points").str));				
			}

			List<int> bosses_id = new List<int>();
			JSONObject id_array = json.GetField("bosses_id");	
			foreach(JSONObject j in id_array.list) {
				bosses_id.Add(int.Parse(j.GetField("boss_id").str));				
			}	

			spawnOnMap.setBossesSpawnsCoordinates(coordinates, bosses_id, bravery_points_array, int.Parse(json.GetField("user_bravery_points").str));
		}

		
	}
}