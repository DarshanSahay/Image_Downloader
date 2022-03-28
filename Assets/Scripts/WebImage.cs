using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebImage : MonoBehaviour
{
    private Base_ImageDownloader downloader;
    private string imageUrl;
    private string imageName;
    private bool canSave = false;

    [SerializeField] private GameObject panelImage;
    [SerializeField] private Text inputImgName;
    [SerializeField] private Text inputLink;
    [SerializeField] private Image texImage;
    
    private void Start()
    {
        downloader = Base_ImageDownloader.Instance;
    }
    public void OpenPanel()
    {
        panelImage.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        panelImage.gameObject.SetActive(false);
    }
    public void OnButtonClick()                                               //being called by OnClick button event to call function from
    {                                                                         //base downloader script and donwload the image
        if (inputLink.text.Length != 0 && inputImgName.text.Length != 0)
        {
            imageName = inputImgName.text;
            imageUrl = inputLink.text;
            downloader.RequestImage(imageName,imageUrl, texImage);
            if (canSave)                                                       //if enabled , the image will be saved locally after image gets downloaded
            {
                downloader.shouldSave = true;
            }
        }
        else
        {
            Debug.Log("Input Field Cannot Be Empty");
        }
    }
    public void LoadImage()                                       //being called by OnClick button event to load image from system
    {
        imageName = inputImgName.text;
        downloader.LoadImageOnDevice(imageName, texImage);
    }
}
