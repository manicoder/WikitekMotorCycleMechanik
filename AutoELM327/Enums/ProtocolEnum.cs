using System;
using System.Collections.Generic;
using System.Text;

namespace AutoELM327.Enums
{
    public enum ProtocolEnum
    { 
        Automatic = 0x00,
        SAE_J1850_PWM = 0x01,
        SAE_J1850_VPW = 0x02,
        ISO_9141_2 = 0x03,
        ISO14230_4KWP_5BAUD_INIT = 0x04,
        ISO14230_4KWP_FASTINIT = 0x05,
        ISO15765_500KB_11BIT_CAN = 0x06,
        ISO15765_500KB_29BIT_CAN = 0x07,
        ISO15765_250KB_11BIT_CAN = 0x08,
        ISO15765_250Kb_29BIT_CAN = 0x09,
        SAEJ1939CAN_29BIT_ID_250KBAUD = 0x0A,
    }
    public enum Connectivity
    {
        None,
        Bluetooth,
        WiFi,
        USB
    }
    public enum Platform
    {
        None,
        Android,
        UWP,
        iOS
    }
}
