using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SciFiWarriorCON : MonoBehaviour
{
// A reference to the player GameObject.
[SerializeField]
private GameObject player;
// The speed of the character.
[SerializeField]
private float moveSpeed = 1.0f;
// A reference to the animator.
private Animator anim;
// Store the current AnimatorStateInfo.
AnimatorStateInfo currentStateInfo;
// Hashes to animator parameters.
private int speedHash = Animator.StringToHash("Speed");
private int dyingHash = Animator.StringToHash("IsDead");
private float WaitTime = 3;
private float WaitTimer = 0;
// Start is called before the first frame update
void Start()
{
anim = GetComponent<Animator>();
}
void Update()
{
currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
if (currentStateInfo.IsName("Dead"))
{
// The GameObject is in the dead state.
// Start the timer.
WaitTimer += Time.deltaTime;
if (WaitTimer > WaitTime)
{
anim.SetBool("IsDead", false);
WaitTimer = 0;
}
}
if (Vector3.Distance(transform.position, player.transform.position) < 7)
{
anim.SetFloat(speedHash, 0.7f);
Vector3 tmp = player.transform.position;
tmp.y = this.transform.position.y;
transform.LookAt(tmp, Vector3.up);
currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
if (currentStateInfo.IsName("Walk"))
{
transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
}
}
else
{
anim.SetFloat(speedHash, 0);
}
}
void OnTriggerEnter(Collider other)
{
Debug.Log("OnTriggerEnter - " + other.gameObject.name);
if (other.gameObject.tag.Contains("Projectile"))
{
anim.SetBool(dyingHash, true);
Destroy(other.gameObject);
}
}
}