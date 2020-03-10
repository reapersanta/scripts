using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : MonoBehaviour
{
    //TODO shooting profile for ease of use (use scriptable objects for the profiles)
    [SerializeField] private GameObject BulletObject;
    [SerializeField] private Transform GunTip;
    [SerializeField] private int BulletsPerArray;
    [Range(0f,360f)]
    [SerializeField] private float IndividualArraySpread;

    [SerializeField] private int TotalBulletArray;
    [Range(0f,360f)]
    [SerializeField] private float TotalArraySpread;

    [Range(0f,360f)]
    [SerializeField] private float SpinSpeed;

    [SerializeField] private float DelayBetweenFires;
    [SerializeField] private float FireRate;
    [SerializeField] private int BulletSpeed;// currently not working with the current Bullet Behaviour script
    [SerializeField] private float BulletRotationSpeed;
    
    private float _fireRate;
    private float _spin;

    private bool _canShoot;
    private float _delayFire;
    //private List<Transform> _gunTips;

    private void Start()
    {
        // for(int i = 0; i < TotalBulletArray ; i++)
        // {
        //     _gunTips.Add(Instantiate(new GameObject(),GunTip.position,Quaternion.identity).GetComponent<Transform>());
        // }
    }
    void Update()
    {

        if(_fireRate <= 0f && _canShoot)
        {
            Shoot();
            _fireRate = FireRate;
        }else
        {
            _fireRate -= Time.deltaTime;
        }
        //rotate
        _spin += SpinSpeed*Time.deltaTime;
        if(_delayFire <= 0f)
        {
            SwitchShoot();
            _delayFire = DelayBetweenFires;
        } else
        {
            _delayFire -= Time.deltaTime;
        }
    }

    private void SwitchShoot()
    {
        _canShoot = !_canShoot;
    }

    void Shoot()
    {
        if(TotalBulletArray < 1 || BulletsPerArray < 1)
        {
            return;
        }
        float totalArraySpread = 0;
        for(int i = 0; i < TotalBulletArray; i++)
        {
            GunTip.rotation = Quaternion.Euler(0f,0f,_spin);
            GunTip.Rotate(Vector3.forward * totalArraySpread);
            float indBulletSpread = 0;
            for(int x = 0; x < BulletsPerArray; x++)
            {
                GunTip.Rotate(Vector3.forward * indBulletSpread);
                var g = Instantiate(BulletObject,GunTip.position,GunTip.rotation);g.GetComponent<BulletBehavior>().Speed = BulletSpeed;
                if(BulletRotationSpeed > 0f)
                {
                    g.AddComponent<RotateObject>().RotationSpeed.z = BulletRotationSpeed;
                }
                indBulletSpread += IndividualArraySpread;
            }
            totalArraySpread += TotalArraySpread;
        }
    }
}
