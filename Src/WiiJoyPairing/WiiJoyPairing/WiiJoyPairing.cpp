#include "stdafx.h"
#include <windows.h>
#include <bthsdpdef.h>
#include <bthdef.h>
#include <BluetoothAPIs.h>
#include <strsafe.h>
#include <iostream>
#pragma comment(lib, "Bthprops.lib")
BLUETOOTH_DEVICE_INFO btdir;
BLUETOOTH_DEVICE_INFO btdil;
BLUETOOTH_DEVICE_INFO btdiw;
bool joyconrfound = false;
bool joyconlfound = false;
bool wiimotefound = false;
#pragma warning(disable : 4995)
extern "C"
{
	__declspec(dllexport) int connect()
	{
		HBLUETOOTH_DEVICE_FIND hFind = NULL;
		HANDLE hRadios[256];
		HBLUETOOTH_RADIO_FIND hFindRadio;
		BLUETOOTH_FIND_RADIO_PARAMS radioParam;
		BLUETOOTH_RADIO_INFO radioInfo;
		BLUETOOTH_DEVICE_SEARCH_PARAMS srch;
		BLUETOOTH_DEVICE_INFO btdi;
		int nRadios = 0;
		radioParam.dwSize = sizeof(BLUETOOTH_FIND_RADIO_PARAMS);
		radioInfo.dwSize = sizeof(BLUETOOTH_RADIO_INFO);
		btdir.dwSize = sizeof(btdir);
		btdil.dwSize = sizeof(btdil);
		btdiw.dwSize = sizeof(btdiw);
		btdi.dwSize = sizeof(btdi);
		srch.dwSize = sizeof(BLUETOOTH_DEVICE_SEARCH_PARAMS);
		hFindRadio = BluetoothFindFirstRadio(&radioParam, &hRadios[nRadios++]);
		while (BluetoothFindNextRadio(hFindRadio, &hRadios[nRadios++]))
		{
			hFindRadio = BluetoothFindFirstRadio(&radioParam, &hRadios[nRadios++]);
			BluetoothFindRadioClose(hFindRadio);
		}
		srch.fReturnAuthenticated = TRUE;
		srch.fReturnRemembered = TRUE;
		srch.fReturnConnected = TRUE;
		srch.fReturnUnknown = TRUE;
		srch.fIssueInquiry = TRUE;
		srch.cTimeoutMultiplier = 2;
		srch.hRadio = hRadios[1];
		BluetoothGetRadioInfo(hRadios[1], &radioInfo);
		WCHAR pass[6];
		DWORD pcServices = 16;
		GUID guids[16];
		pass[0] = radioInfo.address.rgBytes[0];
		pass[1] = radioInfo.address.rgBytes[1];
		pass[2] = radioInfo.address.rgBytes[2];
		pass[3] = radioInfo.address.rgBytes[3];
		pass[4] = radioInfo.address.rgBytes[4];
		pass[5] = radioInfo.address.rgBytes[5];
		for (int i = 0; i < 3; i++)
		{
			hFind = BluetoothFindFirstDevice(&srch, &btdi);
			if (hFind > 0)
			{
				do
				{
					if (!wcscmp(btdi.szName, L"Nintendo RVL-WBC-01") | !wcscmp(btdi.szName, L"Nintendo RVL-CNT-01"))
					{
						BluetoothAuthenticateDevice(NULL, hRadios[1], &btdi, pass, 6);
						BluetoothEnumerateInstalledServices(hRadios[1], &btdi, &pcServices, guids);
						BluetoothSetServiceState(hRadios[1], &btdi, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
						BluetoothUpdateDeviceRecord(&btdi);
						btdiw = btdi;
						wiimotefound = true;
					}
					if (!joyconlfound)
						if (!wcscmp(btdi.szName, L"Joy-Con (L)"))
						{
							BluetoothAuthenticateDevice(NULL, hRadios[1], &btdi, pass, 6);
							BluetoothEnumerateInstalledServices(hRadios[1], &btdi, &pcServices, guids);
							BluetoothSetServiceState(hRadios[1], &btdi, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
							BluetoothUpdateDeviceRecord(&btdi);
							btdil = btdi;
							joyconlfound = true;
						}
					if (!joyconrfound)
						if (!wcscmp(btdi.szName, L"Joy-Con (R)"))
						{
							BluetoothAuthenticateDevice(NULL, hRadios[1], &btdi, pass, 6);
							BluetoothEnumerateInstalledServices(hRadios[1], &btdi, &pcServices, guids);
							BluetoothSetServiceState(hRadios[1], &btdi, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
							BluetoothUpdateDeviceRecord(&btdi);
							btdir = btdi;
							joyconrfound = true;
						}
				} while (BluetoothFindNextDevice(hFind, &btdi));
				BluetoothFindDeviceClose(hFind);
			}
		}
		BluetoothFindRadioClose(hFindRadio);
		if (!wiimotefound && !joyconrfound && joyconlfound)
			return 1;
		if (!joyconrfound && wiimotefound && joyconlfound)
			return 2;
		if (!joyconrfound && !joyconlfound && wiimotefound)
			return 3;
		if (!wiimotefound && joyconrfound && joyconlfound)
			return 4;
		if (!joyconlfound && wiimotefound && joyconrfound)
			return 5;
		if (!wiimotefound && !joyconlfound && joyconrfound)
			return 6;
		if (wiimotefound && joyconlfound && joyconrfound)
			return 7;
		return 0;
	}
	__declspec(dllexport) bool disconnect()
	{
		if (wiimotefound)
		{ 
			BluetoothRemoveDevice(&btdiw.Address);
			wiimotefound = false;
		}
		if (joyconlfound)
		{
			BluetoothRemoveDevice(&btdil.Address);
			joyconlfound = false;
		}
		if (joyconrfound)
		{
			BluetoothRemoveDevice(&btdir.Address);
			joyconrfound = false;
		}
		return true;
	}
}