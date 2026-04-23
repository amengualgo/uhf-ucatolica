package com.example.demo_uhf_usb;

import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.IdRes;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import androidx.viewpager.widget.ViewPager;

import com.example.demo_uhf_usb.adapter.CommonFragmentPagerAdapter;
import com.google.android.material.tabs.TabLayout;

import java.util.ArrayList;
import java.util.List;

public abstract class BaseActivity extends AppCompatActivity {

    private String TAG = "CW_BaseActivity";
    private FragmentTransaction ft;
    private List<Fragment> mFragments = new ArrayList<>();
    public int currShowFragmentIndex=0;
    public Fragment currFragment;

    private TabLayout tabLayout;
    private ViewPager viewPager;
    private List<String> mTitles = new ArrayList<>();

    private CommonFragmentPagerAdapter<Fragment> pagerAdapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        onCreating(savedInstanceState);
        initCommonViews();
        initFragments(R.id.fragmentContainerId, mFragments, currShowFragmentIndex);
        initTabTitles(mFragments, mTitles);
    }

    private void initCommonViews() {
        tabLayout = findViewById(R.id.tabLayout);
        tabLayout.setTabMode(TabLayout.MODE_SCROLLABLE);
        viewPager = findViewById(R.id.viewPager);

    }

    protected void initFragments(@IdRes int fragmentContainerId, List<Fragment> fragments, int defShowFragmentIndex) {
        if(fragments == null || fragments.isEmpty())
            return;
        mFragments = fragments;
        currShowFragmentIndex = defShowFragmentIndex;

        Log.e(TAG,"initFragments  defShowFragmentIndex="+defShowFragmentIndex);

        ft = getSupportFragmentManager().beginTransaction();
        for(int i=0; i<mFragments.size(); i++) {
            ft.add(fragmentContainerId, mFragments.get(i), String.valueOf(i));
            ft.hide(mFragments.get(i));
        }
        currFragment = mFragments.get(currShowFragmentIndex);
        ft.show(currFragment); // 默认选中第1个
        ft.commit(); // 提交
    }

    protected void initTabTitles(List<Fragment> fragments, List<String> titles) {
        if(tabLayout == null || viewPager == null) {
            throw new IllegalStateException("Layout resources must have TabLayout as its id tagLayout and ViewPager as its id viewPager!");
        } else if(mFragments == null || mFragments.isEmpty() || titles == null || titles.isEmpty()) {
            throw new IllegalStateException("Fragment can not be null or tab title can not be empty!");
        }
        mFragments = fragments;
        mTitles = titles;
        if(pagerAdapter == null) {
            pagerAdapter = new CommonFragmentPagerAdapter<>(getSupportFragmentManager(), mFragments, mTitles);
            viewPager.setAdapter(pagerAdapter);
            tabLayout.setupWithViewPager(viewPager);

        }
    }

    protected abstract void onCreating(Bundle savedInstanceState);

    private Toast mToast;
    public void showToast(String text) {
        if(mToast != null) {
            mToast.cancel();
        }
        mToast = Toast.makeText(this, text, Toast.LENGTH_SHORT);
        mToast.show();
    }

    public void showToast(int resId) {
        showToast(getString(resId));
    }
}
