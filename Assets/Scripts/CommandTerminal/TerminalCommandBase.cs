using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerminalCommandBase : ScriptableObject
{
    public abstract void RegisterCommands();

    public abstract void UnregisterCommands();
}
