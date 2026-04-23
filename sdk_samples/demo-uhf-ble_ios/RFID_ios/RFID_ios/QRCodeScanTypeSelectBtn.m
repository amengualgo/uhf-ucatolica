//
//  QRCodeScanTypeSelectBtn.m
//  RFID_ios
//
//  Created by   on 2020/10/11.
//  Copyright © 2020  . All rights reserved.
//

#import "QRCodeScanTypeSelectBtn.h"
#import <Masonry.h>

#define typeBtnH  50

@interface QRCodeScanTypeSelectBtn()

/** bgView */
@property (nonatomic, strong) UIView *bgView;
/** defaultBtn */
@property (nonatomic, strong) UIButton *defaultBtn;
/** utf8Btn */
@property (nonatomic, strong) UIButton *utf8Btn;
/** gb2312Btn */
@property (nonatomic, strong) UIButton *gb2312Btn;

@end

@implementation QRCodeScanTypeSelectBtn

#pragma mark -  lazyLoading
-(UIView *)bgView {
    if (!_bgView) {
        _bgView = [[UIView alloc]init];
        _bgView.backgroundColor = [UIColor whiteColor];
    }
    return _bgView;
}

-(UIButton *)defaultBtn {
    if (!_defaultBtn) {
        _defaultBtn = [[UIButton alloc]init];
        [_defaultBtn setTitle:@"  default" forState:UIControlStateNormal];
        [_defaultBtn setTitle:@"  default" forState:UIControlStateHighlighted];
        [_defaultBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_defaultBtn setTitleColor:[UIColor blackColor] forState:UIControlStateHighlighted];
        _defaultBtn.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        [_defaultBtn addTarget:self action:@selector(clickDefaultBtnAction:) forControlEvents:UIControlEventTouchUpInside];
    }
    return _defaultBtn;
}

-(UIButton *)utf8Btn {
    if (!_utf8Btn) {
        _utf8Btn = [[UIButton alloc]init];
        [_utf8Btn setTitle:@"  utf-8" forState:UIControlStateNormal];
        [_utf8Btn setTitle:@"  utf-8" forState:UIControlStateHighlighted];
        [_utf8Btn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_utf8Btn setTitleColor:[UIColor blackColor] forState:UIControlStateHighlighted];
        _utf8Btn.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        [_utf8Btn addTarget:self action:@selector(clickUtf8BtnAction:) forControlEvents:UIControlEventTouchUpInside];
    }
    return _utf8Btn;
}

-(UIButton *)gb2312Btn {
    if (!_gb2312Btn) {
        _gb2312Btn = [[UIButton alloc]init];
        [_gb2312Btn setTitle:@"  gb2312" forState:UIControlStateNormal];
        [_gb2312Btn setTitle:@"  gb2312" forState:UIControlStateHighlighted];
        [_gb2312Btn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_gb2312Btn setTitleColor:[UIColor blackColor] forState:UIControlStateHighlighted];
        _gb2312Btn.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        [_gb2312Btn addTarget:self action:@selector(clickGb2312BtnAction:) forControlEvents:UIControlEventTouchUpInside];
    }
    return _gb2312Btn;
}

#pragma mark - lifeCycle
-(instancetype)init {
    if (self = [super init]) {
        self.backgroundColor = [[UIColor blackColor]colorWithAlphaComponent:.15];
        self.frame = [UIScreen mainScreen].bounds;
        [self buildSubViews];
    }
    return self;
}

#pragma mark - buildSubViews
-(void)buildSubViews {
    [self addSubview:self.bgView];
    [self.bgView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.mas_offset(40);
        make.right.mas_offset(-40);
        make.bottom.mas_offset(-110);
        make.height.mas_equalTo(typeBtnH * 3);
    }];
    
    [self.bgView addSubview:self.defaultBtn];
    [self.defaultBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.top.mas_equalTo(self.bgView);
        make.height.mas_equalTo(typeBtnH);
    }];
    
    [self.bgView addSubview:self.utf8Btn];
    [self.utf8Btn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.mas_equalTo(self.bgView);
        make.top.mas_equalTo(self.defaultBtn.mas_bottom);
        make.height.mas_equalTo(typeBtnH);
    }];
    
    [self.bgView addSubview:self.gb2312Btn];
    [self.gb2312Btn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.mas_equalTo(self.bgView);
        make.top.mas_equalTo(self.utf8Btn.mas_bottom);
        make.height.mas_equalTo(typeBtnH);
    }];
}

-(void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event {
    UIView *touchView = touches.anyObject.view;
    if ([touchView isEqual:self]) {
        [self removeFromSuperview];
    }
}

#pragma mark - action
#pragma mark - clickBtnAction
-(void) clickDefaultBtnAction:(UIButton *)btn {
    [self removeFromSuperview];
    if (self.clickDefaultBtnBlock) {
        self.clickDefaultBtnBlock(btn);
    }
}

-(void) clickUtf8BtnAction:(UIButton *)btn {
    [self removeFromSuperview];
    if (self.clickUtf8BtnBlock) {
        self.clickUtf8BtnBlock(btn);
    }
}

-(void) clickGb2312BtnAction:(UIButton *)btn {
    [self removeFromSuperview];
    if (self.clickGb2312BtnBlock) {
        self.clickGb2312BtnBlock(btn);
    }
}

@end
