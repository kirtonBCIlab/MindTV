using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System;
using static SimpleFileBrowser.FileBrowserSFB;
using UnityEngine.Networking;
using System.Collections;

//handles uploading image by the user
public class ItemUpload : MonoBehaviour
{
    public Image imagePlaceholder;
    public GameObject inventorySlotPrefab;
    public GameObject inventory;
    public GameObject addItemDialog;

    private byte[] lastUploadedBytes;

    private void OnPhotoDialogSaved(string[] paths)
    {
        //reads and loads the bytes from the user selected image
        if (paths.Length != 0)
        {
            var path = paths[0];
            lastUploadedBytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new(1, 1);
            loadTexture.LoadImage(lastUploadedBytes);

            imagePlaceholder.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }

    private void OnDialogCanceled()
    {
        Debug.Log("Canceled");
    }

    //opens file explorer in Unity using FileBrowser
    public void OpenImagePathPanel()
    {
        //on success go to OnPhotoDialogSaved
        OnSuccess ls = OnPhotoDialogSaved;
        //on cancellation go to OnDialogCanceled
        OnCancel lc = OnDialogCanceled;
        //only .png is acceptable
        SetFilters(false, new string[] { ".png" });
        //opens the loading dialog for user to choose desired image to upload
        ShowLoadDialog(ls, lc, PickMode.Files);
    }

    //creates a new image for training object selection
    public void OnClickCreate()
    {
        if (lastUploadedBytes != null)
        {
            CreateItem();
            addItemDialog.SetActive(false);
        }
    }

    //creates new item for training object based on user selected image
    private void CreateItem()
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

            SpriteRenderer spriteRenderer = trainingItem.GetComponent<SpriteRenderer>();
            Image image = trainingItem.GetComponent<Image>();
            TrainingItem trainingItemScript = trainingItem.GetComponent<TrainingItem>();

            image.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
            spriteRenderer.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
            trainingItemScript.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 110.0f);
        }
    }
}
