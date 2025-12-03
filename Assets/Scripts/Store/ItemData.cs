using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Shop Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemType Type;
    public enum ItemType
    {
        Test,
        Consumable, // 소모품
        Equipment,  // 장비
        Currency,    // 재화
        Upgrade
    }

    // 상점에 표시될 정보
    public string itemID; // 고유번호
    public string itemName; // 이름
    public string itemDescription; // 설명
    public int price; // 가격
    public Sprite itemIcon; // 아이콘
}
