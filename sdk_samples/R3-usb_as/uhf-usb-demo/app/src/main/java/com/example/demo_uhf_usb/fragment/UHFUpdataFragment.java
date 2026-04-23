package com.example.demo_uhf_usb.fragment;

import android.app.ProgressDialog;
import android.bluetooth.BluetoothDevice;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.database.Cursor;
import android.graphics.Color;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;

import com.example.demo_uhf_usb.BaseFragment;

import com.example.demo_uhf_usb.MainActivity;
import com.example.demo_uhf_usb.R;
import com.example.demo_uhf_usb.filebrowser.FileManagerActivity;
import com.rscja.deviceapi.RFIDWithUHFBLE;
import com.rscja.deviceapi.interfaces.ConnectionStatus;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.util.Arrays;
import java.util.HashMap;


public class UHFUpdataFragment extends BaseFragment implements View.OnClickListener {

    MainActivity mContext;
    TextView tvPath, tvMsg;
    Button btSelect;
    Button btnUpdata, btnReadVere;
    String TAG = "CW_UHFUpdata";
    RadioButton rbSTM32, rbR2000;
    String version;

    private ProgressDialog progressDialog = null;
    private String mFilePath;
    private Uri mFileStreamUri;

    private HashMap<String, String> beforeVerMap;
    private HashMap<String, String> latestVerMap;

    private static final int SELECT_FILE_REQ = 11;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        Log.e(TAG, "onCreateView");
        return inflater.inflate(R.layout.fragment_uhfupdata, container, false);
    }

    @Override
    public void onActivityCreated(Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        mContext = (MainActivity) getActivity();

        tvPath = (TextView) mContext.findViewById(R.id.tvPath);
        tvMsg = (TextView) mContext.findViewById(R.id.tvMsg);
        btSelect = (Button) mContext.findViewById(R.id.btSelect);
        btnUpdata = (Button) mContext.findViewById(R.id.btnUpdata);
        btnReadVere = (Button) mContext.findViewById(R.id.btnReadVere);

        rbSTM32 = (RadioButton) mContext.findViewById(R.id.rbSTM32);
        rbR2000 = (RadioButton) mContext.findViewById(R.id.rbR2000);
        rbSTM32.setVisibility(View.GONE);
        rbR2000.setVisibility(View.GONE);

        btSelect.setOnClickListener(this);
        btnUpdata.setOnClickListener(this);
        btnReadVere.setOnClickListener(this);

        init();
    }


    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.btSelect:
                Intent intent = new Intent(mContext, FileManagerActivity.class);
                startActivity(intent);
                break;
            case R.id.btnUpdata:
                update();
                break;
            case R.id.btnReadVere:
                String v = mContext.mRFIDWithUHFUSB.getSTM32Version();//获取版本号
                tvMsg.setText("version:" + v);
                break;
        }
    }

    @Override
    public void onDestroyView() {
        mContext.unregisterReceiver(pathReceiver);
        super.onDestroyView();
    }

    public void update() {
        if (!(mContext.mRFIDWithUHFUSB.getConnectStatus() == ConnectionStatus.CONNECTED)) {
            Toast.makeText(mContext, "not connected!", Toast.LENGTH_SHORT).show();
            return;
        }
        String filePath = tvPath.getText().toString();
        if (TextUtils.isEmpty(filePath)) {
            Toast.makeText(mContext, R.string.up_msg_sel_file, Toast.LENGTH_SHORT).show();
            return;
        }
        if (filePath.toLowerCase().lastIndexOf(".bin") < 0) {
            Toast.makeText(mContext, "文件格式错误", Toast.LENGTH_SHORT).show();
            return;
        }

        char flag = 0;
        File file = new File(filePath);
        if(file.length() > 200 * 1024) {
            flag=mContext.mRFIDWithUHFUSB.UPDATE_UHF;
            version = mContext.mRFIDWithUHFUSB.getVersion();//获取版本号
        } else{
            flag=mContext.mRFIDWithUHFUSB.UPDATE_STM32;
            version = mContext.mRFIDWithUHFUSB.getSTM32Version();//获取版本号
        }
        tvMsg.setText("version:" + version);
        new UpdateTask(filePath, flag).execute();
    }

    class UpdateTask extends AsyncTask<String, Integer, Boolean> {
        String path = "";
        char flag;
        public UpdateTask(String path, char flag) {
            this.path = path;
            this.flag = flag;
        }
        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            String msg = "";
            if(flag==mContext.mRFIDWithUHFUSB.UPDATE_UHF) {
                msg = "准备升级射频模块...";
            } else   {
                msg = "准备升级主板固件...";
            }
            progressDialog = new ProgressDialog(mContext);
            progressDialog.setMessage(msg);
            progressDialog.setCancelable(false);
            progressDialog.setCanceledOnTouchOutside(false);
            progressDialog.show();
        }

        @Override
        protected Boolean doInBackground(String... strings) {
            boolean result = false;
            File uFile = new File(path);
            if (!uFile.exists()) {
                return false;
            }
            long uFileSize = uFile.length();
            int packageCount = (int) (uFileSize / 64);
            RandomAccessFile raf = null;
            try {
                raf = new RandomAccessFile(path, "r");
            } catch (FileNotFoundException e) {
            }
            if (raf == null) {
                return false;
            }

            if(flag==mContext.mRFIDWithUHFUSB.UPDATE_UHF) {
                Log.d(TAG, "uhfJump2Boot");
                if (!mContext.mRFIDWithUHFUSB.uhfJump2Boot()) {
                    Log.d(TAG, "uhfJump2Boot 失败");
                    return false;
                }
            }else{
                Log.d(TAG, "uhfJump2BootSTM32");
                if (!mContext.mRFIDWithUHFUSB.uhfJump2BootSTM32()) {
                    Log.d(TAG, "uhfJump2BootSTM32 失败");
                    return false;
                }
            }

            sleep(2000);
            Log.d(TAG, "UHF uhfStartUpdate");
            if (!mContext.mRFIDWithUHFUSB.uhfStartUpdate()) {
                Log.d(TAG, "uhfStartUpdate 失败");
                return false;
            }
            int pakeSize = 64;
            byte[] currData = new byte[(int) uFileSize];
            for (int k = 0; k < packageCount; k++) {
                int index = k * pakeSize;
                try {
                    int rsize = raf.read(currData, index, pakeSize);
//                    Log.d(TAG, "总包数量="+uFileSize+"  beginPack=" +index + " endPack=" + (index+pakeSize-1) +" rsize="+rsize);
                } catch (IOException e) {
                    stop();
                    return false;
                }
                byte[] data = Arrays.copyOfRange(currData, index, index + pakeSize);
//                Log.d(TAG,"data="+ StringUtility.bytes2HexString(data,data.length));

                Log.d(TAG, "UHF uhfUpdating");
                if (mContext.mRFIDWithUHFUSB.uhfUpdating(data)) {
                    result = true;
                    publishProgress(index + pakeSize, (int) uFileSize);
                } else {
                    Log.d(TAG, "uhfUpdating 失败");
                    stop();
                    return false;
                }

            }
            if (uFileSize % pakeSize != 0) {
                int index = packageCount * pakeSize;
                int len = (int) (uFileSize % pakeSize);
                try {
                    int rsize = raf.read(currData, index, len);
                    Log.d(TAG, "beginPack=" + index + " countPack=" + len + " rsize=" + rsize);
                } catch (IOException e) {
                    stop();
                    return false;
                }
                if (mContext.mRFIDWithUHFUSB.uhfUpdating(Arrays.copyOfRange(currData, index, index + len))) {
                    result = true;
                    publishProgress((int) uFileSize, (int) uFileSize);
                } else {
                    Log.d(TAG, "uhfUpdating 失败");
                    stop();
                    return false;
                }
            }
            stop();
            return result;
        }

        @Override
        protected void onProgressUpdate(Integer... values) {
            super.onProgressUpdate(values);
            progressDialog.setMessage((values[0] * 100 / values[1]) + "% " + mContext.getString(R.string.app_msg_Upgrade));
            tvMsg.setText("version:" + version);
        }

        @Override
        protected void onPostExecute(Boolean result) {
            super.onPostExecute(result);
            if (!result) {
                Toast.makeText(mContext, R.string.uhf_msg_upgrade_fail, Toast.LENGTH_SHORT).show();
                tvMsg.setText(R.string.uhf_msg_upgrade_fail);
                tvMsg.setTextColor(Color.RED);
            } else {
                Toast.makeText(mContext, R.string.uhf_msg_upgrade_succ, Toast.LENGTH_SHORT).show();
                tvMsg.setText(R.string.uhf_msg_upgrade_succ);
                tvMsg.setTextColor(Color.GREEN);
            }
            tvMsg.setText(tvMsg.getText() + " version=" + (flag == mContext.mRFIDWithUHFUSB.UPDATE_UHF ? mContext.mRFIDWithUHFUSB.getVersion() : mContext.mRFIDWithUHFUSB.getSTM32Version()));
            progressDialog.dismiss();
        }

        private void stop() {
            Log.d(TAG, "UHF uhfStopUpdate");
            if (!mContext.mRFIDWithUHFUSB.uhfStopUpdate())
                Log.d(TAG, "uhfStopUpdate 失败");
            sleep(2000);
        }
    }

    private void sleep(int time) {
        try {
            Thread.sleep(time);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    //-----------------------------------------------------------------------------
    PathReceiver pathReceiver = new PathReceiver();

    public void init() {
        IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(FileManagerActivity.Path_ACTION);
        mContext.registerReceiver(pathReceiver, intentFilter);
    }


    public class PathReceiver extends BroadcastReceiver {
        @Override
        public void onReceive(Context context, Intent intent) {
            mFilePath = intent.getStringExtra(FileManagerActivity.Path_Key);
            mFileStreamUri = Uri.fromFile(new File(mFilePath));
            tvPath.setText(mFilePath);
        }
    }
/*
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        switch (requestCode) {
            case SELECT_FILE_REQ:
                if(data != null) {
                    final Uri uri = data.getData();
                    if (uri != null && uri.getScheme().equals("content")) {
                        mFileStreamUri = uri;
                        mFilePath = getRealPathFromURI(mContext, mFileStreamUri);
                        tvPath.setText(mFilePath);
                    }
                }
                break;
        }
    }

    public String getRealPathFromURI(Context context, Uri contentURI) {
        String result;
        Cursor cursor = context.getContentResolver().query(contentURI,
                new String[] { MediaStore.Images.ImageColumns.DATA },//
                null, null, null);
        if (cursor == null)
            result = contentURI.getPath();
        else {
            cursor.moveToFirst();
            int index = cursor.getColumnIndex(MediaStore.Images.ImageColumns.DATA);
            result = cursor.getString(index);
            cursor.close();
        }
        return result;
    }
    */
}
