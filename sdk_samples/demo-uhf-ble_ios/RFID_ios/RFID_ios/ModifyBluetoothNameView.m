//
//  ModifyBluetoothNameView.m
//  RFID_ios
//
//  Created by NLAB on 2020/7/12.
//  Copyright © 2020  . All rights reserved.
//

#import "ModifyBluetoothNameView.h"
#import <Masonry.h>

@interface ModifyBluetoothNameView()

/** 修改名称label */
@property (nonatomic, strong) UILabel *modifyLabel;
/** 修改名称输入框 */
@property (nonatomic, strong) UITextField *modifyTextField;
/** save btn */
@property (nonatomic, strong) UIButton *modifyBtn;

@end

@implementation ModifyBluetoothNameView
#pragma mark - lazyLoading
#pragma mark -
-(UILabel *)modifyLabel {
    if (!_modifyLabel) {
        _modifyLabel = [[UILabel alloc]init];
        _modifyLabel.text = @"new name";
        _modifyLabel.textColor = [UIColor blackColor];
        _modifyLabel.font = [UIFont systemFontOfSize:15];
        _modifyLabel.textAlignment = NSTextAlignmentLeft;
    }
    return _modifyLabel;
}

-(UITextField *)modifyTextField {
    if (!_modifyTextField) {
        _modifyTextField = [[UITextField alloc]init];
        _modifyTextField.placeholder = @"please input new name";
        _modifyTextField.textColor = [UIColor blackColor];
        _modifyTextField.borderStyle = UITextBorderStyleLine;
        _modifyTextField.font = [UIFont systemFontOfSize:16];
        _modifyTextField.textAlignment = NSTextAlignmentLeft;
    }
    return _modifyTextField;
}

-(UIButton *)modifyBtn {
    if (!_modifyBtn) {
        _modifyBtn = [[UIButton alloc]init];
        _modifyBtn.backgroundColor = [UIColor lightGrayColor];
        [_modifyBtn setTitle:NSLocalizedString(@"Modify Bluetooth Name", nil) forState:UIControlStateNormal];
        [_modifyBtn setTitle:NSLocalizedString(@"Modify Bluetooth Name", nil) forState:UIControlStateHighlighted];
        _modifyBtn.titleLabel.font = [UIFont systemFontOfSize:18];
        [_modifyBtn addTarget:self action:@selector(modifyNameBtnClick:) forControlEvents:UIControlEventTouchUpInside];
        _modifyBtn.layer.cornerRadius = 10;
    }
    return _modifyBtn;
}

#pragma mark - lifeCycle
-(instancetype)initWithFrame:(CGRect)frame {
    if (self = [super initWithFrame:frame]) {
        [self buildSubViews];
    }
    return self;
}

#pragma mark - subViews
#pragma mark - buildSubViews
-(void) buildSubViews {
    [self addSubview:self.modifyLabel];
    [self.modifyLabel mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.mas_offset(10);
        make.right.mas_offset(-10);
        make.top.mas_offset(40);
        make.height.mas_equalTo(20);
    }];
    
    [self addSubview:self.modifyTextField];
    [self.modifyTextField mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.mas_offset(10);
        make.right.mas_offset(-10);
        make.top.mas_equalTo(self.modifyLabel.mas_bottom).mas_offset(10);
        make.height.mas_equalTo(50);
    }];
    
    [self addSubview:self.modifyBtn];
    [self.modifyBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.mas_offset(10);
        make.right.mas_offset(-10);
        make.top.mas_equalTo(self.modifyTextField.mas_bottom).mas_offset(40);
        make.height.mas_equalTo(50);
    }];
}

#pragma mark - action
#pragma mark - ClickBtnAction
-(void) modifyNameBtnClick:(UIButton *)btn {
    NSString *neName = self.modifyTextField.text;
    if (self.clickSaveBtnBlock) {
        self.clickSaveBtnBlock(btn, neName);
    }
}

@end
