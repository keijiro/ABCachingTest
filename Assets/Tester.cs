using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour
{
    WWW TryDownload(int index)
    {
        var basePath = "file:" + Application.streamingAssetsPath;
        var fullPath = System.IO.Path.Combine(basePath, "textures" + index);
        return WWW.LoadFromCacheOrDownload(fullPath, 0);
    }

    IEnumerator Start()
    {
        Caching.CleanCache();
        Caching.maximumAvailableDiskSpace = 20 * 1024 * 1024;

        while (!Caching.enabled) yield return null;

        while (true)
        {
            for (var i = 0; i < 10; i++)
            {
                var www = TryDownload(i);
                yield return www;
                var ab = www.assetBundle;
                www.Dispose();

                ab.LoadAllAssets();
                ab.Unload(true);

                Debug.Log(i);
            }
            yield return new WaitForSeconds(1);
        }
    }

    void OnGUI()
    {
        var usedMB = Caching.spaceOccupied / (1024 * 1024);
        GUI.Label(new Rect(0, 0, 200, 100), "spaceOccupied: " + usedMB + "MB");
    }
}
