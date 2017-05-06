using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler
{
    private Dictionary<string, Command> commands;
    private PlayerController playerController;

    public InputHandler(PlayerController thisController)
    {
        playerController = thisController;

        commands = new Dictionary<string, Command>();
        commands.Add("Forward", new MoveForwardCommand());
        commands.Add("Strafe", new StrafeCommand());
        commands.Add("Turn", new TurnCommand());
        commands.Add("Attack", new AttackCommand());
        commands.Add("UsePotion", new UsePotionCommand());
        commands.Add("ToggleRun", new ToggleRunCommand());
    }

    public void HandleInput()
    {
        // all input for the ui is handled by the ui system itself
        if (playerController.InMenu) return; 

        if (Mathf.Abs(Input.GetAxis("Forward")) > 0.01f) commands.Get("Forward").Execute(playerController, Input.GetAxis("Forward"));
        if (Mathf.Abs(Input.GetAxis("Strafe")) > 0.01f) commands.Get("Strafe").Execute(playerController, Input.GetAxis("Strafe"));
        if (Mathf.Abs(Input.GetAxis("Turn")) > 0.01f) commands.Get("Turn").Execute(playerController, Input.GetAxis("Turn"));
        if (Input.GetMouseButtonDown(0)) commands.Get("Attack").Execute(playerController);
        if (Input.GetKeyDown(KeyCode.Alpha1)) commands.Get("UsePotion").Execute(playerController);
        if (Input.GetKeyDown(KeyCode.LeftShift)) commands.Get("ToggleRun").Execute(playerController);

        return;
    }
}
