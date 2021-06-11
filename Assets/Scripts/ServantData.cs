using UnityEngine;

[CreateAssetMenu(menuName = "Servant Data", fileName = "New Servant", order = 0)]
public class ServantData : ScriptableObject
{
	public PNTemplate.Projectile ProjectilePrefab;
	public float FireRate = .24f;
	public float ProjectileSpeed;
	public int ProjectileDamage;
	public float SightRadius = 8;
}