using UnityEngine;
using UnityEngine.UI;
using System;


public class ShopItemSlot : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public Text nameText;
    public Text descriptionText;
    public Text priceText;
    public Button buyButton;
    // 구매 이벤트가 발생했을 때 호출할 델리게이트 (ItemID를 넘겨줍니다)
    public Action<string> OnBuyButtonClicked;

    ItemData currentItemData;

    public void SetItemData(ItemData data)
    {
        currentItemData = data;

        // UI 업데이트
        iconImage.sprite = data.itemIcon;
        nameText.text = data.itemName;
        descriptionText.text = data.itemDescription;
        priceText.text = data.price.ToString() + " G\n Buy";

        // 구매 버튼 이벤트 연결
        // 버튼 클릭 시 외부(ShopManager)에 이 아이템의 ID로 구매 요청을 보냅니다.
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            if (OnBuyButtonClicked != null)
            {
                OnBuyButtonClicked(currentItemData.itemID);
            }
        });
    }

    // 구매 버튼 비활성화
    public void DisableButton()
    {
        if (buyButton != null)
        {
            buyButton.interactable = false;
        }
    }

    public void EnableButton()
    {
        if (buyButton != null)
        {
            buyButton.interactable = true;
        }
    }
}