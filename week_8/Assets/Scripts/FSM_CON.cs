using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FSM_CON : MonoBehaviour
{
    private GameObject[] gos;
// Stores a reference to the player.
private GameObject Player = null;
private float MoveSpeed = 3;
// Projectile hit event variables.
private bool HitByProjectileEvent = false;
// Enum for game states.
private enum GameStates { IDLE, CHASEPLAYER, RETREAT };
private GameStates State = GameStates.IDLE;
// Enum for state events.
private enum GameEvents { ON_ENTER, ON_UPDATE };
private GameEvents Event = GameEvents.ON_ENTER;
// Start is called before the first frame update
void Start()
    {
        gos = GameObject.FindGameObjectsWithTag("Player");
if (gos.Length > 0)
{
Player = gos[0];
}
if (Player == null)
{
Debug.Log("No Player found in the scene!!!");
    }
    }
// Update is called once per frame
void Update()
{
// Update the FSM behaviour.
FSMUpdate();
}
// FSM update method.
private void FSMUpdate()
{
// --- Idle. ---
if (State == GameStates.IDLE)
{
if (Event == GameEvents.ON_ENTER)
{
idle_Enter();
}
if (Event == GameEvents.ON_UPDATE)
{
idle_Update();
}
}
// --- Move. ---
if (State == GameStates.CHASEPLAYER)
{
if (Event == GameEvents.ON_ENTER)
{
chasePlayer_Enter();
}
if (Event == GameEvents.ON_UPDATE)
{
chasePlayer_Update();
}
}if (State == GameStates.RETREAT)
{
if (Event == GameEvents.ON_ENTER)
{
retreat_Enter();
}
if (Event == GameEvents.ON_UPDATE)
{
retreat_Update();
}
}
// Process input / general events. (If needed).
FSMProcessInput();
//:~END: --- Process inputs ---
}
void OnCollisionEnter(Collision col)
{
    Debug.Log("OnCollisionEnter - " + col.gameObject.name);
if (col.gameObject.tag.Contains("Projectile"))
{
Destroy(col.gameObject);
HitByProjectileEvent = true;
}
}
// Process input / general events.
private void FSMProcessInput()
{
// Process input here / general events if needed. For example, user input.
// Could cause a state change. Must call state change method.
// I would only change states in state code.
}
// Change state method. The only places a state should change. Call this method when
private void ChangeFSMState(GameStates newState)
{
// Finish / exit current state.
switch (State)
{
case GameStates.IDLE:
idle_Exit();
break;
case GameStates.CHASEPLAYER:
chasePlayer_Exit();
break;
case GameStates.RETREAT:
retreat_Exit();
break;
}
// Move to the next state.
State = newState;
Event = GameEvents.ON_ENTER;
}private void idle_Enter()
{
// Change to update at the end of enter.
Event = GameEvents.ON_UPDATE;
}
// Idle - update.
private void idle_Update()
{
transform.Rotate(0, 1, 0, Space.World);
// FSM transition rule.
if(Vector3.Distance(transform.position, Player.transform.position) < 10)
{
// Change the state.
ChangeFSMState(GameStates.CHASEPLAYER);
}
// FSM transition rule.
if (HitByProjectileEvent == true)
{
HitByProjectileEvent = false;
// Change the state.
ChangeFSMState(GameStates.RETREAT);
}
}
// Idle - Exit.
private void idle_Exit()
{
}
// ******************************* -Chase Player- *******************************
// Chase Player - Enter.
private void chasePlayer_Enter()
{
// Change to update at the end of enter.
Event = GameEvents.ON_UPDATE;
}
// Chase Player - Update.
private void chasePlayer_Update()
{
transform.position = Vector3.MoveTowards(transform.position,
Player.transform.position, (MoveSpeed * Time.deltaTime));
// FSM transition rule.
if (HitByProjectileEvent == true)
{
HitByProjectileEvent = false;
// Change the state.
ChangeFSMState(GameStates.RETREAT);
}
}
// Chase Player - Exit.
private void chasePlayer_Exit()
{
}private void retreat_Enter()
{
// Change to update at the end of enter.
Event = GameEvents.ON_UPDATE;
}
// Retreat - Update.
private void retreat_Update()
{
// Implement game state here.
// Could cause a state change. Must call state change method.
Vector3 Direction = Player.transform.position - transform.position;
// Clear y to avoid going up and down.
Direction.y = 0;
Vector3 Position = transform.position - Direction;
transform.position = Vector3.MoveTowards(transform.position, Position, (MoveSpeed
* Time.deltaTime));
// FSM transition rule.
if (Vector3.Distance(transform.position, Player.transform.position) > 20)
{
// Change the state.
ChangeFSMState(GameStates.IDLE);
}
}
// Retreat - Exit.
private void retreat_Exit()
{
}
}