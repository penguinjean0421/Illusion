using UnityEngine;
using System.Collections.Generic;

public class Store : MonoBehaviour
{
    [Header("Data Source")]
    // ScriptableObject 아이템 목록을 여기에 끌어다 놓습니다.
    public List<ItemData> allShopItems;
    public int maxDisplayStore = 6;

    [Header("UI References")]
    public GameObject shopItemSlotPrefab; // ShopItemSlot 스크립트가 붙은 프리팹
    public Transform contentParent;        // 아이템 슬롯들이 생성될 부모 Transform (ScrollView Content)

    // 구매 후 버튼 비활성화
    Dictionary<string, ShopItemSlot> itemSlotDictionary = new Dictionary<string, ShopItemSlot>();

    internal int playerGold = 0; // 현재 플레이어의 골드
    string boughtItem;

    public void SetupShopUI()
    {
        // 기존의 슬롯이 있다면 모두 제거
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        itemSlotDictionary.Clear();

        List<ItemData> shopDisplayItems = GetRandomItems(allShopItems, maxDisplayStore);

        foreach (ItemData itemData in shopDisplayItems)
        {
            if (itemSlotDictionary.ContainsKey(itemData.itemID))
            {
                Debug.LogWarning($"경고: 상점 디스플레이 아이템 목록에 중복된 ID가 있습니다. ID: {itemData.itemID}");
                continue;
            }

            GameObject slotObject = Instantiate(shopItemSlotPrefab, contentParent);
            ShopItemSlot slot = slotObject.GetComponent<ShopItemSlot>();

            slot.SetItemData(itemData);
            slot.OnBuyButtonClicked += BuyItem;
            slot.EnableButton();

            itemSlotDictionary.Add(itemData.itemID, slot);
        }

        playerGold += GameManager.instance.score;
        GameManager.instance.MoneyUpdate(playerGold);
        Debug.Log("상점 UI 로드 완료. 현재 골드: " + playerGold);
    }

    // 상점에 진열한 아이템 선별
    List<ItemData> GetRandomItems(List<ItemData> sourceList, int count)
    {
        List<ItemData> resultList = new List<ItemData>();

        List<ItemData> availableItems = new List<ItemData>(sourceList);

        int itemsToPick = Mathf.Min(count, availableItems.Count);

        for (int i = 0; i < itemsToPick; i++)
        {
            int randomIndex = Random.Range(0, availableItems.Count);

            ItemData selectedItem = availableItems[randomIndex];
            resultList.Add(selectedItem);

            availableItems.RemoveAt(randomIndex);
        }
        return resultList;
    }

    void BuyItem(string itemID)
    {
        // 1. 아이템 데이터 조회
        ItemData itemToBuy = allShopItems.Find(item => item.itemID == itemID);

        // 2. 재화 확인 (구매 가능 여부)
        if (playerGold >= itemToBuy.price)
        {
            // 3. 구매 처리
            playerGold -= itemToBuy.price; // 골드 차감 (실제로는 인벤토리/재화 관리자 호출)
            boughtItem = itemToBuy.itemName;

            GameManager.instance.BoughtItem(itemToBuy.itemID);

            // 아이템 지급 로직 (예시)
            Debug.Log($"**구매 성공**: {itemToBuy.Type} 타입 아이템 {itemToBuy.itemName}을(를) {itemToBuy.price} 골드로 구매했습니다.");
            Debug.Log($"남은 골드: {playerGold}");

            switch (itemToBuy.Type)
            {
                case (ItemData.ItemType.Upgrade):
                    ItemManager.instance.ObjeectScoreUp(itemToBuy.itemID);
                    ItemManager.instance.InsertWormHole(itemToBuy.itemID);
                    break;

                case (ItemData.ItemType.Object):
                    ItemManager.instance.AddTorque(itemToBuy.itemID);
                    ItemManager.instance.GravaityChange(itemToBuy.itemID);
                    ItemManager.instance.CoefficientValueChange(itemToBuy.itemID);
                    break;

                // case (ItemData.ItemType.Test):
                //     Debug.Log("Test Item");
                //     break;

                default:
                    break;
            }
            if (itemSlotDictionary.TryGetValue(itemID, out ShopItemSlot purchasedSlot)) { purchasedSlot.DisableButton(); }

            UpdatePlayerCurrencyUI(); // 구매 후 UI 갱신 (선택 사항: 재화 표시 등)
        }
        else
        {
            // 구매 실패
            Debug.LogWarning($"구매 실패: 골드가 부족합니다! (필요: {itemToBuy.price}, 현재: {playerGold})");
        }
    }

    void UpdatePlayerCurrencyUI()
    {
        // TODO: 게임 내 재화 표시 UI를 업데이트하는 코드를 여기에 작성하세요.
        // 예를 들어: UIManager.Instance.UpdateGoldText(playerGold);

        GameManager.instance.MoneyUpdate(playerGold);
        GameManager.instance.BuyItem(boughtItem);
    }
}