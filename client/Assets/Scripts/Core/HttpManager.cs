using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoSingleton<HttpManager>
{
    public void SendPostRequest(string url, object data, Action<string> callback)
    {
        StartCoroutine(DoSendPostRequest(url, data, callback));
    }

    private IEnumerator DoSendPostRequest(string url, object data, Action<string> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            string jsonData = JsonConvert.SerializeObject(data);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            request.uploadHandler.contentType = "application/json";

            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                
                callback.Invoke(json);
            }
            else
            {
                callback.Invoke(null);
            }
        }
    }
}