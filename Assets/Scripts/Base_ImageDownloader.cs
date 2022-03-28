using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class Base_ImageDownloader : MonoBehaviour
{
    #region Links for References Download
    private string url1 = "https://images.easytechjunkie.com/computer-downloading-status-bar.jpg";
    private string url2 = "https://thumbs.dreamstime.com/z/landscape-nature-mountan-alps-rainbow-76824355.jpg";
    #endregion

    public static Base_ImageDownloader Instance;
    public bool shouldSave = false;
    private int count = 0;
    private string[] files;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AutoFormatData();
    }
    private void AutoFormatData()                                          //checks for the data inside the system and deletes them after 7 days from the day it was created
    {
        files = Directory.GetFiles(Application.persistentDataPath);
        if(files != null)
        {
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < System.DateTime.Now.AddDays(-7))
                {
                    fileInfo.Delete();
                }
            }
        }
    }
    public void RequestImage(string imageName,string imageURL,Image newImage)             
    {
        StartCoroutine(GetImage(imageName,imageURL, newImage));
        count++;
    }
    private IEnumerator GetImage(string imageName,string imageURL, Image newImage)           //requesting image through unity web networking
    {
        if(count < 4)
        {
            UnityWebRequest getImage = UnityWebRequestTexture.GetTexture(imageURL);
            getImage.timeout = 10;

            yield return getImage.SendWebRequest();

            if(getImage.result == UnityWebRequest.Result.ConnectionError ||
               getImage.result == UnityWebRequest.Result.DataProcessingError ||
               getImage.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("There is Some Error");
                Debug.Log(getImage.error);
            }
            else
            {                                                                                     //setting up data for the sprite of image
                Texture2D imageTex = DownloadHandlerTexture.GetContent(getImage);         
                newImage.sprite = Sprite.Create(imageTex, new Rect(0f, 0f, imageTex.width, imageTex.height), Vector2.zero);
                if (shouldSave)
                {
                    SaveImageOnDevice(imageName, imageTex);                             //will save if boolean value is true changed through webimage script
                    shouldSave = false;
                }
            }
            count--;
        }
        else
        {
            count--;
            Debug.Log("3 images already being downloaded");
        }
    }
    private void SaveImageOnDevice(string imageName,Texture2D tex)                       //converts data to bytes and save it locally with the imagename
    {
        byte[] dataByte = tex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + imageName, dataByte);
        Debug.Log("Image Saved");
        Debug.Log(Application.persistentDataPath + imageName);
    }
    public void LoadImageOnDevice(string imageName,Image loadImage)                     //converts the data from byte to texture and sets the texture to sprite
    {
        byte[] getByte = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);
        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(getByte);
        loadImage.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
        Debug.Log("Image Loaded");
    }
}
