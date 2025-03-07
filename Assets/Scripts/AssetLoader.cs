using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public enum FileExtensions
{
    ScriptableObject, Prefab, Sprite
    //Add more file extensions here
}

public abstract class AssetLoader<T> where T : Object
{
    public static List<T> LoadAllAssets(string path, FileExtensions fileExtension)
    {
        var assets = new List<T>();
        var absoluteFolderPath = Path.Combine(Application.dataPath, path);

        if (!Directory.Exists(absoluteFolderPath))
        {
            Debug.LogError($"Directory does not exist: {absoluteFolderPath}");
            return null;
        }

        List<string> extensions = GetExtensions(fileExtension);
        if (extensions == null || extensions.Count == 0)
        {
            Debug.LogError("Was not able to find extension for the file.");
            return null;
        }

        FindAllAssets(assets, absoluteFolderPath, extensions);
        Debug.Log($"Assets found: {assets.Count}");
        return assets;
    }

    private static List<string> GetExtensions(FileExtensions fileExtension)
    {
        return fileExtension switch
        {
            FileExtensions.ScriptableObject => new List<string> { ".asset" },
            FileExtensions.Prefab => new List<string> { ".prefab" },
            FileExtensions.Sprite => new List<string> { ".png", ".jpg", ".jpeg", ".psd", ".tga" },
            _ => null
        };
    }

    private static void FindAllAssets(List<T> assets, string absoluteFolderPath, List<string> extensions)
    {
        try
        {
            foreach (string extension in extensions)
            {
                var files = Directory.GetFiles(absoluteFolderPath, $"*{extension}");

                foreach (var file in files)
                {
                    var relativePath = "Assets" + file.Replace(Application.dataPath, "").Replace("\\", "/");
                    var asset = AssetDatabase.LoadAssetAtPath<T>(relativePath);

                    if (asset != null)
                        assets.Add(asset);
                }
            }

            var directories = Directory.GetDirectories(absoluteFolderPath);
            foreach (var directory in directories)
                FindAllAssets(assets, directory, extensions);
        }
        catch (IOException e)
        {
            Debug.LogException(e);
        }
    }
}