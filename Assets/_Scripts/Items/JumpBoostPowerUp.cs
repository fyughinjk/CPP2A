using UnityEngine;

public class JumpBoostPowerUp : PowerUp
{
    public float jumpMultiplier = 1.5f; // 50% stronger jump

    public override void OnPickup(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.StartCoroutine(ApplyJumpBoost(pc));
        }
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ApplyJumpBoost(PlayerController pc)
    {
        float originalJumpForce = pc.jumpForce;
        pc.jumpForce = originalJumpForce * jumpMultiplier;

        yield return new WaitForSeconds(duration);

        pc.jumpForce = originalJumpForce;
    }
}
