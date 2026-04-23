package com.example.demo_uhf_usb.fragment;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.Spinner;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.example.demo_uhf_usb.BaseFragment;
import com.example.demo_uhf_usb.Constant;
import com.example.demo_uhf_usb.MainActivity;
import com.example.demo_uhf_usb.R;
import com.example.demo_uhf_usb.utils.SPUtil;

/**
 * Created by WuShengjun on 2019/6/24.
 * Description:
 */

public class UHFSetFragment extends BaseFragment implements View.OnClickListener {

    private String TAG = "CW_UHFSetFragment";
    private MainActivity mActivity;
    private Spinner spMode;
    private Button BtSetFre, BtGetFre;

    private Spinner spPower;
    private Button btnSetPower, btnGetPower;

    private Button btnBeepOpen, btnBeepClose;
    private CheckBox cbVoiceCues;

    private String[] arrayMode;
    private String[] arrayPower;

    private Spinner spProtocol;
    Button btnSetProtocol,btGetProtocol;

    private final static int WHAT_GET_POWER = 1;
    private final static int WHAT_GET_FRE = 2;
    private Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case WHAT_GET_POWER:
                    int iPower = (int) msg.obj;
                    int index = getPowerIndex(iPower);
                    if (index > -1) {
                        spPower.setSelection(index);
                        if (msg.arg1 == 1)
                            showToast(R.string.get_power_succ);
                    } else {
                        if (msg.arg1 == 1)
                            showToast(R.string.get_power_fail);
                    }
                    break;
                case WHAT_GET_FRE:
                    int idx = (int) msg.obj;
                    if (idx != -1) {
                        int count = spMode.getCount();
                        idx = getModeIndex(idx);
                        spMode.setSelection(idx > count - 1 ? count - 1 : idx);
                        if (msg.arg1 == 1)
                            showToast(R.string.uhf_msg_read_frequency_succ);
                    } else {
                        if (msg.arg1 == 1)
                            showToast(R.string.uhf_msg_read_frequency_fail);
                    }
                    break;
            }
        }
    };

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        Log.e(TAG, "onCreateView");
        View view = inflater.inflate(R.layout.fragment_uhf_set, container, false);
        init(view);
        return view;
    }

    private void init(View view) {
        spMode = view.findViewById(R.id.spMode);
        arrayMode = getResources().getStringArray(R.array.arrayMode);
        setSpinnerData(spMode, arrayMode);

        spProtocol= view.findViewById(R.id.spProtocol);

        btGetProtocol = view.findViewById(R.id.btnGetProtocol);
        btGetProtocol.setOnClickListener(this);
        btnSetProtocol = view.findViewById(R.id.btnSetProtocol);
        btnSetProtocol.setOnClickListener(this);

        BtSetFre = view.findViewById(R.id.BtSetFre);
        BtSetFre.setOnClickListener(this);
        BtGetFre = view.findViewById(R.id.BtGetFre);
        BtGetFre.setOnClickListener(this);

        spPower = view.findViewById(R.id.spPower);
        arrayPower = getResources().getStringArray(R.array.arrayPower);
        setSpinnerData(spPower, arrayPower);

        btnSetPower = view.findViewById(R.id.btnSetPower);
        btnSetPower.setOnClickListener(this);
        btnGetPower = view.findViewById(R.id.btnGetPower);
        btnGetPower.setOnClickListener(this);

        btnBeepOpen = view.findViewById(R.id.btnBeepOpen);
        btnBeepOpen.setOnClickListener(this);
        btnBeepClose = view.findViewById(R.id.btnBeepClose);
        btnBeepClose.setOnClickListener(this);

        cbVoiceCues = view.findViewById(R.id.cbVoiceCues);
        cbVoiceCues.setChecked(SPUtil.getShared(Constant.VOICE_CUES, true));
        cbVoiceCues.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(SPUtil.saveShared(Constant.VOICE_CUES, isChecked)) {
                    mActivity.setVoiceCuesOpen(isChecked);
                } else {
                    showToast(getString(R.string.setup_failed));
                }
            }
        });
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        Log.e(TAG, "onActivityCreated");
        mActivity = (MainActivity) getActivity();
    }

    @Override
    public void setUserVisibleHint(boolean isVisibleToUser) {
        super.setUserVisibleHint(isVisibleToUser);
        initDefaultConfig();
    }

    @Override
    public void onResume() {
        super.onResume();
        initDefaultConfig();
    }

    @Override
    public void onClick(View v) {
        if(!mActivity.isOpenConnected()) {
            showToast(R.string.open_connect_first);
            return;
        }
        switch (v.getId()) {
            case R.id.BtSetFre:
                setFre();
                break;
            case R.id.BtGetFre:
                getFre(true);
                break;
            case R.id.btnSetPower:
                setPower();
                break;
            case R.id.btnGetPower:
                getPower(true);
                break;
            case R.id.btnBeepOpen:
                openBeep();
                break;
            case R.id.btnBeepClose:
                closeBeep();
                break;
            case R.id.btnGetProtocol:
                getProtocol(true);
                break;
            case R.id.btnSetProtocol:
                setProtocol();
                break;
        }
    }

    private int getMode(String modeName) {
        if (modeName.equals(getString(R.string.China_Standard_840_845MHz))) {
            return 0x01;
        } else if (modeName.equals(getString(R.string.China_Standard_920_925MHz))) {
            return 0x02;
        } else if (modeName.equals(getString(R.string.ETSI_Standard))) {
            return 0x04;
        } else if (modeName.equals(getString(R.string.United_States_Standard))) {
            return 0x08;
        } else if (modeName.equals(getString(R.string.Korea))) {
            return 0x16;
        } else if (modeName.equals(getString(R.string.Japan))) {
            return 0x32;
        } /*else if (modeName.equals(getString(R.string.Fixed_Frequency_915MHz))) {
            return 3;
        } else if (modeName.equals(getString(R.string.China_Standard_plus))) {
            return 4;
        } else if (modeName.equals(getString(R.string.Morocco))) {
            return 7;
        } else if (modeName.equals(getString(R.string.South_Africa_915_919MHz))) {
            return 8;
        } else if (modeName.equals(getString(R.string.New_Zealand))) {
            return 9;
        }*/
        return 0x08;
    }

    private String getModeName(int mode) {
        switch (mode) {
            case 0x01:
                return getString(R.string.China_Standard_840_845MHz);
            case 0x02:
                return getString(R.string.China_Standard_920_925MHz);
            case 0x04:
                return getString(R.string.ETSI_Standard);
            case 0x08:
                return getString(R.string.United_States_Standard);
            case 0x16:
                return getString(R.string.Korea);
            case 0x32:
                return getString(R.string.Japan);
            /*case 3:
                return getString(R.string.Fixed_Frequency_915MHz);
            case 7:
                return getString(R.string.China_Standard_840_845MHz);
            case 8:
                return getString(R.string.South_Africa_915_919MHz);
            case 9:
                return getString(R.string.New_Zealand);*/
            default:
                return getString(R.string.United_States_Standard);
        }
    }

    private int getModeIndex(String modeName) {
        for (int i = 0; i < arrayMode.length; i++) {
            if (arrayMode[i].equals(modeName)) {
                return i;
            }
        }
        return 0;
    }

    private int getModeIndex(int mode) {
        return getModeIndex(getModeName(mode));
    }

    private int getPowerIndex(int power) {
        if (arrayPower != null && power > -1) {
            for (int i = 0; i < arrayPower.length; i++) {
                if (power == Integer.valueOf(arrayPower[i])) {
                    return i;
                }
            }
        }
        return -1;
    }

    /**
     * 设置工作模式
     */
    private void setFre() {
        String modeName = spMode.getSelectedItem().toString();
        int mode = getMode(modeName);
        Log.e(TAG, "setFre mode=" + mode);
        if (mActivity.mRFIDWithUHFUSB.setFrequencyMode((byte) mode)) {
            showToast(R.string.uhf_msg_set_frequency_succ);
        } else {
            showToast(R.string.uhf_msg_set_frequency_fail);
        }
    }

    /**
     * 获取工作模式
     *
     * @param showToast
     */
    public void getFre(boolean showToast) {
        int mode = mActivity.mRFIDWithUHFUSB.getFrequencyMode();
        Log.e(TAG, "getFre mode=" + mode);

        Message msg = mHandler.obtainMessage(WHAT_GET_FRE, mode);
        msg.arg1 = showToast ? 1 : 0;
        mHandler.sendMessage(msg);
    }

    /**
     * 获取输出功率
     *
     * @param showToast
     */
    private void getPower(boolean showToast) {
        int iPower = mActivity.mRFIDWithUHFUSB.getPower();
        Log.e(TAG, "getPower=" + iPower);

        Message msg = mHandler.obtainMessage(WHAT_GET_POWER, iPower);
        msg.arg1 = showToast ? 1 : 0;
        mHandler.sendMessage(msg);
    }

    /**
     * 设置输出功率
     */
    private void setPower() {
        int iPower = Integer.valueOf(spPower.getSelectedItem().toString());
        Log.e(TAG, "setPower=" + iPower);
        if (mActivity.mRFIDWithUHFUSB.setPower(iPower)) {
            showToast(R.string.set_power_succ);
        } else {
            showToast(R.string.set_power_fail);
        }
    }

    private boolean setBeep(boolean open) {
        return mActivity.mRFIDWithUHFUSB.setBeep(open);
    }

    /**
     * 打开蜂鸣器
     */
    private void openBeep() {
        if(setBeep(true)) {
            showToast("Open success");
        } else {
            showToast("Open failure");
        }
    }

    /**
     * 关闭蜂鸣器
     */
    private void closeBeep() {
        if(setBeep(false)) {
            showToast("Close success");
        } else {
            showToast("Close failure");
        }
    }

    /**
     * 获取默认配置
     */
    private void initDefaultConfig() {
        if(getUserVisibleHint() && mActivity != null) {
            new Thread() {
                @Override
                public void run() {
                    getPower(false);
                    getFre(false);
                }
            }.start();
        }
    }

    private void setSpinnerData(Spinner spinner, String[] array) {
        ArrayAdapter adapter = new ArrayAdapter(getContext(), android.R.layout.simple_spinner_item, array);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner.setAdapter(adapter);
    }

    /**
     * 设置协议
     *
     * @return
     */
    private boolean setProtocol() {
        if (mActivity.mRFIDWithUHFUSB.setProtocol(spProtocol.getSelectedItemPosition())) {
            mActivity.showToast(R.string.uhf_msg_set_protocol_succ);
            return true;
        } else {
            mActivity.showToast(R.string.uhf_msg_get_protocol_fail);
        }
        return false;
    }

    /**
     * 获取协议
     *
     * @param showToast
     * @return
     */
    private void getProtocol(boolean showToast) {
        int pro = mActivity.mRFIDWithUHFUSB.getProtocol();
        if (pro >= 0 && pro < spProtocol.getCount()) {
            spProtocol.setSelection(pro);
            mActivity.showToast(R.string.uhf_msg_get_protocol_succ);
        } else {
            mActivity.showToast(R.string.uhf_msg_get_protocol_fail);
        }
    }
}
