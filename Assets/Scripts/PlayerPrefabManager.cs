using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabManager : MonoBehaviour
{
    // Store prefabs in a dictionary or list for easy access
    [System.Serializable]
    public class PlayerPrefabVariant
    {
        public GameObject prefab;  // The prefab to instantiate
        public ColorVariant colorVariant;  // Color variant (A, B, C, etc.)
        public string weaponType;  // Weapon type (e.g., "Pistol", "Rifle", etc.)
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

    // Method to swap prefabs based on weapon and color variant
    public void SwapPrefab(string weaponType, ColorVariant colorVariant)
    {
        // Find the matching prefab based on weaponType and colorVariant
        PlayerPrefabVariant selectedPrefab = FindPrefabByWeaponAndColor(weaponType, colorVariant);

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
        }
        else
        {
            Debug.LogError("Prefab not found for weapon type: " + weaponType + " and color variant: " + colorVariant);
        }
    }

    // This method searches for the correct prefab based on the selected weapon and color variant
    private PlayerPrefabVariant FindPrefabByWeaponAndColor(string weaponType, ColorVariant colorVariant)
    {
        foreach (var variant in playerPrefabs)
        {
            if (variant.weaponType == weaponType && variant.colorVariant == colorVariant)
            {
                return variant; // Return the matching prefab variant
            }
        }

        return null; // No matching prefab found
    }
}
