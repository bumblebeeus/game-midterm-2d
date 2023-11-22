using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DataBase;
using UnityEngine.Assertions;

public class ShopManager : MonoBehaviour
{
    private DataBase.Player schemaPlayer = DataBase.Player.getCurrentPlayer();

    public TMP_Text coinUI;
    public ShopItemSO[] shopItemSO;
    public GameObject[] shopPanelsGO;
    public Shoptemplate[] shopPanels;
    public Button[] purchaseButtons;

    void Start()
    {
        StartCoroutine(DataBase.Skin.listShopSkins((sucess, skins) => {
            if (sucess) {
                // initialize the shopItemSO array with size of 2 (we expect only 2 items)
                shopItemSO = new ShopItemSO[2];
                for (int i = 0; i < shopItemSO.Length; i++){
                    // Create a new instance of ShopItemSO
                    ShopItemSO newShopItem = ScriptableObject.CreateInstance<ShopItemSO>();
                    shopItemSO[i] = newShopItem;
                    /*
                    shopItemSO[i].baseCost = ...;
                    shopItemSO[i].title = ...;
                    shopItemSO[i].image = ...;
                    */
                    shopItemSO[i].baseCost = skins[i].price;
                    shopItemSO[i].title = "Skin " + i.ToString();
                    shopItemSO[i].image = Resources.Load<Sprite>("skin" + (i + 1).ToString());
                    // about the Resources.Load, please read the docs here:
                    // https://docs.unity3d.com/ScriptReference/Resources.Load.html
                    // Hint: save the images of sprites in the Resources folder

                    shopPanelsGO[i].SetActive(true); // you better not change this line or you will be fucked up
                }
                coinUI.text = "Your coins: " + schemaPlayer.coins;
                loadPanels();
                CheckPurchaseable();
            } else {
                Debug.Log("Cannot request from db");
            }
        }));

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void AddCoins(int amount){
    //     coins += amount;
    //     coinUI.text = coins.ToString();
    //     CheckPurchaseable();
    // }

    public void CheckPurchaseable(){
        StartCoroutine(PlayerSkin.getAllSkins(schemaPlayer.username, (success, playerSkins) => {
            for (int i = 0; i < shopItemSO.Length; i++){
                bool isBought = false;
                for (int j = 0; j < playerSkins.Length; j++) {
                    if (playerSkins[j].skin_id == i + 1) {
                        isBought = true;
                        break;
                    }
                }

                if (isBought) {
                    if (schemaPlayer.current_skin == i + 1) {
                        purchaseButtons[i].interactable = false;
                        purchaseButtons[i].GetComponentInChildren<TMP_Text>().text = "Equipped";
                    } else {
                        purchaseButtons[i].interactable = true;
                        purchaseButtons[i].GetComponentInChildren<TMP_Text>().text = "Equip";
                    }
                } else {
                    if (schemaPlayer.coins >= shopItemSO[i].baseCost) {
                        purchaseButtons[i].interactable = true;
                    } else {
                        purchaseButtons[i].interactable = false;
                        // example of how to change the text on the button:
                        // I think the text on the button will be "Purchase" -> "Equip" -> "Equipped" (disable: interactable = false)
                        purchaseButtons[i].GetComponentInChildren<TMP_Text>().text = "Not enough coins";
                    }
                }
            }
        }));
    }


   
    public void PurchaseItem(int index){
        if (purchaseButtons[index].GetComponentInChildren<TMP_Text>().text == "Equip") {
            schemaPlayer.current_skin = index + 1;
            StartCoroutine(schemaPlayer.updateBasicInfo((success) => {
            if (success) {
                Debug.Log("Equip successfully");
                CheckPurchaseable();
            } else {
                Debug.Log("Error in Equip");
            }
        }));
        } else if (schemaPlayer.coins >= shopItemSO[index].baseCost){
            schemaPlayer.coins -= shopItemSO[index].baseCost;
            coinUI.text = "Your coins: " + schemaPlayer.coins.ToString();
            StartCoroutine(PlayerSkin.create(schemaPlayer.username, index + 1, (success) => {
                if (success) {
                    Debug.Log("Purchase successfully");
                    CheckPurchaseable();
                } else {
                    Debug.Log("Error in Purchase");
                }
            }));
        }
    }

    // this function is to set up the panels of scroll view in shop to be rendered as the shopItemSO array
    public void loadPanels(){
        for (int i = 0; i < shopItemSO.Length; i++){
            shopPanels[i].title.text = shopItemSO[i].title;
            shopPanels[i].itemImage.sprite = shopItemSO[i].image;
            shopPanels[i].purchasePrice.text = "Coins: " + shopItemSO[i].baseCost.ToString();
        }
    }

}
