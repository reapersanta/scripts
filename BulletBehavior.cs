using System;
using UnityEngine;


// this class is for the bullet prefab
public class BulletBehavior : MonoBehaviour
{
// feel free to add some type of Bullet
	public enum TypeOfBullet
	{
		Constant,
		Punchy,
		Rocket,
	}
	// sorry i don't know how to name variables right, feel free to change or modify
	public TypeOfBullet BulletType;
	public bool MoveWithTransform = true;
	public float TimeToDestroy = 8f;
	public float Speed;
	[Header("If Punchy or Rocket")] 
	public bool UseAsBoomerang;
	public float MoveIncrement;
	[Range(1f, 5f)] public float Punchyness;
	[Range(1f, 50f)] public float Damping;
	[Header("If not moving with transform")]
	public Vector2 FreeMovement;
	public GameObject Explosion;
	public int BulletDamage;

	private Transform _myTransform;
    
    private void Start()
    {
	    _myTransform = this.transform;
        Invoke(nameof(Kill),TimeToDestroy);
    }

    void Update()
    {
        if(MoveWithTransform) // when moveWithTransform is True it will move with rotation according to its transform.Up
        {
	        switch (BulletType)
	        {
		        case TypeOfBullet.Constant:
			        _myTransform.position += Time.deltaTime * Speed * _myTransform.up; 
			        break;
		        case TypeOfBullet.Punchy:
			        _myTransform.position += Time.deltaTime * MoveIncrement * Punchyness *
			                                 _myTransform.up;
			        if (MoveIncrement <= 0f && !UseAsBoomerang)
			        {
				        MoveIncrement = 0f;
				        return;
			        }
			        MoveIncrement -= Time.deltaTime * Damping;
			        break;
		        case TypeOfBullet.Rocket:
			        _myTransform.position += Time.deltaTime * MoveIncrement * _myTransform.up;
			        if (MoveIncrement <= 0f && !UseAsBoomerang)
			        {
				        MoveIncrement = 0f;
				        return;
			        }
			        MoveIncrement += Time.deltaTime * Punchyness;
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        } else // when moveWithTransform is false it will move through its axis
        {
            _myTransform.position += (Vector3)FreeMovement*Time.deltaTime;
        }
    }

    	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<ITakeDamage>()?.TakeDamage(BulletDamage);
		    Kill();
        } else if (other.gameObject.CompareTag("Block"))
        {
            Kill();
        }
	}

	private void Kill()
	{
		Instantiate(Explosion, _myTransform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}
}
