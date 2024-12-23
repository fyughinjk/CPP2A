using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class WeaponUI : MonoBehaviour
{
    public Image weaponImage;      // The main UI Image
    public Sprite unarmedSprite;   // Drag your fist icon
    public Sprite swordSprite;     // Drag your sword icon
    public Sprite magicSprite;     // Drag your magic icon

    // Called by PlayerController or you poll PlayerController in Update
    public void UpdateWeaponIcon(WeaponType currentWeapon)
    {
        switch (currentWeapon)
        {
            case WeaponType.Unarmed:
                weaponImage.sprite = unarmedSprite;
                break;
            case WeaponType.Sword:
                weaponImage.sprite = swordSprite;
                break;
            case WeaponType.Magic:
                weaponImage.sprite = magicSprite;
                break;
        }
    }
}
