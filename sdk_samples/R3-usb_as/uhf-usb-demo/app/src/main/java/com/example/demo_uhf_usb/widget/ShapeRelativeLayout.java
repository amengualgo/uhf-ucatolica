package com.example.demo_uhf_usb.widget;

import android.content.Context;
import android.content.res.TypedArray;
import android.graphics.Color;
import android.graphics.drawable.GradientDrawable;
import android.util.AttributeSet;
import android.util.TypedValue;
import android.widget.RelativeLayout;

import com.example.demo_uhf_usb.R;

/**
 * Created by WuShengjun on 2018-10-24.
 * Description: 矩形或圆角背景LinearLayout
 */

public class ShapeRelativeLayout extends RelativeLayout {

    private int TOP_LEFT = 1;
    private int TOP_RIGHT = 2;
    private int BOTTOM_RIGHT = 4;
    private int BOTTOM_LEFT = 8;

    /**
     * 填充颜色
     */
    private int mFillColor = Color.TRANSPARENT;

    /**
     * 描边颜色
     */
    private int mStrokeColor = 0xbbbbbb;

    /**
     * 描边宽度
     */
    private int mStrokeWidth = 0;

    /**
     * 圆角半径
     */
    private int mCornerRadius = 0;
    /**
     * 圆角位置
     */
    private int mCornerPosition = -1;

    /**
     * 起始颜色
     */
    private int mStartColor = 0xFFFFFFFF;

    /**
     * 结束颜色
     */
    private int mEndColor = 0xFFFFFFFF;

    /**
     * 渐变方向
     * 0-GradientDrawable.Orientation.TOP_BOTTOM
     * 1-GradientDrawable.Orientation.LEFT_RIGHT
     */
    private int mOrientation = 0;

    /**
     * 普通shape样式
     */
    private GradientDrawable normalGradientDrawable = new GradientDrawable();

    public ShapeRelativeLayout(Context context) {
        this(context, null);
    }

    public ShapeRelativeLayout(Context context, AttributeSet attrs) {
        this(context, attrs, 0);
    }

    public ShapeRelativeLayout(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        inits(context);
        initAttrs(context, attrs);
        initBackground();
    }

    private void inits(Context context) {
        mStrokeColor = Color.TRANSPARENT;
        mStrokeWidth = 0;
        mCornerRadius = 0;
        mCornerPosition = -1;
        mStartColor = 0xFFFFFFFF;
        mEndColor = 0xFFFFFFFF;
    }

    private void initAttrs(Context context, AttributeSet attrs) {
        TypedArray typedArray = context.obtainStyledAttributes(attrs, R.styleable.ShapeRelativeLayout);
        mFillColor = typedArray.getColor(R.styleable.ShapeRelativeLayout_srl_fillColor, mFillColor);
        mStrokeColor = typedArray.getColor(R.styleable.ShapeRelativeLayout_srl_strokeColor, mStrokeColor);
        mStrokeWidth = typedArray.getDimensionPixelSize(R.styleable.ShapeRelativeLayout_srl_strokeWidth, mStrokeWidth);
        mCornerRadius = typedArray.getDimensionPixelSize(R.styleable.ShapeRelativeLayout_srl_cornerRadius, mCornerRadius);
        mCornerPosition = typedArray.getInt(R.styleable.ShapeRelativeLayout_srl_cornerPosition, mCornerPosition);
        mStartColor = typedArray.getColor(R.styleable.ShapeRelativeLayout_srl_startColor, mStartColor);
        mEndColor = typedArray.getColor(R.styleable.ShapeRelativeLayout_srl_endColor, mEndColor);
        mOrientation = typedArray.getColor(R.styleable.ShapeRelativeLayout_srl_orientation, mOrientation);
        typedArray.recycle();
    }

    private void initBackground() {
        // 渐变色
        if (mStartColor != 0xFFFFFFFF && mEndColor != 0xFFFFFFFF) {
            normalGradientDrawable.setColors(new int[]{mStartColor, mEndColor});
            if (mOrientation == 0) {
                normalGradientDrawable.setOrientation(GradientDrawable.Orientation.TOP_BOTTOM);
            } else if (mOrientation == 1) {
                normalGradientDrawable.setOrientation(GradientDrawable.Orientation.LEFT_RIGHT);
            }
        } else { // 初始化normal状态填充色
            normalGradientDrawable.setColor(mFillColor);
        }

        // 初始化颜色及状态
        normalGradientDrawable.setShape(GradientDrawable.RECTANGLE);
        // 统一设置圆角半径
        if (mCornerPosition == -1) {
            float radius = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_PX, mCornerRadius, getResources().getDisplayMetrics());
            normalGradientDrawable.setCornerRadius(radius);
        } else { // 根据圆角位置设置圆角半径
            float[] radii = getCornerRadiusByPosition();
            normalGradientDrawable.setCornerRadii(radii);
        }
        // 默认的透明边框不绘制,否则会导致没有阴影
        if (mStrokeColor != 0) {
            normalGradientDrawable.setStroke(mStrokeWidth, mStrokeColor);
        }
        if(mStrokeWidth > 0 || mCornerRadius > 0 || mFillColor != Color.TRANSPARENT) {
            setBackground(normalGradientDrawable);
        }
    }

    /**
     * 根据圆角位置获取圆角半径
     */
    private float[] getCornerRadiusByPosition() {
        float[] result = new float[]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        if (containsFlag(mCornerPosition, TOP_LEFT)) {
            result[0] = mCornerRadius;
            result[1] = mCornerRadius;
        }
        if (containsFlag(mCornerPosition, TOP_RIGHT)) {
            result[2] = mCornerRadius;
            result[3] = mCornerRadius;
        }
        if (containsFlag(mCornerPosition, BOTTOM_RIGHT)) {
            result[4] = mCornerRadius;
            result[5] = mCornerRadius;
        }
        if (containsFlag(mCornerPosition, BOTTOM_LEFT)) {
            result[6] = mCornerRadius;
            result[7] = mCornerRadius;
        }
        return result;
    }

    /**
     * 是否包含对应flag
     * 按位或
     */
    private boolean containsFlag(int flagSet, int flag) {
        return (flagSet | flag) == flagSet;
    }

    /**
     * dp转px
     * @param dpVal
     * @return
     */
    private int dp2px(float dpVal) {
        float scale = getResources().getDisplayMetrics().density;
        return (int) (scale * dpVal + 0.5f);
    }
}
