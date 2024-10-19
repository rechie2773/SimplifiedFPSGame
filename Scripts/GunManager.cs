using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
public class GunManager : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    

    //Graphics
    public ParticleSystem muzzleFlash ;
    public GameObject BulletImpact;
    public float impactForce = 100f;
    public TextMeshProUGUI text;

    //Audio
    public AudioSource FireSound;
    public AudioSource ReloadingSound;
    //light for gun
    public Light muzzleFlashLight;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = false;
        }
    }
    private void Update()
    {
        MyInput();
       
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (!PauseMenu.paused)
        {

            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

            //Shoot
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        FireSound.Play();
        muzzleFlash.Play();
        readyToShoot = false;

        //Light
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = true;
            Invoke("DisableMuzzleFlashLight", timeBetweenShooting);
        }

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            
            Enemy enemy = rayHit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            //hit object with physic
            if (rayHit.rigidbody != null)
            {
                rayHit.rigidbody.AddForce(-rayHit.normal * impactForce);
            }
            //bullet hole effect
            GameObject DesyncImpact = Instantiate(BulletImpact, rayHit.point, Quaternion.LookRotation(rayHit.normal));
            //delete bullet hold after x seconds
            Destroy(DesyncImpact, 0.5f);
        }
        //enemy
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            EnemyAI enemy1 = rayHit.transform.GetComponent<EnemyAI>(); 
            if (enemy1 != null)
            {
                enemy1.TakeDamage(damage);
            }
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<EnemyAI>().TakeDamage(damage);           
        }
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }    
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {   
        ReloadingSound.Play();
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;       
    }
    private void DisableMuzzleFlashLight()
    {
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = false;
        }
    }
   
}
