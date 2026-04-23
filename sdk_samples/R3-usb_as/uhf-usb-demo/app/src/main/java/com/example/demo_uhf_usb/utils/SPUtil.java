package com.example.demo_uhf_usb.utils;

import android.content.SharedPreferences;

import com.example.demo_uhf_usb.App;

import java.util.Set;

/**
 * Created by WuShengjun on 2019/6/28.
 * Description:
 */

public class SPUtil {
    public static boolean saveShared(String key, String value) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putString(key, value).commit();
    }

    public static String getShared(String key, String defValue) {
        SharedPreferences spf = getShared();
        return spf.getString(key, defValue);
    }

    public static boolean saveShared(String key, int value) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putInt(key, value).commit();
    }

    public static int getShared(String key, int defValue) {
        SharedPreferences spf = getShared();
        return spf.getInt(key, defValue);
    }

    public static boolean saveShared(String key, long value) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putLong(key, value).commit();
    }

    public static long getShared(String key, long defValue) {
        SharedPreferences spf = getShared();
        return spf.getLong(key, defValue);
    }

    public static boolean saveShared(String key, float value) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putFloat(key, value).commit();
    }

    public static float getShared(String key, float defValue) {
        SharedPreferences spf = getShared();
        return spf.getFloat(key, defValue);
    }

    public static boolean saveShared(String key, boolean value) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putBoolean(key, value).commit();
    }

    public static boolean getShared(String key, boolean defValue) {
        SharedPreferences spf = getShared();
        return spf.getBoolean(key, defValue);
    }

    public static boolean saveShared(String key, Set<String> set) {
        SharedPreferences.Editor editor = getEditor();
        return editor.putStringSet(key, set).commit();
    }

    public static Set<String> getShared(String key, Set<String> defValue) {
        SharedPreferences spf = getShared();
        return spf.getStringSet(key, defValue);
    }

    private static SharedPreferences.Editor getEditor() {
        return getShared().edit();
    }

    private static SharedPreferences getShared() {
        App app = App.getInstance();
        return app.spf;
    }
}
