package com.example.demo_uhf_usb.fragment;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.hardware.usb.UsbDevice;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;

import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.SimpleAdapter;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.example.demo_uhf_usb.BaseFragment;
import com.example.demo_uhf_usb.MainActivity;
import com.example.demo_uhf_usb.R;
import com.example.demo_uhf_usb.Utils;
import com.example.demo_uhf_usb.interfac.OnUSBStatusChangedListener;
import com.example.demo_uhf_usb.utils.NumberTool;
import com.rscja.deviceapi.RFIDWithUHFUART;
import com.rscja.deviceapi.entity.UHFTAGInfo;
import com.rscja.deviceapi.interfaces.ConnectionStatus;
import com.rscja.deviceapi.interfaces.KeyEventCallback;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * Created by WuShengjun on 2019/6/24.
 * Description:
 */

public class UHFInventoryFragment extends BaseFragment implements View.OnClickListener {

    private String TAG = "CW_UHFInventoryFragment";

    private MainActivity mActivity;
    private TextView tv_count, tv_total, tv_time;
    private ListView LvTags;
    private Button btnInventory, btnInventoryLoop, btnStop, btnClear;

    private long mStartTime;
    private List<HashMap<String, String>> mTagList = new ArrayList<>();
    private List<String> tempDatas = new ArrayList<>();
    private SimpleAdapter mAdapter;
    private int total;
    private final String TAG_EPC = "tagEPC";
    private final String TAG_Data = "tagData";
    private final String TAG_COUNT = "tagCount";
    private final String TAG_RSSI = "tagRSSI";
    private RadioButton rbEPC, rbEPC_TID, rbEPC_TID_USER;

    private CheckBox cbFilter;
    private ViewGroup layout_filter;
    private Button btnSetFilter;

    private AlertDialog mDialog;
    private boolean loopFlag;
    private EditText etUserPtr, etUserLen;
    private static final int WHAT_INVENTORY = 1;
    private static final int WHAT_STOP_INVENTORY_FAIL = 2;
    private Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case WHAT_INVENTORY:
                    UHFTAGInfo info = (UHFTAGInfo) msg.obj;
                    addDataToList( info.getEPC(),mergeTidEpc(info.getTid(), info.getEPC(),info.getUser()), info.getRssi());
                    break;
                case WHAT_STOP_INVENTORY_FAIL:
                    showToast("Stopping inventory failure");
                    break;
            }
        }
    };

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_uhf_inventory, container, false);
        init(view);
        return view;
    }

    private void init(View view) {

        rbEPC = (RadioButton) view.findViewById(R.id.rbEPC);
        rbEPC.setOnClickListener(this);
        rbEPC_TID = (RadioButton) view.findViewById(R.id.rbEPC_TID);
        rbEPC_TID.setOnClickListener(this);
        rbEPC_TID_USER = (RadioButton) view.findViewById(R.id.rbEPC_TID_USER);
        rbEPC_TID_USER.setOnClickListener(this);


        tv_count = view.findViewById(R.id.tv_count);
        tv_total = view.findViewById(R.id.tv_total);
        tv_time = view.findViewById(R.id.tv_time);

        btnInventory = view.findViewById(R.id.btnInventory);
        btnInventory.setOnClickListener(this);
        btnInventoryLoop = view.findViewById(R.id.btnInventoryLoop);
        btnInventoryLoop.setOnClickListener(this);
        btnStop = view.findViewById(R.id.btnStop);
        btnStop.setOnClickListener(this);
        btnClear = view.findViewById(R.id.btnClear);
        btnClear.setOnClickListener(this);

        mAdapter = new SimpleAdapter(getContext(), mTagList, R.layout.item_list_tag,
                new String[]{TAG_Data, TAG_COUNT, TAG_RSSI},
                new int[]{R.id.tvTagEpc, R.id.tvTagCount, R.id.tvTagRssi});
        LvTags = view.findViewById(R.id.LvTags);
        LvTags.setAdapter(mAdapter);

        initFilter(view);
    }



    private void initFilter(View view) {
        layout_filter = (ViewGroup) view.findViewById(R.id.layout_filter);
        layout_filter.setVisibility(View.GONE);
        cbFilter = (CheckBox) view.findViewById(R.id.cbFilter);
        cbFilter.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                layout_filter.setVisibility(isChecked ? View.VISIBLE : View.GONE);
            }
        });
        cbFilter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(!cbFilter.isChecked()){
                    Toast.makeText(mActivity,""+android.os.Build.TAGS,Toast.LENGTH_SHORT).show();
                    //禁用过滤
                    String dataStr = "00";
                    if (mActivity.mRFIDWithUHFUSB.setFilter(RFIDWithUHFUART.Bank_EPC, 0, 0, dataStr)) {
                        showToast(R.string.msg_disable_succ);
                    } else {
                        showToast(R.string.msg_disable_fail);
                    }
                    return;
                }
            }
        });

        final EditText etLen = (EditText) view.findViewById(R.id.etLen);
        final EditText etPtr = (EditText) view.findViewById(R.id.etPtr);
        final EditText etData = (EditText) view.findViewById(R.id.etData);
        final RadioButton rbEPC = (RadioButton) view.findViewById(R.id.rbEPC_filter);
        final RadioButton rbTID = (RadioButton) view.findViewById(R.id.rbTID_filter);
        final RadioButton rbUser = (RadioButton) view.findViewById(R.id.rbUser_filter);
        btnSetFilter = (Button) view.findViewById(R.id.btSet);

        btnSetFilter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                int filterBank = RFIDWithUHFUART.Bank_EPC;
                if (rbEPC.isChecked()) {
                    filterBank = RFIDWithUHFUART.Bank_EPC;
                } else if (rbTID.isChecked()) {
                    filterBank = RFIDWithUHFUART.Bank_TID;
                } else if (rbUser.isChecked()) {
                    filterBank = RFIDWithUHFUART.Bank_USER;
                }
                if (etLen.getText().toString() == null || etLen.getText().toString().isEmpty()) {
                    showToast("数据长度不能为空");
                    return;
                }
                if (etPtr.getText().toString() == null || etPtr.getText().toString().isEmpty()) {
                    showToast("起始地址不能为空");
                    return;
                }
                int ptr = Utils.toInt(etPtr.getText().toString(), 0);
                int len = Utils.toInt(etLen.getText().toString(), 0);
                String data = etData.getText().toString().trim();
                if (len > 0) {
                    String rex = "[\\da-fA-F]*"; //匹配正则表达式，数据为十六进制格式
                    if (data == null || data.isEmpty() || !data.matches(rex)) {
                        showToast("过滤的数据必须是十六进制数据");
                        return;
                    }

                    int l = data.replace(" ", "").length();
                    if (len <= l * 4) {
                        if(l % 2 != 0)
                            data += "0";
                    } else {
                        showToast(R.string.uhf_msg_set_filter_fail2);
                        return;
                    }

                    if (mActivity.mRFIDWithUHFUSB.setFilter(filterBank, ptr, len, data)) {
                        showToast(R.string.uhf_msg_set_filter_succ);
                    } else {
                        showToast(R.string.uhf_msg_set_filter_fail);
                    }
                } else {
                    //禁用过滤
                    String dataStr = "00";
                    if (mActivity.mRFIDWithUHFUSB.setFilter(RFIDWithUHFUART.Bank_EPC, 0, 0, dataStr)) {
                        showToast(R.string.msg_disable_succ);
                    } else {
                        showToast(R.string.msg_disable_fail);
                    }
                    cbFilter.setChecked(false);
                }

            }
        });

        rbEPC.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (rbEPC.isChecked()) {
                    etPtr.setText("32");
                }
            }
        });
        rbTID.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (rbTID.isChecked()) {
                    etPtr.setText("0");
                }
            }
        });
        rbUser.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (rbUser.isChecked()) {
                    etPtr.setText("0");
                }
            }
        });
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        Log.e(TAG, "onActivityCreated");
        mActivity = (MainActivity) getActivity();
        mActivity.setOnUSBStatusChangedListener(new OnUSBStatusChangedListener() {
            @Override
            public void onStatusChanged(UsbDevice usbDevice, int status) {
                if(status == OnUSBStatusChangedListener.CONNECTED) {
                    setViewEnabled(true);
                } else {
                    setViewEnabled(false);
                    if(loopFlag) {
                        stopInventory();
                    }
                }
            }
        });

        setViewEnabled(mActivity.isOpenConnected());
    }

    @Override
    public void onDestroyView() {
        Log.e(TAG, "onDestroyView");
        clear();
        stopInventory();
        mHandler.removeCallbacksAndMessages(null);
        mActivity.mRFIDWithUHFUSB.setKeyEventCallback(null);
        super.onDestroyView();
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.btnInventory:
                inventory();
                break;
            case R.id.btnInventoryLoop:
                inventoryLoop();
                break;
            case R.id.btnStop:
                stopInventory();
                break;
            case R.id.btnClear:
                clear();
                break;
            case R.id.rbEPC:
                setMode(Mode.EPC);
                break;
            case R.id.rbEPC_TID:
                setMode(Mode.EPC_TID);
                break;
            case R.id.rbEPC_TID_USER:
                alertSet();
                break;
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        mActivity.mRFIDWithUHFUSB.setKeyEventCallback(new MyKeyEventCallback());
        Log.e(TAG, "onResume");
    }
    @Override
    public void setUserVisibleHint(boolean isVisibleToUser) {
        super.setUserVisibleHint(isVisibleToUser);
        Log.e(TAG, "isVisibleToUser="+isVisibleToUser);
         if(!isVisibleToUser){
             if(mActivity!=null)
                mActivity.mRFIDWithUHFUSB.setKeyEventCallback(null);
         }else{
             if(mActivity!=null)
                mActivity.mRFIDWithUHFUSB.setKeyEventCallback(new MyKeyEventCallback());
         }
    }

    @Override
    public void onPause() {
        super.onPause();
        Log.e(TAG, "onPause");
    }

    private void setViewEnabled(boolean enabled) {
        btnInventory.setEnabled(enabled);
        btnInventoryLoop.setEnabled(enabled);
//        btnStop.setEnabled(!enabled);
    }

    private void inventory() {
        if(!mActivity.isOpenConnected()) {
            showToast(R.string.open_connect_first);
            return;
        }
        UHFTAGInfo uhftagInfo = mActivity.mRFIDWithUHFUSB.inventorySingleTag();
        mStartTime = System.currentTimeMillis();
        if (uhftagInfo != null) {
            Message msg = mHandler.obtainMessage(WHAT_INVENTORY, uhftagInfo);
            mHandler.sendMessage(msg);
            mActivity.playSound(1);
        }
    }

    private synchronized void inventoryLoop() {
        if(!mActivity.isOpenConnected()) {
            showToast(R.string.open_connect_first);
            return;
        }
        if(mActivity.mRFIDWithUHFUSB.startInventoryTag()) {
            Log.e(TAG, "start inventoryLoop");
            loopFlag = true;
            setViewEnabled(false);
            tv_time.setText("0s");
            mStartTime = System.currentTimeMillis();
            new InventoryLoopThread().start();
        } else {
            showToast("Starting inventory failure");
        }
    }

    private synchronized void stopInventory() {

        if(mActivity.isOpenConnected()) {
            setViewEnabled(true);
        } else {
            return;
        }

        if(!mActivity.mRFIDWithUHFUSB.stopInventory()) {
            mHandler.sendEmptyMessage(WHAT_STOP_INVENTORY_FAIL);
        }
        sleepTime(200);
        loopFlag = false;
    }

    private void clear() {
        total = 0;
        tv_count.setText("0");
        tv_total.setText("0");
        tv_time.setText("0s");
        mTagList.clear();
        tempDatas.clear();
        mAdapter.notifyDataSetChanged();
    }

    /**
     * 添加EPC到列表中
     * @param
     */
    private void addDataToList(String epc,String epcAndTidUser, String rssi) {
        if (!TextUtils.isEmpty(epc)) {
            int index = checkIsExist(epc);
            HashMap<String, String> map = new HashMap<>();
            map.put(TAG_Data, epcAndTidUser);
            map.put(TAG_EPC, epc);
            map.put(TAG_COUNT, String.valueOf(1));
            map.put(TAG_RSSI, rssi);
            if (index == -1) {
                mTagList.add(map);
                tempDatas.add(epc);
            } else {
                int tagCount = Integer.parseInt(mTagList.get(index).get(TAG_COUNT), 10) + 1;
                map.put(TAG_COUNT, String.valueOf(tagCount));
                map.put(TAG_Data, epcAndTidUser);
                mTagList.set(index, map);
            }

            updateDataViews();
        }
    }

    private void updateDataViews() {
        setTotalTime();
        tv_count.setText(String.valueOf(mAdapter.getCount()));
        tv_total.setText(String.valueOf(++total));
        mAdapter.notifyDataSetChanged();
    }

    private void setTotalTime() {
        float useTime = (System.currentTimeMillis() - mStartTime) / 1000.0F;
//        tv_time.setText(NumberTool.getPointDouble(loopFlag ? 1 : 3, useTime) + "s");
        tv_time.setText(NumberTool.getPointDouble(1, useTime) + "s");
    }

    /**
     * 二分查找，找到该值在数组中的下标，否则为-1
     * @param array
     * @param src
     * @return
     */
    private int binarySearch(List<String> array, String src) {
        int left = 0;
        int right = array.size() - 1;
        // 这里必须是 <=
        while (left <= right) {
            if (compareString(array.get(left), src)) {
                return left;
            } else if (left != right) {
                if (compareString(array.get(right), src))
                    return right;
            }
            left++;
            right--;
        }
        return -1;
    }

    /**
     * 判断EPC是否在列表中
     * @param epc 索引
     * @return
     */
    public int checkIsExist(String epc) {
        int existFlag = -1;
        if (TextUtils.isEmpty(epc)) {
            return existFlag;
        }
        return binarySearch(tempDatas, epc);
    }

    private boolean compareString(String str1, String str2) {
        if (str1.length() != str2.length()) {
            return false;
        } else if (str1.hashCode() != str2.hashCode()) {
            return false;
        } else {
            char[] value1 = str1.toCharArray();
            char[] value2 = str2.toCharArray();
            int size = value1.length;
            for (int k = 0; k < size; k++) {
                if (value1[k] != value2[k]) {
                    return false;
                }
            }
            return true;
        }
    }

    private String mergeTidEpc(String tid, String epc,String user) {
        if(!TextUtils.isEmpty(tid) && !tid.equals("0000000000000000") && !tid.equals("000000000000000000000000")){
            if(!TextUtils.isEmpty(user)){
                return "TID:" + tid + "\nEPC:" + epc +"\nUSER:"+user;
            }else{
                return "TID:" + tid + "\nEPC:" + epc;
            }
        } else {
            return epc;
        }

    }

    private void sleepTime(long time) {
        try {
            Thread.sleep(time);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    class InventoryLoopThread extends Thread {
        public synchronized void run() {
            UHFTAGInfo info;
            Message msg;
            while (loopFlag) {
                info = mActivity.mRFIDWithUHFUSB.readTagFromBuffer();
                if (info != null) {
                    msg = mHandler.obtainMessage(WHAT_INVENTORY, info);
                    mHandler.sendMessage(msg);
                    mActivity.playSound(1);
                }
            }
        }
    }

    public enum  Mode {
        EPC, EPC_TID, EPC_TID_USER
    }

    private void alertSet() {
        if (mDialog == null) {
            View view = LayoutInflater.from(getContext()).inflate(R.layout.dialog_epc_tid_user, null);
            etUserPtr = view.findViewById(R.id.etUserPtr);
            etUserLen = view.findViewById(R.id.etUserLen);
            DialogInterface.OnClickListener positiveListener = new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    setMode(Mode.EPC_TID_USER);
                }
            };
            mDialog = getAlert(view, "EPC+TID+USER", null, false, positiveListener);
         }
        if (!mDialog.isShowing()) {
            mDialog.show();
        }
    }

    private AlertDialog getAlert(View view, String title, String message, boolean cancelable, DialogInterface.OnClickListener positiveListener) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setIcon(R.drawable.webtext);
        if(view != null) {
            builder.setView(view);
        } else {
            builder.setMessage(message);
        }
        builder.setCancelable(cancelable);
        if(positiveListener != null) {
            builder.setPositiveButton(R.string.ok, positiveListener);
        } else {
            builder.setNegativeButton(R.string.close, null);
        }
        return builder.create();
    }
    private void setMode(Mode mode) {
        switch (mode) {
            case EPC:
                if (mActivity.mRFIDWithUHFUSB.setEPCMode()) {
                    showToast("success");
                } else {
                    showToast("fail");
                }
                break;
            case EPC_TID:
                if (mActivity.mRFIDWithUHFUSB.setEPCAndTIDMode()) {
                    showToast("success");
                } else {
                    showToast("fail");
                }
                break;
            case EPC_TID_USER:
                String strUserPtr = etUserPtr.getText().toString();
                String strUserLen = etUserLen.getText().toString();
                int userPtr = 0;
                int userLen = 6;
                if (!TextUtils.isEmpty(strUserPtr)) {
                    userPtr = Integer.valueOf(strUserPtr);
                }
                if (!TextUtils.isEmpty(strUserLen)) {
                    userLen = Integer.valueOf(strUserLen);
                }
                if (mActivity.mRFIDWithUHFUSB.setEPCAndTIDUserMode(userPtr, userLen)) {
                    showToast("success");
                } else {
                    showToast("fail");
                }
                break;
        }
    }
    class  MyKeyEventCallback implements KeyEventCallback{
        @Override
        public void onKeyDown(int i) {
            Log.e(TAG, "setKeyEventCallback>onKeyDown=" + i);
            if(!loopFlag) {
                if(mActivity.mRFIDWithUHFUSB.getConnectStatus()==ConnectionStatus.CONNECTED) {
                    inventoryLoop();
                }
            } else {
                stopInventory();
            }
        }
    }
}
