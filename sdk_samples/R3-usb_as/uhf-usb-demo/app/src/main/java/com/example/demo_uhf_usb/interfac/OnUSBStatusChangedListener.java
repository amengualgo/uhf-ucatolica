package com.example.demo_uhf_usb.interfac;

import android.hardware.usb.UsbDevice;

/**
 * Created by WuShengjun on 2019/6/25.
 * Description:
 */

public interface OnUSBStatusChangedListener {
    int ATTACHED = 0;
    int CONNECTED = 1;
    int PERMISSION_GRANTED = 2;
    int BEFORE_DISCONNECT = -1;
    int DISCONNECTED = -2;
    int DETACHED = -3;
    int PERMISSION_NOT_GRANTED = -4;

    void onStatusChanged(UsbDevice usbDevice, int status);
}
