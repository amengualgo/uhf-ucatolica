package com.example.demo_uhf_usb.widget;

import android.content.Context;
import android.content.res.TypedArray;
import android.graphics.Color;
import android.graphics.drawable.GradientDrawable;
import android.graphics.drawable.StateListDrawable;

import android.util.AttributeSet;
import android.util.TypedValue;

import androidx.appcompat.widget.AppCompatEditText;

import com.example.demo_uhf_usb.R;

/**
 * Created by WuShengjun on 2018-10-24.
 * Description: 通用EditText输入框
 */

public class CommonEditText extends AppCompatEditText {

    private int TOP_LEFT = 1;
    private int TOP_RIGHT = 2;
    private int BOTTOM_RIGHT = 4;
    private int BOTTOM_LEFT = 8;

    /**
     * 填充颜色
     */
    private int mNormalFillColor = Color.TRANSPARENT;

    /**
     * 焦点填充颜色
     */
    private int mFocusedFillColor = Color.TRANSPARENT;

    /**
     * 不可编辑填充颜色
     */
    private int mNotEnabledFillColor = Color.TRANSPARENT;

    /**
     * 描边颜色
     */
    private int mStrokeColor = 0xbbbbbb;

    /**
     * 描边焦点状态颜色
     */
    private int mStrokeFocusedColor = 0x000000;

    /**
     * 描边不可编辑状态颜色
     */
    private int mStrokeNotEnabledColor = 0xe0e0e0;

    /**
     * 描边宽度
     */
    private int mStrokeWidth = 1;

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
    /**
     * 焦点shape样式
     */
    private GradientDrawable focusGradientDrawable = new GradientDrawable();
    /**
     * 不可编辑shape样式
     */
    private GradientDrawable notEnabledGradientDrawable = new GradientDrawable();
    /**
     * shape样式集合
     */
    private StateListDrawable stateListDrawable = new StateListDrawable();

    public CommonEditText(Context context) {
        this(context, null);
    }

    public CommonEditText(Context context, AttributeSet attrs) {
        this(context, attrs, android.R.attr.editTextStyle);
    }

    public CommonEditText(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        inits(context);
        initAttrs(context, attrs);
        initBackground();
    }

    private void inits(Context context) {
        mNormalFillColor = Color.TRANSPARENT;
        mFocusedFillColor = Color.TRANSPARENT;
        mNotEnabledFillColor = Color.TRANSPARENT;
        mStrokeColor = Color.parseColor("#bbbbbb");
        mStrokeFocusedColor = Color.parseColor("#bbbbbb");
        mStrokeNotEnabledColor = Color.parseColor("#e0e0e0");
        mStrokeWidth = dp2px(1);
        mCornerRadius = dp2px(4);
        mCornerPosition = -1;
        mStartColor = 0xFFFFFFFF;
        mEndColor = 0xFFFFFFFF;
        setTextColor(Color.parseColor("#393939"));
        setHintTextColor(Color.parseColor("#aaaaaa"));
    }

    private void initAttrs(Context context, AttributeSet attrs) {
        TypedArray typedArray = context.obtainStyledAttributes(attrs, R.styleable.CommonEditText);
        mNormalFillColor = typedArray.getColor(R.styleable.CommonEditText_cet_normalFillColor, mNormalFillColor);
        mFocusedFillColor = typedArray.getColor(R.styleable.CommonEditText_cet_focusedFillColor, mFocusedFillColor);
        mNotEnabledFillColor = typedArray.getColor(R.styleable.CommonEditText_cet_notEnabledFillColor, mNotEnabledFillColor);
        mStrokeColor = typedArray.getColor(R.styleable.CommonEditText_cet_strokeColor, mStrokeColor);
        mStrokeFocusedColor = typedArray.getColor(R.styleable.CommonEditText_cet_strokeFocusedColor, mStrokeFocusedColor);
        mStrokeNotEnabledColor = typedArray.getColor(R.styleable.CommonEditText_cet_strokeNotEnabledColor, mStrokeNotEnabledColor);
        mStrokeWidth = typedArray.getDimensionPixelSize(R.styleable.CommonEditText_cet_strokeWidth, mStrokeWidth);
        mCornerRadius = typedArray.getDimensionPixelSize(R.styleable.CommonEditText_cet_cornerRadius, mCornerRadius);
        mCornerPosition = typedArray.getInt(R.styleable.CommonEditText_cet_cornerPosition, mCornerPosition);
        mStartColor = typedArray.getColor(R.styleable.CommonEditText_cet_startColor, mStartColor);
        mEndColor = typedArray.getColor(R.styleable.CommonEditText_cet_endColor, mEndColor);
        mOrientation = typedArray.getColor(R.styleable.CommonEditText_cet_orientation, mOrientation);
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
            normalGradientDrawable.setColor(mNormalFillColor);
        }

        // 初始化颜色及状态
        notEnabledGradientDrawable.setColor(mNotEnabledFillColor);
        focusGradientDrawable.setColor(mFocusedFillColor);
        normalGradientDrawable.setShape(GradientDrawable.RECTANGLE);
        focusGradientDrawable.setShape(GradientDrawable.RECTANGLE);
        notEnabledGradientDrawable.setShape(GradientDrawable.RECTANGLE);
        // 统一设置圆角半径
        if (mCornerPosition == -1) {
            float radius = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_PX, mCornerRadius, getResources().getDisplayMetrics());
            normalGradientDrawable.setCornerRadius(radius);
            focusGradientDrawable.setCornerRadius(radius);
            notEnabledGradientDrawable.setCornerRadius(radius);
        } else { // 根据圆角位置设置圆角半径
            float[] radii = getCornerRadiusByPosition();
            normalGradientDrawable.setCornerRadii(radii);
            focusGradientDrawable.setCornerRadii(radii);
            notEnabledGradientDrawable.setCornerRadii(radii);
        }
        // 默认的透明边框不绘制,否则会导致没有阴影
        if (mStrokeColor != 0) {
            normalGradientDrawable.setStroke(mStrokeWidth, mStrokeColor);
            focusGradientDrawable.setStroke(mStrokeWidth, mStrokeFocusedColor);
            notEnabledGradientDrawable.setStroke(mStrokeWidth, mStrokeNotEnabledColor);
        }

        // 注意此处的add顺序，normal必须在最后一个，否则其他状态无效
        // 设置focused状态
        stateListDrawable.addState(new int[]{android.R.attr.state_focused}, focusGradientDrawable);
//        // 设置enabled状态
//        stateListDrawable.addState(new int[]{android.R.attr.state_enabled}, focusGradientDrawable);
        // 设置normal状态
        stateListDrawable.addState(new int[]{}, normalGradientDrawable);
//        setBackground(normalGradientDrawable);
        setBackground(stateListDrawable);
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
