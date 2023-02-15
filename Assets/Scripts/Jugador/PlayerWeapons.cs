using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public WeaponInventory originalInventory;
    public Animator animator;
    List<Weapon> inventory;
    Weapon currentWeapon = null;
    float timer = 0;
    Coroutine reloading;
    public static Action<int, int> onBulletCountChange;
    public AudioSource sonidoArma;
    void Start()
    {
        inventory = originalInventory.weapons;
        if (inventory.Count == 0)
        {
            Debug.LogError("El inventario esta vacio!");
            return;
        }
        currentWeapon = inventory[0];
        onBulletCountChange?.Invoke(currentWeapon.bulletCount, currentWeapon.magazineSize);
    }
    void Update()
    {
        if (reloading != null)
            return;
        if (Input.GetMouseButton(0) && currentWeapon.fireType == FireType.Auto)
        {
            Shoot();
        }
        else if (Input.GetMouseButtonDown(0) && currentWeapon.fireType == FireType.Semi)
        {
            Shoot();
        }
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (reloading == null)
            {
                reloading = StartCoroutine(Reload());
            }
        }
    }
    void Shoot()
    {
        if (currentWeapon.timeToShoot < timer && currentWeapon.bulletCount >= 0)
        {
            onBulletCountChange?.Invoke(currentWeapon.bulletCount, currentWeapon.magazineSize);
            timer = 0;
            //Gastando la bala
            currentWeapon.bulletCount--;
            RaycastHit hit; //Informaci�n de donde da �l rayo
            //Rayo desde la mitad de la pantalla
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            //Invocamos al rayo
            Physics.Raycast(ray.origin, ray.direction, out hit, currentWeapon.range);
            if (hit.collider)
            {
                EnemyHealth enemy = hit.collider.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.Damage(currentWeapon.damage);
                }
            }
            sonidoArma.Play();
            animator.SetTrigger("Disparo");
        }
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(currentWeapon.timeToReload);
        currentWeapon.bulletCount = currentWeapon.magazineSize;
        onBulletCountChange?.Invoke(currentWeapon.bulletCount, currentWeapon.magazineSize);
        animator.SetTrigger("Recarga");
        reloading = null;
    }
}
