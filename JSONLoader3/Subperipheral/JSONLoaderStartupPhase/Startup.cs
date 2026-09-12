using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace JSONLoader3.Subperipheral.JSONLoaderStartupPhase;

/// <summary>
/// This class handles inserting our Startup Phase into the PlayerLoopSystem.
/// </summary>
public static class AddJSONLoaderStartupPhase
{
    /// <summary>
    /// A check determining if our Phase has ran already.
    /// </summary>
    private static bool hasPhaseExecuted = false;

    /// <summary>
    /// An action that ensures X Method is actually called by our Phase.
    /// </summary>
    public static Action OnStartupPhaseExecuted { get; set; }

    /// <summary>
    /// Installs our Phase into the PlayerLoopSystem.
    /// </summary>
    public static void Install()
    {
        try
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem startupPhase = new PlayerLoopSystem
            {
                type = typeof(AddJSONLoaderStartupPhase),
                updateDelegate = ExecuteStartupPhase
            };

            if (!InsertAfter(
                    ref playerLoop,
                    typeof(Initialization),
                    startupPhase))
            {
                JSONLoader3.FormatLogger(
                    "error",
                    "Startup Phase for JSONLoader3 and CSVLoader",
                    "Failed to install the startup phase into the Unity PlayerLoop.");

                return;
            }

            PlayerLoop.SetPlayerLoop(playerLoop);

            JSONLoader3.FormatLogger(
                "info",
                "Startup Phase for JSONLoader3 and CSVLoader",
                "Successfully installed startup phase into the Unity PlayerLoop.");
        }
        catch (Exception ex)
        {
            JSONLoader3.FormatLogger(
                "error",
                "Startup Phase for JSONLoader3 and CSVLoader",
                $"Exception while installing startup phase: {ex}");
        }
    }

    /// <summary>
    /// Executes our Loading Phase.
    /// </summary>
    private static void ExecuteStartupPhase()
    {
        if (hasPhaseExecuted)
        {
            return;
        }

        hasPhaseExecuted = true;

        JSONLoader3.FormatLogger(
            "info",
            "Startup Phase for JSONLoader3 and CSVLoader",
            "Executing JSONLoader3 startup phase.");

        if (OnStartupPhaseExecuted == null)
        {
            JSONLoader3.FormatLogger(
                "error",
                "Startup Phase for JSONLoader3 and CSVLoader",
                "Startup phase was triggered, but no callback was assigned.");

            return;
        }

        try
        {
            OnStartupPhaseExecuted.Invoke();

            JSONLoader3.FormatLogger(
                "info",
                "Startup Phase for JSONLoader3 and CSVLoader",
                "JSONLoader3 startup phase completed successfully.");
        }
        catch (Exception ex)
        {
            JSONLoader3.FormatLogger(
                "error",
                "Startup Phase for JSONLoader3 and CSVLoader",
                $"Exception during startup phase: {ex}");
        }
    }

    /// <summary>
    /// A bool determining whether we inserted after the System successfully
    /// </summary>
    /// <param name="root">The root PlayerLoopSystem.</param>
    /// <param name="targetType">The Type we're trying to Insert After.</param>
    /// <param name="newSystem">The Phase we want to Insert.</param>
    /// <returns>True if we inserted successfully, false otherwise.</returns>
    private static bool InsertAfter(
        ref PlayerLoopSystem root,
        Type targetType,
        PlayerLoopSystem newSystem)
    {
        if (root.subSystemList == null)
        {
            return false;
        }

        for (int i = 0; i < root.subSystemList.Length; i++)
        {
            if (root.subSystemList[i].type == targetType)
            {
                PlayerLoopSystem[] systems = root.subSystemList;

                PlayerLoopSystem[] newSystems =
                    new PlayerLoopSystem[systems.Length + 1];

                for (int j = 0; j <= i; j++)
                {
                    newSystems[j] = systems[j];
                }

                newSystems[i + 1] = newSystem;

                for (int j = i + 1; j < systems.Length; j++)
                {
                    newSystems[j + 1] = systems[j];
                }

                root.subSystemList = newSystems;

                return true;
            }

            PlayerLoopSystem child = root.subSystemList[i];

            if (InsertAfter(ref child, targetType, newSystem))
            {
                root.subSystemList[i] = child;
                return true;
            }
        }

        return false;
    }
}