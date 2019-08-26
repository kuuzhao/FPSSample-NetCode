using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;


[DisableAutoCreation]
public class PlayerCommandSendSystem : CommandSendSystem<PlayerCommandData>
{
}

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[UpdateAfter(typeof(GhostReceiveSystemGroup))]
[UpdateBefore(typeof(PlayerCommandSendSystem))]
public class NetCodeInputSystem : ComponentSystem
{
    EntityQuery cmdTargetGroup;

    // TODO: these should be put in some global setting
    public static Vector2 s_JoystickLookSensitivity = new Vector2(90.0f, 60.0f);

    static float maxMoveYaw;
    static float maxMoveMagnitude;

    byte grenadeDebugNo = 1;
    PlayerCommandData playerCommandData;

    protected override void OnCreateManager()
    {
        cmdTargetGroup = GetEntityQuery(ComponentType.ReadWrite<CommandTargetComponent>());
        playerCommandData = default(PlayerCommandData);
        playerCommandData.lookPitch = 90.0f;
    }

    protected override void OnUpdate()
    {
        if (cmdTargetGroup.IsEmptyIgnoreFilter)
            return;

        var entityArray = cmdTargetGroup.GetEntityArraySt();

        if (entityArray.Length == 1)
        {
            Entity ent = entityArray[0];
            if (!EntityManager.HasComponent<PlayerCommandData>(ent))
            {
                EntityManager.AddBuffer<PlayerCommandData>(ent);
                var ctc = EntityManager.GetComponentData<CommandTargetComponent>(ent);
                ctc.targetEntity = ent;
                EntityManager.SetComponentData(ent, ctc);
            }

            if (!EntityManager.HasComponent<NetworkStreamDisconnected>(ent))
            {
                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);

                AccumulateInput(ref playerCommandData, Time.deltaTime);

                if (Input.GetMouseButtonUp(1))
                {
                    playerCommandData.grenade = grenadeDebugNo;
                    Debug.Log("LZ: Send Grenade #" + playerCommandData.grenade + " at frame #" + Time.frameCount);
                    grenadeDebugNo++;
                }
                else
                    playerCommandData.grenade = 0;

                playerCommandData.tick = NetworkTimeSystem.predictTargetTick;

                cmdBuf.AddCommandData(playerCommandData);
            }
        }
    }

    // Copied from FPSSample/Assets/Scripts/Game/System/InputSystem.cs
    public void AccumulateInput(ref PlayerCommandData command, float deltaTime)
    {
        // Reset the values which are not supposed to be accumulated.
        maxMoveMagnitude = 0;
        command.buttons.flags = 0;

        // To accumulate move we store the input with max magnitude and uses that
        Vector2 moveInput = new Vector2(Game.Input.GetAxisRaw("Horizontal"), Game.Input.GetAxisRaw("Vertical"));
        float angle = Vector2.Angle(Vector2.up, moveInput);
        if (moveInput.x < 0)
            angle = 360 - angle;
        float magnitude = Mathf.Clamp(moveInput.magnitude, 0, 1);
        if (magnitude > maxMoveMagnitude)
        {
            maxMoveYaw = angle;
            maxMoveMagnitude = magnitude;
        }
        command.moveYaw = maxMoveYaw;
        command.moveMagnitude = maxMoveMagnitude;

        float invertY = Game.configInvertY.IntValue > 0 ? -1.0f : 1.0f;

        Vector2 deltaMousePos = new Vector2(0, 0);
        if (deltaTime > 0.0f)
            deltaMousePos += new Vector2(Game.Input.GetAxisRaw("Mouse X"), Game.Input.GetAxisRaw("Mouse Y") * invertY);
        deltaMousePos += deltaTime * (new Vector2(Game.Input.GetAxisRaw("RightStickX") * s_JoystickLookSensitivity.x, -invertY * Game.Input.GetAxisRaw("RightStickY") * s_JoystickLookSensitivity.y));
        deltaMousePos += deltaTime * (new Vector2(
            ((Game.Input.GetKey(KeyCode.Keypad4) ? -1.0f : 0.0f) + (Game.Input.GetKey(KeyCode.Keypad6) ? 1.0f : 0.0f)) * s_JoystickLookSensitivity.x,
            -invertY * Game.Input.GetAxisRaw("RightStickY") * s_JoystickLookSensitivity.y));

        command.lookYaw += deltaMousePos.x * Game.configMouseSensitivity.FloatValue;
        command.lookYaw = command.lookYaw % 360;
        while (command.lookYaw < 0.0f) command.lookYaw += 360.0f;

        command.lookPitch += deltaMousePos.y * Game.configMouseSensitivity.FloatValue;
        command.lookPitch = Mathf.Clamp(command.lookPitch, 0, 180);

        command.buttons.Or(PlayerCommandData.Button.Jump, Game.Input.GetKeyDown(KeyCode.Space) || Game.Input.GetKeyDown(KeyCode.Joystick1Button0));
        command.buttons.Or(PlayerCommandData.Button.Boost, Game.Input.GetKey(KeyCode.LeftControl) || Game.Input.GetKey(KeyCode.Joystick1Button4));
        command.buttons.Or(PlayerCommandData.Button.PrimaryFire, (Game.Input.GetMouseButton(0) && Game.GetMousePointerLock()) || (Game.Input.GetAxisRaw("Trigger") < -0.5f));
        // TODO: LZ:
        //      turn off secondary fire
        //command.buttons.Or(UserCommand.Button.SecondaryFire, Game.Input.GetMouseButton(1) || Game.Input.GetKey(KeyCode.Joystick1Button5));
        command.buttons.Or(PlayerCommandData.Button.Ability1, Game.Input.GetKey(KeyCode.LeftShift));
        command.buttons.Or(PlayerCommandData.Button.Ability2, Game.Input.GetKey(KeyCode.E));
        command.buttons.Or(PlayerCommandData.Button.Ability3, Game.Input.GetKey(KeyCode.Q));
        command.buttons.Or(PlayerCommandData.Button.Reload, Game.Input.GetKey(KeyCode.R) || Game.Input.GetKey(KeyCode.Joystick1Button2));
        command.buttons.Or(PlayerCommandData.Button.Melee, Game.Input.GetKey(KeyCode.V) || Game.Input.GetKey(KeyCode.Joystick1Button1));
        command.buttons.Or(PlayerCommandData.Button.Use, Game.Input.GetKey(KeyCode.E));

        // Debug.LogError(command.ToString());
    }
}
