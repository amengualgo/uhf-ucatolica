package com.example.demo_uhf_usb.utils;

import android.content.Context;
import android.media.AudioManager;
import android.media.SoundPool;

import com.example.demo_uhf_usb.R;

import java.util.HashMap;

/**
 * Created by Administrator on 2018-12-26.
 */

public class Sound {

    private HashMap<Integer, Integer> soundMap = new HashMap<Integer, Integer>();
    private SoundPool soundPool;
    private float volumnRatio;
    private AudioManager am;

    public Sound(Context context) {
        initSound(context.getApplicationContext());
    }

    public void initSound(Context context) {
        soundPool = new SoundPool(10, AudioManager.STREAM_MUSIC, 5);
        soundMap.put(1, soundPool.load(context, R.raw.barcodebeep, 1));
        soundMap.put(2, soundPool.load(context, R.raw.serror, 1));
        am = (AudioManager) context.getSystemService(Context.AUDIO_SERVICE);// 实例化AudioManager对象
    }

    /**
     * 释放声音池资源
     */
    public void freeSound() {
        if (soundPool != null)
            soundPool.release();
    }

    /**
     * 播放提示音
     *
     * @param id 成功1，失败2
     */
    public void playSound(int id) {
        float audioMaxVolumn = am.getStreamMaxVolume(AudioManager.STREAM_MUSIC); // 返回当前AudioManager对象的最大音量值
        float audioCurrentVolumn = am.getStreamVolume(AudioManager.STREAM_MUSIC);// 返回当前AudioManager对象的音量值
        volumnRatio = audioCurrentVolumn / audioMaxVolumn;
        try {
            soundPool.play(soundMap.get(id), volumnRatio, // 左声道音量
                    volumnRatio, // 右声道音量
                    1, // 优先级，0为最低
                    0, // 循环次数，0无不循环，-1无永远循环
                    1 // 回放速度 ，该值在0.5-2.0之间，1为正常速度
            );
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
