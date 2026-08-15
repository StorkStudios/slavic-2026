using CommandTerminal;
using UnityEngine;

public static class HotinCommand
{
    [RegisterCommand(Help = "Set HotIn value", MaxArgCount = 1, MinArgCount = 1)]
    static void HotIn(CommandArg[] args)
    {
        Hotin.Instance.Value = args[0].Float;
    }
}
