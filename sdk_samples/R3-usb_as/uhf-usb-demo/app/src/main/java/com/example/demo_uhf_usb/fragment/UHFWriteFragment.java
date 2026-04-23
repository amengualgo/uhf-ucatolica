package com.example.demo_uhf_usb.fragment;

import android.os.Bundle;

import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;

import androidx.annotation.IdRes;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.example.demo_uhf_usb.BaseFragment;
import com.example.demo_uhf_usb.MainActivity;
import com.example.demo_uhf_usb.R;
import com.rscja.deviceapi.RFIDWithUHFUSB;
import com.rscja.utility.StringUtility;

/**
 * Created by WuShengjun on 2019/6/24.
 * Description:
 */

public class UHFWriteFragment extends BaseFragment {

    private String TAG = "UHFWriteFragment";

    private MainActivity mActivity;
    private CheckBox cb_filter;
    private EditText etPtr_filter, etLen_filter, etData_filter;
    private RadioGroup rgFilterBank;
    private RadioButton rbEPC_filter, rbTID_filter, rbUser_filter;

    private Spinner SpinnerBank_Write;
    private EditText EtPtr_Write, EtLen_Write, EtAccessPwd_Write, EtData_Write;
    private Button btnWrite;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_uhf_write, container, false);
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

        EtPtr_Write = view.findViewById(R.id.EtPtr_Write);
        EtLen_Write = view.findViewById(R.id.EtLen_Write);
        EtAccessPwd_Write = view.findViewById(R.id.EtAccessPwd_Write);
        EtData_Write = view.findViewById(R.id.EtData_Write);

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

        SpinnerBank_Write = view.findViewById(R.id.SpinnerBank_Write);
        SpinnerBank_Write.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                String element = adapterView.getItemAtPosition(i).toString();// 得到spanner的值
                if (element.equals("EPC")) {
                    EtPtr_Write.setText("2");
                } else {
                    EtPtr_Write.setText("0");
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });

        btnWrite = view.findViewById(R.id.btnWrite);
        btnWrite.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(mActivity.isOpenConnected()) {
                    write();
                } else {
                    showToast(getString(R.string.open_connect_first));
                }
            }
        });
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        mActivity = (MainActivity) getActivity();
    }

    private void write() {
        String strPtr = EtPtr_Write.getText().toString().trim();
        if (strPtr == null || strPtr.isEmpty()) {
            showToast(R.string.uhf_msg_addr_not_null);
            return;
        } else if (!StringUtility.isDecimal(strPtr)) {
            showToast(R.string.uhf_msg_addr_must_decimal);
            return;
        }

        String strPWD = EtAccessPwd_Write.getText().toString().trim();// 访问密码
        if (TextUtils.isEmpty(strPWD)) {
            strPWD = "00000000";
        }
        if (strPWD.length() != 8) {
            showToast(R.string.uhf_msg_addr_must_len8);
            return;
        } else if (!StringUtility.isHexNumberRex(strPWD)) {
            showToast(R.string.rfid_mgs_error_nohex);
            return;
        }

        String strData = EtData_Write.getText().toString().trim();// 要写入的内容
        if (TextUtils.isEmpty(strData)) {
            showToast(R.string.uhf_msg_write_must_not_null);
            return;
        } else if ((strData.length()) % 4 != 0) {
            showToast(R.string.uhf_msg_write_must_len4x);
            return;
        } else if (!StringUtility.isHexNumberRex(strData)) {
            showToast(R.string.rfid_mgs_error_nohex);
            return;
        }

        // 多字单次
        String cntStr = EtLen_Write.getText().toString().trim();
        if (TextUtils.isEmpty(cntStr)) {
            showToast(R.string.uhf_msg_len_not_null);
            return;
        } else if (!StringUtility.isDecimal(cntStr)) {
            showToast(R.string.uhf_msg_len_must_decimal);
            return;
        }

        boolean result = false;
        int bank = RFIDWithUHFUSB.Bank_RESERVED;
        String bankStr = SpinnerBank_Write.getSelectedItem().toString();
        if(bankStr.toUpperCase().contains("EPC")) {
            bank = RFIDWithUHFUSB.Bank_EPC;
        } else if(bankStr.toUpperCase().contains("TID")) {
            bank = RFIDWithUHFUSB.Bank_TID;
        } else if(bankStr.toUpperCase().contains("USER")) {
            bank = RFIDWithUHFUSB.Bank_USER;
        } else if(bankStr.toUpperCase().contains("RESERVED")) {
            bank = RFIDWithUHFUSB.Bank_RESERVED;
        }
        if (cb_filter.isChecked()) { // 指定标签
            if (TextUtils.isEmpty(etPtr_filter.getText().toString())) {
                etPtr_filter.setText("0");
            }
            if (TextUtils.isEmpty(etLen_filter.getText().toString())) {
                showToast(R.string.data_length_can_not_be_empty);
                return;
            }

            int filterPtr = Integer.parseInt(etPtr_filter.getText().toString());
            String filterData = etData_filter.getText().toString();
            int filterCnt = Integer.parseInt(etLen_filter.getText().toString());
            int filterBank = RFIDWithUHFUSB.Bank_EPC;
            if (rbEPC_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_EPC;
            } else if (rbTID_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_TID;
            } else if (rbUser_filter.isChecked()) {
                filterBank = RFIDWithUHFUSB.Bank_USER;
            }

            result = mActivity.mRFIDWithUHFUSB.writeData(strPWD,
                    filterBank,
                    filterPtr,
                    filterCnt,
                    filterData,
                    bank,
                    Integer.parseInt(strPtr),
                    Integer.parseInt(cntStr),
                    strData
            );
        } else {
            result = mActivity.mRFIDWithUHFUSB.writeData(strPWD,
                    bank,
                    Integer.parseInt(strPtr),
                    Integer.valueOf(cntStr), strData);
        }

        if (result) {
            showToast(R.string.uhf_msg_write_succ);
            mActivity.playSound(1);
        } else {
            showToast(R.string.uhf_msg_write_fail);
            mActivity.playSound(2);
        }
    }
}
