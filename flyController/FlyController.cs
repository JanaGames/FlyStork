using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    Rigidbody rigidbody;
    public float speedIdle;
    float speed;
    public float speedForce;
    public float maxSpeed;
    public GameObject prefubBullet;
    public PhysicMaterial matBullet;
    public Transform ShootPos;
    Vector3 localVelocity;
    float hor, ver;
    Vector3 playerMovement;

    public float offsetX;

    public GameObject playerMesh;

    float timeLeft = 1f;
    bool isMoreSpeed;

    public bool isShoot;
    public GameParameters gameParameters;
    public float massBullet;

    AnimatorController animatorController;

    void Start()
    {
        massBullet = 1f;
        isShoot = true;
        speed = speedIdle;
        rigidbody = GetComponent<Rigidbody>();
        animatorController = GetComponent<AnimatorController>();
    }

    void Update()
    {
        SpeedController();
        RotateController();
        if (Input.GetButtonDown("Fire1") && !Menu.pausing && isShoot)
        {
            Shoot();
        }
        else if (!Input.GetButton("Vertical") && !Input.GetButton("Horizontal") && !Input.GetButton("Jump"))
        {
            Fly(Input.GetAxis("Horizontal"), speedIdle);
        }
    }
    void FixedUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            Stop();
        }
        else if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            Fly(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
    void Fly(float _hor, float _ver)
    {
        animatorController.Fly();
        hor = _hor;
        ver = _ver;

        playerMovement = new Vector3(hor, 0f, ver) * speed * Time.deltaTime;
        playerMovement = transform.TransformDirection(playerMovement);

        if (Input.GetAxis("Vertical") >= 0f) rigidbody.MovePosition(transform.position + playerMovement);
    }
    void Stop()
    {
        Fly(Input.GetAxis("Horizontal"), 0f);
    }
    void Shoot()
    {
        isShoot = false;
        //Vector3 BPos = new Vector3(ShootPos.position.x + 0.5f, ShootPos.position.y, ShootPos.position.z);
        GameObject bullet = Instantiate(prefubBullet, ShootPos.position, ShootPos.rotation) as GameObject;
        //bullet.transform.parent = this.transform;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<Collider>().material = matBullet;
        bullet.GetComponent<Bullet>().flyController = this;
        bullet.GetComponent<Bullet>().gameParameters = gameParameters;
        bullet.GetComponent<Bullet>().isTouch = true;
        bullet.GetComponent<Rigidbody>().mass = massBullet;
        rb.AddForce(ShootPos.forward * (5f + (speed * 5f)), ForceMode.Impulse);
        animatorController.Shoot();
    }
    void SpeedController()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isMoreSpeed = true;
            animatorController.Run();
        }
        if (Input.GetKey(KeyCode.W))
        {
            animatorController.Run();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isMoreSpeed = false;
        }
        if (isMoreSpeed && speed < maxSpeed) Timer(1f);
        if (!isMoreSpeed && speed > speedIdle) Timer(-1f);
        if (speed < speedIdle) speed = speedIdle;
        if (speed > maxSpeed) speed = maxSpeed;
    }
    void RotateController()
    {
        //in projectSettings: altNegative - a, altPositive - d is deleted;
        if (Input.GetKey(KeyCode.A))
        {
            //transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
            playerMesh.transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
            animatorController.Left();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerMesh.transform.Rotate(-Vector3.forward * 20 * Time.deltaTime);
            animatorController.Rigth();
        }
    }
    void Timer(float incr)
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            if (CheckSpeed(incr)) speed += incr;
            timeLeft = 1f;
        }
    }
    bool CheckSpeed(float incr)
    {
        //Debug.Log("check :" + ((speed += incr) <= maxSpeed && (speed += incr) >= speedIdle));
        if ((speed += incr) <= maxSpeed && (speed += incr) >= speedIdle) return true;
        else return false;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetDirShoot(Vector3 pos)
    {
        transform.position = pos;
    }
    public Vector3 GetForce() 
    {
        return ShootPos.forward * (5f + (speed * 5f))/massBullet;
    }
}
