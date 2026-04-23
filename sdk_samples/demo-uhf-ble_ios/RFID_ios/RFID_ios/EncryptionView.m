//
//  EncryptionView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "EncryptionView.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation EncryptionView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        _setmiyaoBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:_setmiyaoBtn];
        [_setmiyaoBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_setmiyaoBtn setTitle:@"设置密钥" forState:UIControlStateNormal];
        _setmiyaoBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_setmiyaoBtn addTarget:self action:@selector(setmiyaoBtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat width=([UIScreen mainScreen].bounds.size.width-30-40)/2.0;
        [_setmiyaoBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self).offset(10);
            make.height.mas_equalTo(AdaptH(40));
            make.width.mas_equalTo(width);
        }];
        
        _getmiyaoBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:_getmiyaoBtn];
        [_getmiyaoBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_getmiyaoBtn setTitle:@"获取密钥" forState:UIControlStateNormal];
        _getmiyaoBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_getmiyaoBtn addTarget:self action:@selector(getmiyaoBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_getmiyaoBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(_setmiyaoBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        UILabel *modelLab=[UILabel new];
        [self addSubview:modelLab];
        modelLab.text=@"mode";
        modelLab.font=[UIFont systemFontOfSize:15];
        modelLab.textColor=RGB(150, 150, 150, 1);
        [modelLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(_setmiyaoBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(50);
        }];
        self.modeBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.modeBtn];
        [self.modeBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.modeBtn setTitle:@"ECB" forState:UIControlStateNormal];
        self.modeBtn.titleLabel.font=[UIFont systemFontOfSize:18];
        self.modeBtn.titleLabel.textAlignment=NSTextAlignmentLeft;
        [self.modeBtn addTarget:self action:@selector(modeBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.modeBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(modelLab);
            make.height.mas_equalTo(AdaptH(40));
            make.left.equalTo(modelLab.mas_right);
            make.right.equalTo(self);
        }];
        
        UILabel *miyaoLab=[UILabel new];
        [self addSubview:miyaoLab];
        miyaoLab.text=@"密 钥";
        miyaoLab.font=[UIFont systemFontOfSize:15];
        miyaoLab.textColor=RGB(150, 150, 150, 1);
        [miyaoLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(modelLab.mas_bottom).offset(20);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(60);
        }];
        UIView *linView=[UIView new];
        [self addSubview:linView];
        linView.backgroundColor=RGB(150, 150, 150, 1);
        [linView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(miyaoLab.mas_bottom).offset(-3);
            make.left.equalTo(miyaoLab.mas_right);
            make.right.equalTo(self).offset(-5);
            make.height.mas_equalTo(1);
        }];
        self.textField1=[UITextField new];
        [self addSubview:self.textField1];
        self.textField1.font=[UIFont systemFontOfSize:16];
        [self.textField1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(miyaoLab.mas_right);
            make.bottom.equalTo(linView.mas_top);
            make.height.mas_equalTo(35);
            make.right.equalTo(linView);
        }];
        
        
        UILabel *chuLab=[UILabel new];
        [self addSubview:chuLab];
        chuLab.text=@"初始值";
        chuLab.font=[UIFont systemFontOfSize:15];
        chuLab.textColor=RGB(150, 150, 150, 1);
        [chuLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(miyaoLab);
            make.top.equalTo(miyaoLab.mas_bottom).offset(20);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(60);
        }];
        UIView *linView1=[UIView new];
        [self addSubview:linView1];
        linView1.backgroundColor=RGB(150, 150, 150, 1);
        [linView1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(chuLab.mas_bottom).offset(-3);
            make.left.equalTo(chuLab.mas_right);
            make.right.equalTo(self).offset(-5);
            make.height.mas_equalTo(1);
        }];
        self.textField2=[UITextField new];
        [self addSubview:self.textField2];
        self.textField2.font=[UIFont systemFontOfSize:16];
        [self.textField2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(chuLab.mas_right);
            make.bottom.equalTo(linView1.mas_top);
            make.height.mas_equalTo(35);
            make.right.equalTo(linView1);
        }];
        
        UIButton *encryBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:encryBtn];
        [encryBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [encryBtn setTitle:@"加密" forState:UIControlStateNormal];
        encryBtn.backgroundColor=RGB(210, 210, 210, 1);
        [encryBtn addTarget:self action:@selector(encryBtnn) forControlEvents:UIControlEventTouchUpInside];
        [encryBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(linView1.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(40));
            make.width.mas_equalTo(width);
        }];
        UIButton *decryptBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:decryptBtn];
        [decryptBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [decryptBtn setTitle:@"解密" forState:UIControlStateNormal];
        decryptBtn.backgroundColor=RGB(210, 210, 210, 1);
        [decryptBtn addTarget:self action:@selector(decryptBtnn) forControlEvents:UIControlEventTouchUpInside];
        [decryptBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(encryBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        
        UILabel *data1Lab=[UILabel new];
        [self addSubview:data1Lab];
        data1Lab.text=@"加密解密前的数据(hex)";
        data1Lab.font=[UIFont systemFontOfSize:15];
        data1Lab.textColor=RGB(150, 150, 150, 1);
        [data1Lab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(decryptBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(250);
        }];
        self.textField3=[UITextField new];
        [self addSubview:self.textField3];
         self.textField3.font=[UIFont systemFontOfSize:16];
        [self.textField3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
            make.top.equalTo(data1Lab.mas_bottom).offset(6);
            make.height.mas_equalTo(35);
        }];
        UIView *linview2=[UIView new];
        [self addSubview:linview2];
        linview2.backgroundColor=RGB(150, 150, 150, 1);
        [linview2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.textField3);
            make.height.mas_equalTo(1);
            make.top.equalTo(self.textField3.mas_bottom);
        }];
        
        UILabel *data2Lab=[UILabel new];
        [self addSubview:data2Lab];
        data2Lab.text=@"加密解密后的数据(hex)";
        data2Lab.font=[UIFont systemFontOfSize:15];
        data2Lab.textColor=RGB(150, 150, 150, 1);
        [data2Lab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(linview2.mas_bottom).offset(15);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(250);
        }];
        self.textField4=[UITextField new];
        [self addSubview:self.textField4];
        self.textField4.font=[UIFont systemFontOfSize:16];
        [self.textField4 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
            make.top.equalTo(data2Lab.mas_bottom).offset(6);
            make.height.mas_equalTo(35);
        }];
        UIView *linview3=[UIView new];
        [self addSubview:linview3];
        linview3.backgroundColor=RGB(150, 150, 150, 1);
        [linview3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.textField4);
            make.height.mas_equalTo(1);
            make.top.equalTo(self.textField4.mas_bottom);
        }];
        
        
        
    }
    return self;
}
-(void)setmiyaoBtnn
{
    if (self.setmiBlock) {
        self.setmiBlock();
    }
}
-(void)getmiyaoBtnn
{
    if (self.getmiBlock) {
        self.getmiBlock();
    }
}
-(void)modeBtnn
{
    if (self.modelBlock) {
        self.modelBlock();
    }
}
-(void)encryBtnn
{
    if (self.encryBlock) {
        self.encryBlock();
    }
}
-(void)decryptBtnn
{
    if (self.dencryBlock) {
        self.dencryBlock();
    }
}
@end
