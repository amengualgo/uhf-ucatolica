package com.example.demo_uhf_usb;

import android.Manifest;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.fragment.app.Fragment;

import com.example.demo_uhf_usb.fragment.BarcodeFragment;
import com.example.demo_uhf_usb.fragment.UHFEraseFragment;
import com.example.demo_uhf_usb.fragment.UHFInventoryFragment;
import com.example.demo_uhf_usb.fragment.UHFKillFragment;
import com.example.demo_uhf_usb.fragment.UHFLockFragment;
import com.example.demo_uhf_usb.fragment.UHFReadFragment;
import com.example.demo_uhf_usb.fragment.UHFSetFragment;
import com.example.demo_uhf_usb.fragment.UHFUpdataFragment;
import com.example.demo_uhf_usb.fragment.UHFWriteFragment;
import com.example.demo_uhf_usb.interfac.OnUSBStatusChangedListener;
import com.example.demo_uhf_usb.utils.SPUtil;
import com.example.demo_uhf_usb.utils.Sound;
import com.rscja.deviceapi.RFIDWithUHFUSB;

import java.util.List;

public class MainActivity extends BaseActivity {

    private String TAG = "MainActivity";

    private boolean isVoiceCuesOpen; // 是否开启声音提示

    public RFIDWithUHFUSB mRFIDWithUHFUSB;
    private TextView tvDeviceInfo, tvMessage;
    private Button btnOpen;
    private boolean isOpen; // 是否已打开连接

    private UsbDevice currDevice;

    private Sound mSound;
    private static final int WRITE_EXTERNAL_STORAGE_PERMISSION_REQUEST = 101;
    private static final int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST=102;
    @Override
    protected void onCreating(Bundle savedInstanceState) {
        setContentView(R.layout.activity_main);
        init();
        initDevice();
        registerReceiver();
        checkReadWritePermission();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        super.onOptionsItemSelected(item);
        if (item.getItemId() == R.id.UHF_Battery) {
            String ver = getString(R.string.action_uhf_bat) + ":" + mRFIDWithUHFUSB.getBattery() + "%";
            Utils.alert(MainActivity.this, R.string.action_uhf_bat, ver, R.drawable.webtext);
        } else if (item.getItemId() == R.id.UHF_T) {
            String temp = getString(R.string.title_about_Temperature) + ":" + mRFIDWithUHFUSB.getTemperature() + "℃";
            Utils.alert(MainActivity.this, R.string.title_about_Temperature, temp, R.drawable.webtext);
        } else if (item.getItemId() == R.id.UHF_ver) {
            String ver = mRFIDWithUHFUSB.getVersion();
            Utils.alert(MainActivity.this, R.string.action_uhf_ver, ver, R.drawable.webtext);
        }else if (item.getItemId() == R.id.STM32_ver) {
            String ver = mRFIDWithUHFUSB.getSTM32Version();
            Utils.alert(MainActivity.this, R.string.action_stm32_ver, ver, R.drawable.webtext);
        }

        return true;
    }
    private void init() {
        if(BuildConfig.DEBUG) {
            setTitle(String.format("%s(v%s-debug)", getString(R.string.app_name), BuildConfig.VERSION_NAME));
        } else {
            setTitle(String.format("%s(v%s)", getString(R.string.app_name), BuildConfig.VERSION_NAME));
        }

        mSound = new Sound(this);
        isVoiceCuesOpen = SPUtil.getShared(Constant.VOICE_CUES, true);

        tvDeviceInfo = findViewById(R.id.tvDeviceInfo);
        tvMessage = findViewById(R.id.tvMessage);
        btnOpen = findViewById(R.id.btnOpen);
        btnOpen.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (currDevice == null) {
                    showToast(R.string.unplugged_device);
                    return;
                }
                if (btnOpen.getText().equals(getString(R.string.open))) {
                    setBtnStatus(true);
                    openDevice(currDevice);
                } else {
                    setBtnStatus(false);
                    free();
                }
            }
        });
    }


    @Override
    protected void initTabTitles(List<Fragment> fragments, List<String> titles) {

        fragments.add(new UHFInventoryFragment());
        fragments.add(new UHFSetFragment());
        fragments.add(new UHFReadFragment());
        fragments.add(new UHFWriteFragment());
        fragments.add(new BarcodeFragment());
        fragments.add(new UHFEraseFragment());
        fragments.add(new UHFLockFragment());
        fragments.add(new UHFKillFragment());
        fragments.add(new UHFUpdataFragment());


        titles.add(getString(R.string.inventory));
        titles.add(getString(R.string.config));
        titles.add(getString(R.string.read_labels));
        titles.add(getString(R.string.write_labels));
        titles.add(getString(R.string.title_2Dscan));
        titles.add(getString(R.string.uhf_msg_tab_erase));
        titles.add(getString(R.string.lock));
        titles.add(getString(R.string.kill));
        titles.add(getString(R.string.title_update));

        super.initTabTitles(fragments, titles);
    }

    private void setBtnStatus(boolean opened) {
        if(opened) {
            btnOpen.setText(R.string.close);
        } else {
            btnOpen.setText(R.string.open);
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mSound.freeSound();
        free();
        unRegisterReceiver();
    }

    /**
     * 是否已打开USB连接
     * @return
     */
    public boolean isOpenConnected() {
        return isOpen;
    }

    /**
     * 注册广播
     */
    public void registerReceiver() {
        IntentFilter filter = new IntentFilter(RFIDWithUHFUSB.ACTION_USB_PERMISSION);
        filter.addAction(UsbManager.ACTION_USB_DEVICE_DETACHED);
        filter.addAction(UsbManager.ACTION_USB_DEVICE_ATTACHED);
        registerReceiver(usbReceiver, filter);
    }

    public void unRegisterReceiver() {
        if (usbReceiver != null) {
            unregisterReceiver(usbReceiver);
            usbReceiver = null;
        }
    }

    private void showCurrDevice(UsbDevice device) {
        currDevice = device;
        if (device == null) {
            setBtnStatus(false);
            return;
        }
        StringBuffer stringBuffer = new StringBuffer();
        stringBuffer.append("Name:");
        stringBuffer.append(device.getDeviceName());
        stringBuffer.append("\nV_ID:");
        stringBuffer.append(device.getVendorId());
        stringBuffer.append("\t\tP_ID:");
        stringBuffer.append(device.getProductId());
        tvDeviceInfo.setText(stringBuffer.toString());
    }

    private void showMsg(String msg) {
        tvMessage.setText(msg);
    }

    private void updateStatus(boolean openSuccess) {
        if (openSuccess) {
            showMsg(getString(R.string.msg_open_success));
            isOpen = true;
            updateUSBStatusListen(currDevice, OnUSBStatusChangedListener.CONNECTED);
        } else {
            showMsg(getString(R.string.msg_open_fail));
            isOpen = false;
            updateUSBStatusListen(currDevice, OnUSBStatusChangedListener.DISCONNECTED);
        }
    }

    private void initDevice() {
        mRFIDWithUHFUSB = RFIDWithUHFUSB.getInstance();
        List<UsbDevice> usbDeviceList = mRFIDWithUHFUSB.getUsbDeviceList(getApplicationContext());
        if (usbDeviceList != null && !usbDeviceList.isEmpty()) {
            showCurrDevice(usbDeviceList.get(0));
        }
    }

    private void openDevice(UsbDevice usbDevice) {
        if (mRFIDWithUHFUSB != null && usbDevice != null) {
            boolean res = mRFIDWithUHFUSB.init(usbDevice, getApplicationContext());
            updateStatus(res);
            setBtnStatus(res);
        } else {
            setBtnStatus(false);
        }
    }

    private void free() {
        if (mRFIDWithUHFUSB != null) {
            updateUSBStatusListen(currDevice, OnUSBStatusChangedListener.BEFORE_DISCONNECT);
            mRFIDWithUHFUSB.free();
            updateUSBStatusListen(currDevice, OnUSBStatusChangedListener.DISCONNECTED);
            showMsg(getString(R.string.msg_close_ed));
        }
        isOpen = false;
    }

    private BroadcastReceiver usbReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            UsbDevice device = intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);
            Log.e("USBReceiver", "action：" + action);
            if (RFIDWithUHFUSB.ACTION_USB_PERMISSION.equals(action)) {
                // 获取权限结果的广播
                synchronized (this) {
                    showCurrDevice(device);
                    if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false)) {
//                        showMsg(getString(R.string.msg_permission_success) + device.getDeviceName());
                        updateUSBStatusListen(device, OnUSBStatusChangedListener.PERMISSION_GRANTED);
                        openDevice(device);
                    } else {
                        showMsg(getString(R.string.msg_permission_fail) + device.getDeviceName());
                        updateUSBStatusListen(device, OnUSBStatusChangedListener.PERMISSION_NOT_GRANTED);
                    }
                }
            } else if (UsbManager.ACTION_USB_DEVICE_ATTACHED.equals(action)) {
                // 有新的设备插入了，在这里一般会判断这个设备是不是我们想要的，是的话就去请求权限
                showCurrDevice(device);
                showMsg(getString(R.string.msg_found_device));
                updateUSBStatusListen(device, OnUSBStatusChangedListener.ATTACHED);
                playSound(1);
            } else if (UsbManager.ACTION_USB_DEVICE_DETACHED.equals(action)) {
                // 有设备拔出了
                showCurrDevice(null);
                showMsg(getString(R.string.msg_device_detached));
                isOpen = false;
                updateUSBStatusListen(device, OnUSBStatusChangedListener.DETACHED);
                playSound(2);
            }
        }
    };

    public void setVoiceCuesOpen(boolean open) {
        isVoiceCuesOpen = open;
    }

    public void playSound(int id) {
        // 是否开启声音提示
        if(isVoiceCuesOpen)
            mSound.playSound(id);
    }

    private void updateUSBStatusListen(UsbDevice usbDevice, int status) {
        if(onUSBStatusChangedListener != null) {
            onUSBStatusChangedListener.onStatusChanged(usbDevice, status);
        }
    }

    private OnUSBStatusChangedListener onUSBStatusChangedListener;
    public void setOnUSBStatusChangedListener(OnUSBStatusChangedListener onUSBStatusChangedListener) {
        this.onUSBStatusChangedListener = onUSBStatusChangedListener;
    }

    private void checkReadWritePermission() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            if (checkSelfPermission(Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                requestPermissions(new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE}, WRITE_EXTERNAL_STORAGE_PERMISSION_REQUEST);
            }
            if (checkSelfPermission(Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                requestPermissions(new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, READ_EXTERNAL_STORAGE_PERMISSION_REQUEST);
            }
        }
    }


}
