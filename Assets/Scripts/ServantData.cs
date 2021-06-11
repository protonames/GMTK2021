using PNTemplate;
using UnityEngine;

[CreateAssetMenu(menuName = "Servant Data", fileName = "New Servant", order = 0)]
public class ServantData : ScriptableObject
{
	public float FireRate = .24f;
	public int ProjectileDamage;
	public Projectile ProjectilePrefab;
	public float ProjectileSpeed;
	public float SightRadius = 8;
}