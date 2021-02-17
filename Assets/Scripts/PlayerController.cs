using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] ForestBehaviour _forestBehaviour;

    List<GameObject> bulletPool = new List<GameObject>();
    
    public void ShootTheBullet()
    {
        var bullet = GetPooledBullet(); // Gets the bullet from object pooler
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++) // Checks if there is bullets which deactivated and can be used
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].SetActive(true); // Activates the bullet and returns it
                return bulletPool[i];
            }
        }
        // If there is no bullet to use, instantiates new bullet and returns it
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.GetComponent<BulletBehaviour>().forestBehaviour = _forestBehaviour;
        bulletPool.Add(bullet);
        return bullet;
    }
}
