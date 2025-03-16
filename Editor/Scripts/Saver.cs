using UnityEngine;

public static class Saver
{
    public enum SaveTextureFileFormat
    {
        EXR, JPG, PNG, TGA
    };

    public static void SaveTexture2DToFile(Texture2D tex, string filePath, SaveTextureFileFormat fileFormat, int jpgQuality = 95)
    {
        switch (fileFormat)
        {
            case SaveTextureFileFormat.EXR:
                System.IO.File.WriteAllBytes(filePath + ".exr", tex.EncodeToEXR());
                break;
            case SaveTextureFileFormat.JPG:
                System.IO.File.WriteAllBytes(filePath + ".jpg", tex.EncodeToJPG(jpgQuality));
                break;
            case SaveTextureFileFormat.PNG:
                System.IO.File.WriteAllBytes(filePath + ".png", tex.EncodeToPNG());
                break;
            case SaveTextureFileFormat.TGA:
                System.IO.File.WriteAllBytes(filePath + ".tga", tex.EncodeToTGA());
                break;
        }
    }

    public static void SaveRenderTextureToFile(RenderTexture renderTexture, string filePath, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95)
    {
        Texture2D tex;
        if (fileFormat != SaveTextureFileFormat.EXR)
            tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);
        else
            tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false, true);
        var oldRt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = oldRt;
        SaveTexture2DToFile(tex, filePath, fileFormat, jpgQuality);
        if (Application.isPlaying)
            Object.Destroy(tex);
        else
            Object.DestroyImmediate(tex);

    }

    public static void SaveAsAsset(Object asset, string assetPath)
    {
        UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Saved Asset to " + assetPath);
    }

    public static string GetPathWithPostfix(Object obj, string postfix)
    {
        // Get the path of the original Texture2D asset
        string originalTexturePath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        string directoryPath = System.IO.Path.GetDirectoryName(originalTexturePath);

        // Extract the base file name (without extension) from the original texture path
        string originalFileName = System.IO.Path.GetFileNameWithoutExtension(originalTexturePath);

        string fileName = originalFileName + postfix;
        string filePath = System.IO.Path.Combine(directoryPath, fileName);

        return filePath;
    }
}
