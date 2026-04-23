package com.example.demo_uhf_usb.fragment;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;

import androidx.annotation.IdRes;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.example.demo_uhf_usb.BaseFragment;
import com.example.demo_uhf_usb.MainActivity;
import com.example.demo_uhf_usb.R;
import com.rscja.deviceapi.RFIDWithUHFUSB;
import com.rscja.utility.StringUtility;

import java.util.ArrayList;

/**
 * Created by WuShengjun on 2019/6/24.
 * Description:
 */

public class UHFLockFragment extends BaseFragment implements View.OnClickListener {

    private String TAG = "CW_UHFLockFragment";

    private MainActivity mActivity;
    private CheckBox cb_filter;
    private EditText etPtr_filter, etLen_filter, etData_filter;
    private RadioGroup rgFilterBank;
    private RadioButton rbEPC_filter, rbTID_filter, rbUser_filter;

    private EditText EtAccessPwd_Lock, etLockCode;
    private Button btnLock;

    private AlertDialog mLockCodeDialog;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        Log.e(TAG, "onCreateView");
        View view = inflater.inflate(R.layout.fragment_uhf_lock, container, false);
        init(view);
        return view;
    }

    private void init(View view) {
        etPtr_filter = view.findViewById(R.id.etPtr_filter);
        etLen_filter = view.findViewById(R.id.etLen_filter);
        etData_filter = view.findViewById(R.id.etData_filter);

        rbEPC_filter = view.findViewById(R.id.rbEPC_filter);
        rbTID_filter = view.findViewById(R.id.rbTID_filter);
        rbUser_filter = view.findViewById(R.id.rbUser_filter);

        cb_filter = view.findViewById(R.id.cb_filter);
        cb_filter.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    String data = etData_filter.getText().toString().trim();
                    String rex = "[\\da-fA-F]*"; //匹配正则表达式，数据为十六进制格式
                    if (data == null || data.isEmpty() || !data.matches(rex)) {
                        showToast(getString(R.string.must_be_hex_data));
                        cb_filter.setChecked(false);
                    }
                }
            }
        });

        rgFilterBank = view.findViewById(R.id.rgFilterBank);
        rgFilterBank.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, @IdRes int checkedId) {
                if (checkedId == R.id.rbEPC_filter) {
                    etPtr_filter.setText("32");
                } else {
                    etPtr_filter.setText("0");
                }
            }
        });

        EtAccessPwd_Lock = view.findViewById(R.id.EtAccessPwd_Lock);
        etLockCode = view.findViewById(R.id.etLockCode);
        etLockCode.setOnClickListener(this);

        btnLock = view.findViewById(R.id.btnLock);
        btnLock.setOnClickListener(this);
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        mActivity = (MainActivity) getActivity();
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.etLockCode:
                generateLockCode();
                break;
            case R.id.btnLock:
                lock();
                break;
        }
    }

    private void generateLockCode() {
        if(mLockCodeDialog == null) {
            AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
            builder.setTitle(R.string.tvLockCode);
            View vv = LayoutInflater.from(getContext()).inflate(R.layout.uhf_dialog_lock_code, null);
            final RadioButton rbOpen = vv.findViewById(R.id.rbOpen);
            final CheckBox cbPerm = vv.findViewById(R.id.cbPerm);

            final CheckBox cbKill = vv.findViewById(R.id.cbKill);
            final CheckBox cbAccess = vv.findViewById(R.id.cbAccess);
            final CheckBox cbEPC = vv.findViewById(R.id.cbEPC);
            final CheckBox cbTid = vv.findViewById(R.id.cbTid);
            final CheckBox cbUser = vv.findViewById(R.id.cbUser);

            builder.setView(vv);
            builder.setNegativeButton(android.R.string.cancel, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    dialogInterface.dismiss();
                }
            });
            builder.setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {

                    ArrayList<Integer> lockBankEnums = new ArrayList<>();
                    int lockModeEnum;

                    if (rbOpen.isChecked()) {
                        if (cbPerm.isChecked()) {
                            lockModeEnum = RFIDWithUHFUSB.LockMode_POPEN;
                        } else {
                            lockModeEnum = RFIDWithUHFUSB.LockMode_OPEN;
                        }
                    } else {
                        if (cbPerm.isChecked()) {
                            lockModeEnum = RFIDWithUHFUSB.LockMode_PLOCK;
                        } else {
                            lockModeEnum = RFIDWithUHFUSB.LockMode_LOCK;
                        }
                    }

                    if (cbKill.isChecked()) {
                        lockBankEnums.add(RFIDWithUHFUSB.LockBank_KILL);
                    }
                    if (cbAccess.isChecked()) {
                        lockBankEnums.add(RFIDWithUHFUSB.LockBank_ACCESS);
                    }
                    if (cbEPC.isChecked()) {
                        lockBankEnums.add(RFIDWithUHFUSB.LockBank_EPC);
                    }
                    if (cbTid.isChecked()) {
                        lockBankEnums.add(RFIDWithUHFUSB.LockBank_TID);
                    }
                    if (cbUser.isChecked()) {
                        lockBankEnums.add(RFIDWithUHFUSB.LockBank_USER);
                    }
                    if (!lockBankEnums.isEmpty()) {
                        etLockCode.setText(mActivity.mRFIDWithUHFUSB.generateLockCode(lockBankEnums, lockModeEnum));
                    }
                }
            });
            mLockCodeDialog = builder.create();
        }
        mLockCodeDialog.show();
    }

    public void lock() {
        if(!mActivity.isOpenConnected()) {
            showToast(R.string.open_connect_first);
            return;
        }

        String strPWD = EtAccessPwd_Lock.getText().toString().trim();// 访问密码
        String strLockCode = etLockCode.getText().toString().trim();
        if (!TextUtils.isEmpty(strPWD)) {
            if (strPWD.length() != 8) {
                showToast(R.string.uhf_msg_addr_must_len8);
                return;
            } else if (!StringUtility.isHexNumberRex(strPWD)) {
                showToast(R.string.rfid_mgs_error_nohex);
                return;
            }
        } else {
            showToast(R.string.rfid_mgs_error_nopwd);
            return;
        }

        if (TextUtils.isEmpty(strLockCode)) {
            showToast(R.string.rfid_mgs_error_nolockcode);
            return;
        }

        boolean result = false;
        if (cb_filter.isChecked()) {
            String filterData = etData_filter.getText().toString();
            if (TextUtils.isEmpty(filterData)) {
                showToast(getString(R.string.data_can_not_be_empty));
                return;
            }
            String ptrFilter = etPtr_filter.getText().toString();
            if (TextUtils.isEmpty(ptrFilter)) {
                showToast(getString(R.string.start_address_can_not_be_empty));
                return;
            }
            String lenStr = etLen_filter.getText().toString();
            if (TextUtils.isEmpty(lenStr)) {
                showToast(getString(R.string.data_length_can_not_be_empty));
                return;
            }

            int filterPtr = Integer.parseInt(ptrFilter);
            int filterCnt = Integer.parseInt(lenStr);
            int filterBank = RFIDWithUHFUSB.Bank_EPC;
            if (rbEPC_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_EPC;
            } else if (rbTID_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_TID;
            } else if (rbUser_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_USER;
            }

            result = mActivity.mRFIDWithUHFUSB.lockMem(strPWD,
                    filterBank,
                    filterPtr,
                    filterCnt,
                    filterData,
                    strLockCode);
        } else {
            result = mActivity.mRFIDWithUHFUSB.lockMem(strPWD, strLockCode);
        }

        if (result) {
            showToast(R.string.rfid_mgs_lock_succ);
            mActivity.playSound(1);
        } else {
            showToast(R.string.rfid_mgs_lock_fail);
            mActivity.playSound(2);
        }
    }
}
