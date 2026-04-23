//
//  WriteMessageView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "WriteMessageView.h"
#import <Masonry.h>
#import "BSprogreUtil.h"
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation WriteMessageView

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
            make.width.mas_equalTo(45);
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
            make.width.mas_equalTo(45);
        }];
        self.lengthField=[UITextField new];
        [self addSubview:self.lengthField];
        self.lengthField.text=@"0";
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
        ///////////////////////////////////////////////////////
        
        
        
        
        _saveLab=[UILabel new];
        [self addSubview:_saveLab];
        _saveLab.text=@"Block：";
        _saveLab.font=[UIFont systemFontOfSize:16];
        _saveLab.textColor=RGB(150, 150, 150, 1);
        [_saveLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(self.userBtn.mas_bottom).offset(15);
            make.width.mas_equalTo(82);
            make.height.mas_equalTo(25);
        }];
        
        self.saveBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.saveBtn];
        [self.saveBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.saveBtn setTitle:@"RESSSSS" forState:UIControlStateNormal];
        self.saveBtn.titleLabel.font=[UIFont systemFontOfSize:16];
        [self.saveBtn addTarget:self action:@selector(saveBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.saveBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_saveLab.mas_right);
            make.centerY.equalTo(_saveLab);
            make.height.mas_equalTo(40);
            make.right.equalTo(self).offset(-10);
        }];
        
        
        
        _addressLab1=[UILabel new];
        [self addSubview:_addressLab1];
        _addressLab1.text=@"Ptr：";
        _addressLab1.font=[UIFont systemFontOfSize:16];
        _addressLab1.textColor=RGB(150, 150, 150, 1);
        [_addressLab1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(_saveLab.mas_bottom).offset(15);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(45);
        }];
        self.addressField1=[UITextField new];
        [self addSubview:self.addressField1];
        self.addressField1.text=@"0";
        self.addressField1.font=[UIFont systemFontOfSize:16];
        [self.addressField1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_addressLab1.mas_right);
            make.centerY.equalTo(_addressLab1);
            make.width.mas_equalTo(65);
            make.height.mas_equalTo(30);
        }];
        UIView *linview3=[UIView new];
        [self addSubview:linview3];
        linview3.backgroundColor=RGB(150, 150, 150, 1);
        [linview3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.addressField1.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.addressField1);
        }];
        UILabel *label2=[UILabel new];
        [self addSubview:label2];
        label2.text=@"(word)";
        label2.font=[UIFont systemFontOfSize:16];
        label2.textColor=RGB(150, 150, 150, 1);
        [label2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.addressField1);
            make.left.equalTo(self.addressField1.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(50);
        }];
        
        _lengthLab1=[UILabel new];
        [self addSubview:_lengthLab1];
        _lengthLab1.text=@"Len：";
        _lengthLab1.font=[UIFont systemFontOfSize:16];
        _lengthLab1.textColor=RGB(150, 150, 150, 1);
        [_lengthLab1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(label2.mas_right).offset(20);
            make.top.bottom.equalTo(_addressLab1);
            make.width.mas_equalTo(45);
        }];
        self.lengthField1=[UITextField new];
        [self addSubview:self.lengthField1];
        self.lengthField1.text=@"4";
        self.lengthField1.font=[UIFont systemFontOfSize:16];
        [self.lengthField1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_lengthLab1.mas_right);
            make.centerY.equalTo(_lengthLab1);
            make.height.mas_equalTo(30);
            make.width.mas_equalTo(60);
        }];
        UIView *linview4=[UIView new];
        [self addSubview:linview4];
        linview4.backgroundColor=RGB(150, 150, 150, 1);
        [linview4 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.lengthField1.mas_bottom);
            make.left.right.equalTo(self.lengthField1);
            make.height.mas_equalTo(1);
        }];
        UILabel *label5=[UILabel new];
        [self addSubview:label5];
        label5.text=@"(word)";
        label5.font=[UIFont systemFontOfSize:16];
        label5.textColor=RGB(150, 150, 150, 1);
        [label5 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.lengthField1);
            make.left.equalTo(self.lengthField1.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(50);
        }];
        
        _passLab=[UILabel new];
        [self addSubview:_passLab];
        _passLab.text=@"Access Pwd：";
        _passLab.font=[UIFont systemFontOfSize:16];
        _passLab.textColor=RGB(150, 150, 150, 1);
        [_passLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(_addressLab1.mas_bottom).offset(18);
            make.width.mas_equalTo(110);
        }];
        self.passWordField1=[UITextField new];
        [self addSubview:self.passWordField1];
        self.passWordField1.text=@"00000000";
       // self.passWordField1.userInteractionEnabled=NO;
        self.passWordField1.font=[UIFont systemFontOfSize:16];
        [self.passWordField1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_passLab.mas_right);
            make.centerY.equalTo(_passLab);
            make.right.equalTo(self).offset(-10);
            make.height.mas_equalTo(30);
        }];
        UIView *linview6=[UIView new];
        [self addSubview:linview6];
        linview6.backgroundColor=RGB(150, 150, 150, 1);
        [linview6 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.passWordField1.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.passWordField1);
        }];
        
        _dataLab1=[UILabel new];
        [self addSubview:_dataLab1];
        _dataLab1.text=@"Write Data：";
        _dataLab1.font=[UIFont systemFontOfSize:16];
        _dataLab1.textColor=RGB(150, 150, 150, 1);
        [_dataLab1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(_passLab.mas_bottom).offset(18);
            make.width.mas_equalTo(110);
        }];
        self.dataField1=[UITextField new];
        [self addSubview:self.dataField1];
        self.dataField1.font=[UIFont systemFontOfSize:16];
        [self.dataField1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(_dataLab1.mas_right);
            make.centerY.equalTo(_dataLab1);
            make.right.equalTo(self).offset(-10);
            make.height.mas_equalTo(30);
        }];
        UIView *linview7=[UIView new];
        [self addSubview:linview7];
        linview7.backgroundColor=RGB(150, 150, 150, 1);
        [linview7 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.dataField1.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.dataField1);
        }];
        
        
        _writeMessageBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:_writeMessageBtn];
        [_writeMessageBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_writeMessageBtn setTitle:@"Write Data" forState:UIControlStateNormal];
        _writeMessageBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_writeMessageBtn addTarget:self action:@selector(writeMessageBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_writeMessageBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(20);
            make.top.equalTo(linview7.mas_bottom).offset(15);
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
    
    self.addressField.text = @"32";
    self.lengthField.text=@"0";
    
}
-(void)tidBtnn
{
    self.typeStr=@"2";
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=[UIColor blueColor].CGColor;
    self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    
    self.addressField.text = @"0";
    self.lengthField.text=@"0";
    
}
-(void)userBtnn
{
    self.typeStr=@"3";
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.userBtn.layer.borderColor=[UIColor blueColor].CGColor;
    
    self.addressField.text = @"0";
    self.lengthField.text=@"0";
}
-(void)saveBtnn
{
    if (self.saveBlock) {
        self.saveBlock();
    }
}
-(void)writeMessageBtnn
{
    if (self.writeMessageBlock) {
        self.writeMessageBlock();
    }
}

@end
