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

public class UHFReadFragment extends BaseFragment {

    private String TAG = "UHFReadFragment";

    private MainActivity mActivity;
    private CheckBox cb_filter;
    private EditText etPtr_filter, etLen_filter, etData_filter;
    private RadioGroup rgFilterBank;
    private RadioButton rbEPC_filter, rbTID_filter, rbUser_filter;

    private Spinner SpinnerBank_Read;
    private EditText EtPtr_Read, EtLen_Read, EtAccessPwd_Read, EtData_Read;
    private Button btnRead;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_uhf_read, container, false);
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

        EtPtr_Read = view.findViewById(R.id.EtPtr_Read);
        EtLen_Read = view.findViewById(R.id.EtLen_Read);
        EtAccessPwd_Read = view.findViewById(R.id.EtAccessPwd_Read);
        EtData_Read = view.findViewById(R.id.EtData_Read);

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
                if(checkedId == R.id.rbEPC_filter) {
                    etPtr_filter.setText("32");
                } else {
                    etPtr_filter.setText("0");
                }
            }
        });

        SpinnerBank_Read = view.findViewById(R.id.SpinnerBank_Read);
        SpinnerBank_Read.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                String element = adapterView.getItemAtPosition(i).toString();// 得到spanner的值
                if (element.equals("EPC")) {
                    EtPtr_Read.setText("2");
                } else {
                    EtPtr_Read.setText("0");
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });

        btnRead = view.findViewById(R.id.btnRead);
        btnRead.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(mActivity.isOpenConnected()) {
                    read();
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

    private void read() {
        String ptrStr = EtPtr_Read.getText().toString().trim();
        if (ptrStr.equals("")) {
            showToast(R.string.uhf_msg_addr_not_null);
            return;
        } else if (!TextUtils.isDigitsOnly(ptrStr)) {
            showToast(R.string.uhf_msg_addr_must_decimal);
            return;
        }

        String cntStr = EtLen_Read.getText().toString().trim();
        if (cntStr.equals("")) {
            showToast(R.string.uhf_msg_len_not_null);
            return;
        } else if (!TextUtils.isDigitsOnly(cntStr)) {
            showToast(R.string.uhf_msg_len_must_decimal);
            return;
        }

        String pwdStr = EtAccessPwd_Read.getText().toString().trim();
        if (!TextUtils.isEmpty(pwdStr)) {
            if (pwdStr.length() != 8) {
                showToast(R.string.uhf_msg_addr_must_len8);
                return;
            } else if (!StringUtility.isHexNumberRex(pwdStr)) {
                showToast(R.string.rfid_mgs_error_nohex);
                return;
            }
        } else {
            pwdStr = "00000000";
        }

        String data = "";
        int bank = RFIDWithUHFUSB.Bank_RESERVED;
        String bankStr = SpinnerBank_Read.getSelectedItem().toString();
        if(bankStr.toUpperCase().contains("EPC")) {
            bank = RFIDWithUHFUSB.Bank_EPC;
        } else if(bankStr.toUpperCase().contains("TID")) {
            bank = RFIDWithUHFUSB.Bank_TID;
        } else if(bankStr.toUpperCase().contains("USER")) {
            bank = RFIDWithUHFUSB.Bank_USER;
        } else if(bankStr.toUpperCase().contains("RESERVED")) {
            bank = RFIDWithUHFUSB.Bank_RESERVED;
        }
        if (cb_filter.isChecked()) { //  过滤
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

            data = mActivity.mRFIDWithUHFUSB.readData(pwdStr,
                    filterBank,
                    filterPtr,
                    filterCnt,
                    filterData,
                    bank,
                    Integer.parseInt(ptrStr),
                    Integer.parseInt(cntStr)
            );
        } else {
            data = mActivity.mRFIDWithUHFUSB.readData(pwdStr,
                    bank,
                    Integer.parseInt(ptrStr),
                    Integer.parseInt(cntStr));
        }

        EtData_Read.setText(data);

        if (!TextUtils.isEmpty(data)) {
            showToast("Read data successfully");
            mActivity.playSound(1);
        } else {
            showToast("Read data failure");
            mActivity.playSound(2);
        }
    }
}
