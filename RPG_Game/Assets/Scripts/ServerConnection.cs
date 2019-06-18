using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection
{
    private string url = "http://localhost/api/";

    public IEnumerator postRequest(JSONObject json, string path, System.Action<JSONObject> callBack) {
        JSONObject response;
        using (UnityWebRequest www = UnityWebRequest.Put(url + path, json.Print()))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();            

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);        
            }
            else
            {                
                response = new JSONObject(www.downloadHandler.text);       
                if(callBack != null) {        
                    callBack(response);   
                }          
            }
        }       
    }

    public IEnumerator postRequest(JSONObject json, string path) {
        JSONObject response;
        using (UnityWebRequest www = UnityWebRequest.Put(url + path, json.Print()))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();            

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);        
            }
            else
            {                
                response = new JSONObject(www.downloadHandler.text);                    
            }
        }       
    }

    public IEnumerator getRequest(string path, System.Action<JSONObject> callBack) {
        JSONObject response;         
        using (UnityWebRequest www = UnityWebRequest.Get(url + path))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();            

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);        
            }
            else
            {
                Debug.Log("Form upload complete!");
                response = new JSONObject(www.downloadHandler.text);
                Debug.Log(response.Print());
                callBack(response);
            }
        }       
    }

    public void setUrl(string value) {
        url = "http://" + value + "/api/";
    }
}
