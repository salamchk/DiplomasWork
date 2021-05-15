using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomasWork.Model
{
    public enum Command
    {
        MaxTurnRight = 'G',
        MaxTurnLeft = 'T',
        DirectPosition = 'D',
        MoveForward = 'F',
        MoveBack = 'B',
        TurnRight = 'R',
        TurnLeft = 'L',
        SetDirectPosition = 'M',
        StopMoving = 'S',
        TurnOnTheLight = 'I',
        AutomaticMode = 'A'
    }
}
