using UnityEngine;
using System.Collections.Generic;

public class Store : MonoBehaviour
{
    [Header("Data Source")]
    // ScriptableObject 아이템 목록을 여기에 끌어다 놓습니다.
    public List<ItemData> allShopItems;

    [Header("UI References")]
    public GameObject shopItemSlotPrefab; // ShopItemSlot 스크립트가 붙은 프리팹
    public Transform contentParent;        // 아이템 슬롯들이 생성될 부모 Transform (ScrollView Content)

    // ********** 시뮬레이션 데이터 **********
    private int playerGold = 500; // 현재 플레이어의 골드
    // ****************************************

    void Start()
    {
        // 씬 시작 시 상점 UI를 구성합니다.
        SetupShopUI();
    }

    private void SetupShopUI()
    {
        // 기존의 슬롯이 있다면 모두 제거 (상점 갱신 시 유용)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 아이템 목록을 순회하며 슬롯을 생성합니다.
        foreach (ItemData itemData in allShopItems)
        {
            // 1. 슬롯 UI 생성
            GameObject slotObject = Instantiate(shopItemSlotPrefab, contentParent);
            ShopItemSlot slot = slotObject.GetComponent<ShopItemSlot>();

            // 2. 데이터 설정
            slot.SetItemData(itemData);

            // 3. 구매 이벤트 등록 (가장 중요한 부분)
            // 슬롯에서 발생한 구매 클릭 이벤트를 ShopManager의 BuyItem 함수로 연결합니다.
            slot.OnBuyButtonClicked += BuyItem;
        }

        Debug.Log("상점 UI 로드 완료. 현재 골드: " + playerGold);
    }

    /// <summary>
    /// 아이템 구매를 처리하는 핵심 로직
    /// </summary>
    /// <param name="itemID">구매할 아이템의 ID</param>
    private void BuyItem(string itemID)
    {
        // 1. 아이템 데이터 조회
        ItemData itemToBuy = allShopItems.Find(item => item.itemID == itemID);

        if (itemToBuy == null)
        {
            Debug.LogError($"오류: ID {itemID}에 해당하는 아이템 데이터를 찾을 수 없습니다.");
            return;
        }

        // 2. 재화 확인 (구매 가능 여부)
        if (playerGold >= itemToBuy.price)
        {
            // 3. 구매 처리
            playerGold -= itemToBuy.price; // 골드 차감 (실제로는 인벤토리/재화 관리자 호출)

            // 아이템 지급 로직 (예시)
            Debug.Log($"**구매 성공**: {itemToBuy.itemName}을(를) {itemToBuy.price} 골드로 구매했습니다.");
            Debug.Log($"남은 골드: {playerGold}");

            // TODO: 여기에 실제 인벤토리에 아이템을 추가하는 로직을 구현하세요.
            // if (itemToBuy.Type == ItemData.ItemType.Consumable)
            // {
            //     InventoryManager.Instance.AddItem(itemToBuy.ItemID, 1);
            // }

            // 구매 후 UI 갱신 (선택 사항: 재화 표시 등)
            UpdatePlayerCurrencyUI();
        }
        else
        {
            // 구매 실패
            Debug.LogWarning($"구매 실패: 골드가 부족합니다! (필요: {itemToBuy.price}, 현재: {playerGold})");
        }
    }

    // 예시 함수: 플레이어 재화 UI 업데이트 (실제 UI 텍스트 업데이트 로직)
    private void UpdatePlayerCurrencyUI()
    {
        // TODO: 게임 내 재화 표시 UI를 업데이트하는 코드를 여기에 작성하세요.
        // 예를 들어: UIManager.Instance.UpdateGoldText(playerGold);
    }
}