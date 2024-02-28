using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Collections;
using SimpleFileBrowser;

//handles uploading image by the user
public class ItemUpload : MonoBehaviour
{
    public Image imagePlaceholder;
    public GameObject inventorySlotPrefab;
    public GameObject inventory_1;
    public GameObject inventory_2;
    public GameObject inventory_3;
    public GameObject inventory_4;

    public GameObject addItemDialog;

    private byte[] lastUploadedBytes;


    private void OnPhotoDialogSaved(string[] paths)
    {
        //reads and loads the bytes from the user selected image
        if (paths.Length != 0)
        {
            var path = paths[0];
            string FileName = null;
            FileName = Path.GetFileName(path);
            lastUploadedBytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new(1, 1);
            loadTexture.LoadImage(lastUploadedBytes);

            imagePlaceholder.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            // resizee uploaded picture to 500 by 500 and save the resized photo as png in the icons folder 
            int targetX = 500;
            int targetY = 500;
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(loadTexture,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            loadTexture = result;
            SaveImageToPng( loadTexture, FileName);
        }
    }
    private void SaveImageToPng(Texture2D PhotoTex, string FileName)
    {
        //save the photo to icons folder 
        byte[] PhotoBytes = ImageConversion.EncodeToPNG(PhotoTex);
        File.WriteAllBytes(Application.dataPath + "/../Assets/Icons/" + FileName +".png", PhotoBytes);
    }

    private void OnDialogCanceled()
    {
        Debug.Log("Canceled");
    }

    //opens file explorer in Unity using FileBrowser
    public void OpenImagePathPanel()
    {
        //on success go to OnPhotoDialogSaved
        FileBrowser.OnSuccess ls = OnPhotoDialogSaved;
        //on cancellation go to OnDialogCanceled
        FileBrowser.OnCancel lc = OnDialogCanceled;
        //only .png is acceptable
        FileBrowser.SetFilters(false, new string[] { ".png" });
        //opens the loading dialog for user to choose desired image to upload
        FileBrowser.ShowLoadDialog(ls, lc, FileBrowser.PickMode.Files);
    }

    //creates a new image for training object selection
    public void OnClickCreate()
    {
        if (lastUploadedBytes != null)
        {
            CreateItem(inventory_1);
            CreateItem(inventory_2);
            CreateItem(inventory_3);
            CreateItem(inventory_4);
         
            addItemDialog.SetActive(false);
        }
    }

    //creates new item for training object based on user selected image
    private void CreateItem(GameObject inventory)
    {
        GameObject newItem = Instantiate(inventorySlotPrefab);
        newItem.transform.SetParent(inventory.transform);
        newItem.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
        newItem.transform.localScale = new Vector3(1, 1, 1);
        newItem.transform.SetSiblingIndex(inventory.transform.childCount - 2);

        Transform trainingItem = newItem.transform.Find("TrainingItem");
        Transform frame = newItem.transform.Find("Frame");

        if (trainingItem != null)
        {
            Texture2D loadTexture = new(1, 1);
            loadTexture.LoadImage(lastUploadedBytes);

            // resizee uploaded picture to 500 by 500 
            int targetX = 500;
            int targetY = 500;
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(loadTexture,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            loadTexture = result;

     
            SpriteRenderer spriteRenderer = trainingItem.GetComponent<SpriteRenderer>();
            Image image = trainingItem.GetComponent<Image>();
            TrainingItem trainingItemScript = trainingItem.GetComponent<TrainingItem>();

            image.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
            spriteRenderer.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
            trainingItemScript.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
        }
    }
}
