#include "pch.h"
#include <Xinput.h>
#include "GamePad.h"

namespace DJIVideoParser
{
	GamePad::GamePad(void)
	{
		m_isConnected = false;
	}

	void GamePad::Refresh(){
				
		DWORD result = XInputGetState(0, &m_state);

		if(result == ERROR_DEVICE_NOT_CONNECTED)
			m_isConnected = false;
		else if(result == ERROR_SUCCESS)
			m_isConnected = true;		
	}

	bool GamePad::GetLeftTrigger() {
		return m_state.Gamepad.bLeftTrigger != 0;
	}

	bool GamePad::GetRightTrigger() {
		return m_state.Gamepad.bRightTrigger != 0;
	}

	bool GamePad::GetButtonState(int idx){

		DWORD mask = 0x00;
		switch(idx){
			case 0: mask = 0x0001; break; /* DPAD UP */
			case 1: mask = 0x0002; break; /* DPAD DOWN */
			case 2: mask = 0x0004; break; /* DPAD LEFT */
			case 3: mask = 0x0008; break; /* DPAD RIGHT */
			case 4: mask = 0x0010; break; /* START */
			case 5: mask = 0x0020; break; /* BACK */
			case 6: mask = 0x0040; break; /* LEFT THUMB */
			case 7: mask =  0x0080; break; /* RIGHT THUMB */
			case 8: mask =  0x0100; break; /* LEFT SHOULDER */
			case 9: mask =  0x0200; break; /* RIGHT SHOULDER */
			case 10: mask = 0x1000; break; /* A */
			case 11: mask = 0x2000; break; /* B */
			case 12: mask = 0x4000; break; /* X */
			case 13: mask = 0x8000; break; /* Y */
		}

		DWORD result = m_state.Gamepad.wButtons & mask;

		return result != 0x00;
	}

	short GamePad::GetLeftX() {
		return m_state.Gamepad.sThumbLX;
	}

	short GamePad::GetLeftY() {
		return m_state.Gamepad.sThumbLY;
	}

	short GamePad::GetRightY() {
		return m_state.Gamepad.sThumbRY;
	}

	short GamePad::GetRightX() {
		return m_state.Gamepad.sThumbRX;
	}

	bool GamePad::GetIsConnected(){
		return m_isConnected;
	}
}