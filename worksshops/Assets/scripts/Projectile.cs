using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
public Rigidbody projectileRigidBody;
public float projectilePower = 250;
// Start is called before the first frame update
void Start()
{
}
// Update is called once per frame
void Update()
{
if (Input.GetButtonUp("Fire1"))
{
// Instantiate the projectile.
Rigidbody aInstance = Instantiate(projectileRigidBody, transform.position,
transform.rotation) as Rigidbody;
// Add force.
Vector3 forward = transform.TransformDirection(Vector3.forward);
aInstance.AddForce(forward * projectilePower);
// Destroy the object after X seconds.
Destroy(aInstance.gameObject, 8);
}
}
}