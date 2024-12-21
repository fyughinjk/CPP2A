using UnityEngine;

public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 2f; // Example: double the speed
    public override void OnPickup(GameObject player)
    {
        // Example: if your player script has a reference to speed
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.StartCoroutine(ApplySpeedBoost(pc));
        }

        // Destroy the power-up in the scene
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ApplySpeedBoost(PlayerController pc)
    {
        float originalSpeed = pc.GetSpeed();
        pc.SetSpeed(originalSpeed * speedMultiplier);

        // Wait for duration
        yield return new WaitForSeconds(duration);

        // Revert speed
        pc.SetSpeed(originalSpeed);
    }
}
