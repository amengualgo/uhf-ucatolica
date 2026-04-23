package com.example.demo_uhf_usb;


import android.widget.Toast;

import androidx.fragment.app.Fragment;

public class BaseFragment extends Fragment {


    private Toast mToast;
    public void showToast(String text) {
        if(mToast != null) {
            mToast.cancel();
        }
        mToast = Toast.makeText(getContext(), text, Toast.LENGTH_SHORT);
        mToast.show();
    }

    public void showToast(int resId) {
        showToast(getString(resId));
    }
}
