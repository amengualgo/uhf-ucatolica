//
//  LockView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "LockView.h"
#import <Masonry.h>
#import "BSprogreUtil.h"
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation LockView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        self.typeStr=@"1";
        _topLab=[UILabel new];
        [self addSubview:_topLab];
        _topLab.text=@"filter";
        _topLab.font=[UIFont systemFontOfSize:15];
        _topLab.textColor=RGB(150, 150, 150, 1);
        [_topLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.top.equalTo(self).offset(2);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(50);
        }];
        
        self.enableBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.enableBtn];
        [self.enableBtn setTitle:@"Enable" forState:UIControlStateNormal];
        [self.enableBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.enableBtn.layer.borderWidth=1;
        self.enableBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
        self.enableBtn.layer.masksToBounds=YES;
        self.enableBtn.layer.cornerRadius=10;
        self.enableBtn.backgroundColor=[UIColor whiteColor];
        [self.enableBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self).offset(30);
            make.height.mas_equalTo(36);
            make.width.mas_equalTo(75);
        }];
        [self.enableBtn addTarget:self action:@selector(enableBtnn) forControlEvents:UIControlEventTouchUpInside];
        
        _addressLab=[UILabel new];
        [self addSubview:_addressLab];
        _addressLab.text=@"Ptr：";
        _addressLab.font=[UIFont systemFontOfSize:16];
        _addressLab.textColor=RGB(150, 150, 150, 1);
        [_addressLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(self.enableBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(55);
        }];
        self.addressField=[UITextField new];
        [self addSubview:self.addressField];
        self.addressField.text = @"32";
        self.addressField.font=[UIFont systemFontOfSize:16];
        [self.addressField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_addressLab.mas_right);
            make.centerY.equalTo(_addressLab);
            make.width.mas_equalTo(75);
            make.height.mas_equalTo(30);
        }];
        UIView *linview=[UIView new];
        [self addSubview:linview];
        linview.backgroundColor=RGB(150, 150, 150, 1);
        [linview mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.addressField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.addressField);
        }];
        UILabel *label=[UILabel new];
        [self addSubview:label];
        label.text=@"(bit)";
        label.font=[UIFont systemFontOfSize:16];
        label.textColor=RGB(150, 150, 150, 1);
        [label mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.addressField);
            make.left.equalTo(self.addressField.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(40);
        }];
        
        _lengthLab=[UILabel new];
        [self addSubview:_lengthLab];
        _lengthLab.text=@"Len：";
        _lengthLab.font=[UIFont systemFontOfSize:16];
        _lengthLab.textColor=RGB(150, 150, 150, 1);
        [_lengthLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(label.mas_right).offset(20);
            make.top.bottom.equalTo(_addressLab);
            make.width.mas_equalTo(55);
        }];
        self.lengthField=[UITextField new];
        [self addSubview:self.lengthField];
        self.lengthField.font=[UIFont systemFontOfSize:16];
        [self.lengthField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_lengthLab.mas_right);
            make.centerY.equalTo(_lengthLab);
            make.width.mas_equalTo(75);
            make.height.mas_equalTo(30);
        }];
        UIView *linview1=[UIView new];
        [self addSubview:linview1];
        linview1.backgroundColor=RGB(150, 150, 150, 1);
        [linview1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.lengthField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.lengthField);
        }];
        UILabel *label1=[UILabel new];
        [self addSubview:label1];
        label1.text=@"(bit)";
        label1.font=[UIFont systemFontOfSize:16];
        label1.textColor=RGB(150, 150, 150, 1);
        [label1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.lengthField);
            make.left.equalTo(self.lengthField.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(40);
        }];
        
        _dataLab=[UILabel new];
        [self addSubview:_dataLab];
        _dataLab.text=@"Data：";
        _dataLab.font=[UIFont systemFontOfSize:16];
        _dataLab.textColor=RGB(150, 150, 150, 1);
        [_dataLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(_addressLab.mas_bottom).offset(18);
            make.width.mas_equalTo(82);
        }];
        self.dataField=[UITextField new];
        [self addSubview:self.dataField];
        self.dataField.font=[UIFont systemFontOfSize:16];
        [self.dataField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_dataLab.mas_right);
            make.centerY.equalTo(_dataLab);
            make.right.equalTo(self).offset(-10);
            make.height.mas_equalTo(30);
        }];
        UIView *linview2=[UIView new];
        [self addSubview:linview2];
        linview2.backgroundColor=RGB(150, 150, 150, 1);
        [linview2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.dataField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.dataField);
        }];
        
        self.epcBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.epcBtn];
        [self.epcBtn setTitle:@"EPC" forState:UIControlStateNormal];
        [self.epcBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.epcBtn.layer.borderWidth=2;
        self.epcBtn.layer.masksToBounds=YES;
        self.epcBtn.layer.cornerRadius=10;
        self.epcBtn.layer.borderColor=[UIColor blueColor].CGColor;
        [self.epcBtn addTarget:self action:@selector(epcbtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat width=([UIScreen mainScreen].bounds.size.width-60)/3.0;
        [self.epcBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(linview2.mas_bottom).offset(10);
            make.width.mas_equalTo(width);
            make.height.mas_equalTo(30);
        }];
        
        self.tidBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.tidBtn];
        [self.tidBtn setTitle:@"TID" forState:UIControlStateNormal];
        [self.tidBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.tidBtn.layer.borderWidth=2;
        [self.tidBtn addTarget:self action:@selector(tidBtnn) forControlEvents:UIControlEventTouchUpInside];
        self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
        self.tidBtn.layer.masksToBounds=YES;
        self.tidBtn.layer.cornerRadius=10;
        [self.tidBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.epcBtn.mas_right).offset(15);
            make.top.bottom.equalTo(self.epcBtn);
            make.width.mas_equalTo(width);
        }];
        
        self.userBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.userBtn];
        [self.userBtn setTitle:@"USER" forState:UIControlStateNormal];
        [self.userBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.userBtn.layer.borderWidth=2;
        [self.userBtn addTarget:self action:@selector(userBtnn) forControlEvents:UIControlEventTouchUpInside];
        self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
        self.userBtn.layer.masksToBounds=YES;
        self.userBtn.layer.cornerRadius=10;
        [self.userBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.tidBtn.mas_right).offset(15);
            make.top.bottom.equalTo(self.epcBtn);
            make.width.mas_equalTo(width);
        }];
        
        UIView *grayView=[UIView new];
        [self addSubview:grayView];
        grayView.backgroundColor=[[UIColor blackColor] colorWithAlphaComponent:0.2];
        grayView.layer.cornerRadius=6;
        grayView.layer.masksToBounds=YES;
        [grayView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.top.equalTo(self);
            make.bottom.equalTo(self.userBtn.mas_bottom).offset(8);
        }];
        [self sendSubviewToBack:grayView];
        
        

        _passLab=[UILabel new];
        [self addSubview:_passLab];
        _passLab.text=@"Access Pwd：";
        _passLab.font=[UIFont systemFontOfSize:16];
        _passLab.textColor=RGB(150, 150, 150, 1);
        [_passLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(_userBtn.mas_bottom).offset(20);
            make.width.mas_equalTo(110);
        }];
        self.passWordField=[UITextField new];
        [self addSubview:self.passWordField];
        self.passWordField.placeholder=@"";
        self.passWordField.font=[UIFont systemFontOfSize:16];
        [self.passWordField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_passLab.mas_right);
            make.centerY.equalTo(_passLab);
            make.height.mas_equalTo(30);
            make.right.equalTo(self).offset(-20);
        }];
        UIView *linView3=[UIView new];
        [self addSubview:linView3];
        linView3.backgroundColor=RGB(150, 150, 150, 1);
        [linView3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.passWordField);
            make.top.equalTo(self.passWordField.mas_bottom);
            make.height.mas_equalTo(1);
        }];


        _lockLab=[UILabel new];
        [self addSubview:_lockLab];
        _lockLab.text=@"Lock Code：";
        _lockLab.font=[UIFont systemFontOfSize:16];
        _lockLab.textColor=RGB(150, 150, 150, 1);
        [_lockLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(linView3.mas_bottom).offset(20);
            make.width.mas_equalTo(110);
        }];
        self.lockWordField=[UITextField new];
        [self addSubview:self.lockWordField];
        self.lockWordField.userInteractionEnabled = NO;
        self.lockWordField.font=[UIFont systemFontOfSize:16];
        [self.lockWordField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_lockLab.mas_right);
            make.centerY.equalTo(_lockLab);
            make.height.mas_equalTo(30);
            make.right.equalTo(self).offset(-20);
        }];
        UIButton *lockBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:lockBtn];
        [lockBtn addTarget:self action:@selector(lockkk) forControlEvents:UIControlEventTouchUpInside];
        [lockBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_lockLab.mas_right);
            make.centerY.equalTo(_lockLab);
            make.height.mas_equalTo(30);
            make.right.equalTo(self).offset(-20);
        }];
        
        UIView *linView4=[UIView new];
        [self addSubview:linView4];
        linView4.backgroundColor=RGB(150, 150, 150, 1);
        [linView4 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.lockWordField);
            make.top.equalTo(self.lockWordField.mas_bottom);
            make.height.mas_equalTo(1);
        }];

     

        _lockBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:_lockBtn];
        [_lockBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_lockBtn setTitle:@"Lock" forState:UIControlStateNormal];
        _lockBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_lockBtn addTarget:self action:@selector(lockBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_lockBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(20);
            make.top.equalTo(linView4.mas_bottom).offset(20);
            make.height.mas_equalTo(AdaptH(40));
            make.right.equalTo(self).offset(-20);
        }];
        
        
    }
    return self;
}
-(void)enableBtnn
{
    if (self.enableBtn.selected==YES) {
        self.enableBtn.selected=NO;
        self.enableBtn.backgroundColor=[UIColor whiteColor];
        self.isFilter=NO;
        [BSprogreUtil showMBProgressWith:self andTipString:@"过滤已关闭" autoHide:YES];
    }
    else
    {
        self.enableBtn.selected=YES;
        self.enableBtn.backgroundColor=RGB(151, 151, 151, 1);
        self.isFilter=YES;
        [BSprogreUtil showMBProgressWith:self andTipString:@"过滤已开启" autoHide:YES];
    }
}
-(void)epcbtnn
{
    self.typeStr=@"1";
    self.epcBtn.layer.borderColor=[UIColor blueColor].CGColor;
    self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    
}
-(void)tidBtnn
{
    self.typeStr=@"2";
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=[UIColor blueColor].CGColor;
    self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    
}
-(void)userBtnn
{
    self.typeStr=@"3";
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.userBtn.layer.borderColor=[UIColor blueColor].CGColor;
}
-(void)lockBtnn
{
    if (self.lockBlock) {
        self.lockBlock();
    }
}
-(void)lockkk
{
    if (self.showlockBlock) {
        self.showlockBlock();
    }
}
@end
