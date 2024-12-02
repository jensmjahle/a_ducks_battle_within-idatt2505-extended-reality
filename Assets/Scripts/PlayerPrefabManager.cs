using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabManager : MonoBehaviour
{
    // Store prefabs in a dictionary or list for easy access
    [System.Serializable]
    public class PlayerPrefabVariant
    {
        public GameObject prefab;       // The prefab to instantiate
        public WeaponType weaponType;   // Weapon type (e.g., Pistol, Rifle, etc.)
        public ColorVariant colorVariant;  // Color variant (e.g., A, B, C)
        public PlayerDirection direction;  // Direction variant (e.g., Up, Down, Side)
    }

    // A list to store different prefab variants
    public List<PlayerPrefabVariant> playerPrefabs;

    private GameObject currentPlayerPrefab;

    // Reference to PlayerController
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Method to swap prefabs based on weapon type, color variant, and direction
    public GameObject SwapPrefab(WeaponType weaponType, ColorVariant colorVariant, PlayerDirection direction)
    {
        // Find the matching prefab based on weaponType, colorVariant, and direction
        PlayerPrefabVariant selectedPrefab = FindPrefabByWeaponColorAndDirection(weaponType, colorVariant, direction);

        if (selectedPrefab != null)
        {
            // Destroy the current prefab if it exists
            if (currentPlayerPrefab != null)
            {
                Destroy(currentPlayerPrefab);
            }

            // Instantiate the new prefab and set it as the new player
            currentPlayerPrefab = Instantiate(selectedPrefab.prefab, transform.position, transform.rotation);
            currentPlayerPrefab.transform.SetParent(transform); // Keep it under the same parent
            return currentPlayerPrefab;// Return the new prefab
        }
        else
        {
            Debug.LogError("Prefab not found for weapon type: " + weaponType + ", color variant: " + colorVariant + ", and direction: " + direction);
            return null; // Return null if no prefab is found
        }
       
    }

    // This method searches for the correct prefab based on the selected weapon type, color variant, and direction
    private PlayerPrefabVariant FindPrefabByWeaponColorAndDirection(WeaponType weaponType, ColorVariant colorVariant, PlayerDirection direction)
    {
        foreach (var variant in playerPrefabs)
        {
            if (variant.weaponType == weaponType && variant.colorVariant == colorVariant && variant.direction == direction)
            {
                return variant; // Return the matching prefab variant
            }
        }

        return null; // No matching prefab found
    }


}
