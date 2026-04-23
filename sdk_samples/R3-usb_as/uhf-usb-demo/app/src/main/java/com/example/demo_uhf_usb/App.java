package com.example.demo_uhf_usb;

import android.app.Application;
import android.content.SharedPreferences;

/**
 * Created by WuShengjun on 2019/6/28.
 * Description:
 */

public class App extends Application {

    private static App mInstance;
    public SharedPreferences spf;

    @Override
    public void onCreate() {
        super.onCreate();
        mInstance = this;
        init();
    }

    private void init() {
        spf = getSharedPreferences("App_config", MODE_PRIVATE);
    }

    public static App getInstance() {
        return mInstance;
    }
}
