#pragma once

#include <Xinput.h>

namespace DJIVideoParser
{
	public ref class GamePad sealed
	{
	private: 
		XINPUT_STATE m_state;
		bool m_isConnected;

	public:
		GamePad(void);

		void Refresh();

		bool GetLeftTrigger();
		bool GetRightTrigger();

		short GetLeftX();
		short GetLeftY();

		short GetRightX();
		short GetRightY();

		bool GetButtonState(int idx);

		bool GetIsConnected();
	};
}

