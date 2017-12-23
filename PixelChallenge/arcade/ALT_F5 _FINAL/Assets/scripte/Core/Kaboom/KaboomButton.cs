using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KaboomButton
{
    public enum Button
    {
		BUTTON_NONE,
		BUTTON_OPERATOR_PRESS,
		BUTTON_UP_PRESS,
		BUTTON_LEFT_PRESS = BUTTON_UP_PRESS,
		BUTTON_DOWN_PRESS,
		BUTTON_RIGHT_PRESS = BUTTON_DOWN_PRESS,
		BUTTON_SELECT_PRESS,
        BUTTON_OPERATOR_RELEASE,
		BUTTON_UP_RELEASE,
		BUTTON_LEFT_RELEASE = BUTTON_UP_RELEASE,
		BUTTON_DOWN_RELEASE,
		BUTTON_RIGHT_RELEASE = BUTTON_DOWN_RELEASE,
		BUTTON_SELECT_RELEASE,
    }
}