using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DoubleDCore.Web
{
    public static class WebRequest
    {
        public static async UniTask<Texture> DownloadTexture(string url)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading texture: {request.error}");
                return null;
            }

            Texture texture = DownloadHandlerTexture.GetContent(request);
            return texture;
        }
    }
}