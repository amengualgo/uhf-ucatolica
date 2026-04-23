package com.example.demo_uhf_usb.adapter;


import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentPagerAdapter;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by WuShengjun on 2017/9/16.
 */

public class CommonFragmentPagerAdapter<F extends Fragment> extends FragmentPagerAdapter {
    protected List<F> mFragments;
    protected List<String> mTitles;

    public CommonFragmentPagerAdapter(FragmentManager fm, List<F> mFragments) {
        this(fm, mFragments, new ArrayList<String>());
    }

    public CommonFragmentPagerAdapter(FragmentManager fm, List<F> mFragments, List<String> mTitles) {
        super(fm);
        this.mFragments = mFragments;
        this.mTitles = mTitles;
    }

    public CommonFragmentPagerAdapter(FragmentManager fm, List<F> mFragments, String[] mTitles) {
        this(fm, mFragments, Arrays.asList(mTitles));
    }

    @Override
    public Fragment getItem(int i) {
        return mFragments.get(i);
    }

    @Override
    public int getCount() {
        return mFragments.size();
    }

    @Override
    public CharSequence getPageTitle(int position) {
        if(mTitles == null || mTitles.isEmpty()) {
            return super.getPageTitle(position);
        }
        return mTitles.get(position);
    }
}
