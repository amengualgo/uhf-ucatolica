//
//  SettingView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "SettingView.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation SettingView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        _chooseStr = @"0";
        
        UIScrollView *scrollView = [[UIScrollView alloc]init];
        scrollView.delegate = self;
        [self addSubview:scrollView];
        [scrollView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.top.bottom.equalTo(self);
//            make.height.mas_equalTo(AdaptH(500));
            //make.width.mas_equalTo(AdaptH(300));
        }];
        
        _workLab=[UILabel new];
        [scrollView addSubview:_workLab];
        _workLab.text=@"Working Mode:";
        _workLab.font=[UIFont systemFontOfSize:15];
        _workLab.textColor=RGB(150, 150, 150, 1);
        _workLab.frame=CGRectMake(15, 15, AdaptH(120), AdaptH(25));
//        [_workLab mas_makeConstraints:^(MASConstraintMaker *make) {
//            make.left.top.equalTo(self).offset(15);
//            make.height.mas_equalTo(AdaptH(25));
//            make.width.mas_equalTo(120);
//        }];
        
        self.workBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.workBtn];
        [self.workBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.workBtn setTitle:@"欧洲标准(865~868MHZ)" forState:UIControlStateNormal];
        self.workBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        self.workBtn.titleLabel.textAlignment=NSTextAlignmentLeft;
        [self.workBtn addTarget:self action:@selector(workBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.workBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self->_workLab);
            make.height.mas_equalTo(AdaptH(40));
            make.left.equalTo(self->_workLab.mas_right);
            make.right.equalTo(self);
        }];
        
        //[scrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, 1000)];
        
        _setFrequencyBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_setFrequencyBtn];
        [_setFrequencyBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_setFrequencyBtn setTitle:@"Set Frequency" forState:UIControlStateNormal];
        _setFrequencyBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        _setFrequencyBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_setFrequencyBtn addTarget:self action:@selector(setpinBtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat width=([UIScreen mainScreen].bounds.size.width-30-40)/2.0;
        [_setFrequencyBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.workLab.mas_bottom).offset(5);
            make.height.mas_equalTo(AdaptH(40));
            make.width.mas_equalTo(width);
        }];
        
        _getFrequencyBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_getFrequencyBtn];
        [_getFrequencyBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_getFrequencyBtn setTitle:@"Get Frequency" forState:UIControlStateNormal];
        _getFrequencyBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        _getFrequencyBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_getFrequencyBtn addTarget:self action:@selector(readpinBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_getFrequencyBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.setFrequencyBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
/*
        _usaBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_usaBtn];
        _usaBtn.backgroundColor=RGB(230, 230, 230, 1);
        _usaBtn.layer.masksToBounds=YES;
        _usaBtn.layer.cornerRadius=11;
        _usaBtn.layer.borderWidth=1;
        _usaBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_usaBtn addTarget:self action:@selector(usaBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_usaBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(20);
            make.top.equalTo(self.readpinBtn.mas_bottom).offset(25);
            make.width.mas_equalTo(22);
            make.height.mas_equalTo(22);
        }];
        _usaLab=[UILabel new];
        [scrollView addSubview:_usaLab];
        _usaLab.font=[UIFont systemFontOfSize:15];
        _usaLab.text=@"US";
        _usaLab.textColor=[UIColor blackColor];
        [_usaLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.usaBtn);
            make.height.mas_equalTo(25);
            make.left.equalTo(self.usaBtn.mas_right).offset(10);
            make.width.mas_equalTo(50);
        }];

        _brazilBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_brazilBtn];
        _brazilBtn.backgroundColor=[UIColor whiteColor];
        _brazilBtn.layer.masksToBounds=YES;
        _brazilBtn.layer.cornerRadius=11;
        _brazilBtn.layer.borderWidth=1;
        _brazilBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_brazilBtn addTarget:self action:@selector(brazilBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_brazilBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.usaLab.mas_right).offset(30);
            make.top.bottom.equalTo(self.usaBtn);
            make.width.mas_equalTo(22);

        }];
        _brazilLab=[UILabel new];
        [scrollView addSubview:_brazilLab];
        _brazilLab.font=[UIFont systemFontOfSize:15];
        _brazilLab.text=@"BRA";
        _brazilLab.textColor=[UIColor blackColor];
        [_brazilLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.usaLab);
            make.left.equalTo(self.brazilBtn.mas_right).offset(10);
            make.width.mas_equalTo(50);
        }];

        _otherBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_otherBtn];
        _otherBtn.backgroundColor=[UIColor whiteColor];
        _otherBtn.layer.masksToBounds=YES;
        _otherBtn.layer.cornerRadius=11;
        _otherBtn.layer.borderWidth=1;
        _otherBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_otherBtn addTarget:self action:@selector(otherBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_otherBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.brazilLab.mas_right).offset(30);
            make.top.bottom.equalTo(self.usaBtn);
            make.width.mas_equalTo(22);

        }];
        _otherLab=[UILabel new];
        [scrollView addSubview:_otherLab];
        _otherLab.font=[UIFont systemFontOfSize:15];
        _otherLab.text=@"Other";
        _otherLab.textColor=[UIColor blackColor];
        [_otherLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.usaLab);
            make.left.equalTo(self.otherBtn.mas_right).offset(10);
            make.width.mas_equalTo(50);
        }];

        UILabel *hopLab=[UILabel new];
        [scrollView addSubview:hopLab];
        hopLab.text=@"Hop:";
        hopLab.font=[UIFont systemFontOfSize:16];
        hopLab.textColor=RGB(150, 150, 150, 1);
        [hopLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.otherBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(52);
        }];
        self.hopBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.hopBtn];
        [self.hopBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.hopBtn setTitle:@"902.12" forState:UIControlStateNormal];
        self.hopBtn.titleLabel.textAlignment=NSTextAlignmentLeft;
        self.hopBtn.titleLabel.font=[UIFont systemFontOfSize:16];
        [self.hopBtn addTarget:self action:@selector(hopBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.hopBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(hopLab);
            make.height.mas_equalTo(AdaptH(40));
            make.left.equalTo(hopLab.mas_right);
            make.right.equalTo(self);
        }];
        _setdianBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_setdianBtn];
        [_setdianBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_setdianBtn setTitle:@"Set FreHop" forState:UIControlStateNormal];
        _setdianBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        _setdianBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_setdianBtn addTarget:self action:@selector(setdianBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_setdianBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.right.equalTo(self).offset(-15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(hopLab.mas_bottom).offset(5);
        }];
*/

        _putLab=[UILabel new];
        [scrollView addSubview:_putLab];
        _putLab.text=@"Output Power:";
        _putLab.font=[UIFont systemFontOfSize:15];
        _putLab.textColor=RGB(150, 150, 150, 1);
        [_putLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.getFrequencyBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(125);
        }];
        self.putBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.putBtn];
        [self.putBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.putBtn setTitle:@"10" forState:UIControlStateNormal];
        self.putBtn.titleLabel.font=[UIFont systemFontOfSize:16];
        [self.putBtn addTarget:self action:@selector(putBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.putBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.putLab);
            make.height.mas_equalTo(AdaptH(40));
            make.left.equalTo(self.putLab.mas_right);
            make.right.equalTo(self).offset(-60);
        }];
        
        UILabel *label1=[UILabel new];
        label1.text = @"dBm";
        label1.font=[UIFont systemFontOfSize:16];
        [scrollView addSubview:label1];
        [label1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.putBtn.mas_right);
            make.top.bottom.equalTo(self.putBtn);
            make.width.mas_equalTo(50);
        }];
        
        _setPowerBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_setPowerBtn];
        [_setPowerBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_setPowerBtn setTitle:@"Set Power" forState:UIControlStateNormal];
        _setPowerBtn.backgroundColor=RGB(210, 210, 210, 1);
        _setPowerBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        [_setPowerBtn addTarget:self action:@selector(setgongBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_setPowerBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(self->_putLab.mas_bottom).offset(5);
            make.width.mas_equalTo(width);
        }];

        _getPowerBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_getPowerBtn];
        [_getPowerBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_getPowerBtn setTitle:@"Get Power" forState:UIControlStateNormal];
        _getPowerBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        _getPowerBtn.backgroundColor=RGB(210, 210, 210, 1);
        [_getPowerBtn addTarget:self action:@selector(readgongBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_getPowerBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_setPowerBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        
        UILabel *scanMode=[UILabel new];
        [scrollView addSubview:scanMode];
        scanMode.text=@"Scan Model:";
        scanMode.font=[UIFont systemFontOfSize:15];
        scanMode.textColor=RGB(150, 150, 150, 1);
        [scanMode mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self->_setPowerBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(125);
        }];
        
        _epcBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_epcBtn];
        _epcBtn.backgroundColor=RGB(230, 230, 230, 1);
        _epcBtn.layer.masksToBounds=YES;
        _epcBtn.layer.cornerRadius=11;
        _epcBtn.layer.borderWidth=1;
        _epcBtn.selected = YES;
        _epcBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_epcBtn addTarget:self action:@selector(epcBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_epcBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(scanMode.mas_bottom).offset(5);
            make.width.mas_equalTo(22);
            make.height.mas_equalTo(22);
        }];
        UILabel *epcLab=[UILabel new];
        [scrollView addSubview:epcLab];
        epcLab.font=[UIFont systemFontOfSize:15];
        epcLab.text=@"EPC";
        epcLab.textColor=[UIColor blackColor];
        [epcLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self->_epcBtn);
            make.height.mas_equalTo(22);
            make.left.equalTo(self->_epcBtn.mas_right).offset(10);
            make.width.mas_equalTo(40);
        }];
        
        _epcTidBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_epcTidBtn];
        _epcTidBtn.backgroundColor=[UIColor whiteColor];
        _epcTidBtn.layer.masksToBounds=YES;
        _epcTidBtn.layer.cornerRadius=11;
        _epcTidBtn.layer.borderWidth=1;
        _epcTidBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_epcTidBtn addTarget:self action:@selector(epcTidBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_epcTidBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(epcLab.mas_right).offset(20);
            make.top.bottom.equalTo(self->_epcBtn);
            make.width.mas_equalTo(22);
            
        }];
        UILabel *epcTidLab=[UILabel new];
        [scrollView addSubview:epcTidLab];
        epcTidLab.font=[UIFont systemFontOfSize:15];
        epcTidLab.text=@"EPC+TID";
        epcTidLab.textColor=[UIColor blackColor];
        [epcTidLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(epcLab);
            make.left.equalTo(self->_epcTidBtn.mas_right).offset(10);
            make.width.mas_equalTo(70);
        }];
        
        _epcTidUserBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_epcTidUserBtn];
        _epcTidUserBtn.backgroundColor=[UIColor whiteColor];
        _epcTidUserBtn.layer.masksToBounds=YES;
        _epcTidUserBtn.layer.cornerRadius=11;
        _epcTidUserBtn.layer.borderWidth=1;
        _epcTidUserBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [_epcTidUserBtn addTarget:self action:@selector(epcTidUserBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_epcTidUserBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(epcTidLab.mas_right).offset(20);
            make.top.bottom.equalTo(self->_epcBtn);
            make.width.mas_equalTo(22);
            
        }];
        UILabel *epcTidUserLab=[UILabel new];
        [scrollView addSubview:epcTidUserLab];
        epcTidUserLab.font=[UIFont systemFontOfSize:15];
        epcTidUserLab.text=@"EPC+TID+USER";
        epcTidUserLab.textColor=[UIColor blackColor];
        [epcTidUserLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(epcLab);
            make.left.equalTo(self->_epcTidUserBtn.mas_right).offset(10);
            make.width.mas_equalTo(120);
        }];
        
     
        self.containerView = [UIView new];
        self.containerView.hidden = true;
        [scrollView addSubview:self.containerView];
        [self.containerView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self).offset(15);
            //make.top.equalTo(self.epcBtn.mas_bottom).offset(0);
            self.containerTopConstraint = make.top.equalTo(self.epcBtn.mas_bottom).offset(0);
            self.containerHeightConstraint = make.height.mas_equalTo(0);
//            make.height.mas_equalTo(20);
//            make.width.mas_equalTo(300); // 设置容器的宽度
        }];

       self.userPtrLab = [UILabel new];
        [self.containerView addSubview:self.userPtrLab];
        self.userPtrLab.text = @"userPtr:";
        self.userPtrLab.font = [UIFont systemFontOfSize:14];
        self.userPtrLab.textColor = RGB(131, 131, 131, 1);
        [self.userPtrLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.top.equalTo(self.containerView);
            self.userPtrLabHeightConstraint = make.height.mas_equalTo(0);
            //make.height.mas_equalTo(20);
            make.width.mas_equalTo(60);
        }];

        self.userText = [UITextField new];
        [self.containerView addSubview:self.userText];
        self.userText.keyboardType = UIKeyboardTypePhonePad;
        self.userText.font = [UIFont systemFontOfSize:16];
        self.userText.text = @"0";
        [self.userText mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.userPtrLab.mas_right).offset(5);
            make.top.bottom.equalTo(self.userPtrLab);
            make.width.mas_equalTo(100);
        }];

        UIView *linView = [UIView new];
        [self.containerView addSubview:linView];
        linView.backgroundColor = RGB(88, 88, 88, 1);
        [linView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.userText);
            make.top.equalTo(self.userText.mas_bottom);
            make.height.mas_equalTo(2);
        }];

        UILabel *userPtrlenLab = [UILabel new];
        [self.containerView addSubview:userPtrlenLab];
        userPtrlenLab.text = @"userLen:";
        userPtrlenLab.font = [UIFont systemFontOfSize:14];
        userPtrlenLab.textColor = RGB(131, 131, 131, 1);
        [userPtrlenLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.userText.mas_right).offset(15);
            make.top.bottom.equalTo(self.userPtrLab);
            make.width.mas_equalTo(60);
        }];

        self.userLengthText = [UITextField new];
        [self.containerView addSubview:self.userLengthText];
        self.userLengthText.text = @"6";
        self.userLengthText.keyboardType = UIKeyboardTypePhonePad;
        self.userLengthText.font = [UIFont systemFontOfSize:16];
        [self.userLengthText mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(userPtrlenLab.mas_right).offset(5);
            make.top.bottom.equalTo(self.userPtrLab);
            make.width.mas_equalTo(100);
        }];

        UIView *linView1 = [UIView new];
        [self.containerView addSubview:linView1];
        linView1.backgroundColor = RGB(88, 88, 88, 1);
        [linView1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.userLengthText);
            make.top.equalTo(self.userLengthText.mas_bottom);
            make.height.mas_equalTo(2);
        }];
        
        self.setScanModelBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.setScanModelBtn];
        [self.setScanModelBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.setScanModelBtn setTitle:@"Set ScanModel" forState:UIControlStateNormal];
        self.setScanModelBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        self.setScanModelBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.setScanModelBtn addTarget:self action:@selector(setScanModelBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.setScanModelBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.width.mas_equalTo(width);
            //make.right.equalTo(self).offset(-15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(self.userLengthText.mas_bottom).offset(15);
        }];
        
        self.getScanModelBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.getScanModelBtn];
        [self.getScanModelBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.getScanModelBtn setTitle:@"Get ScanModel" forState:UIControlStateNormal];
        self.getScanModelBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        self.getScanModelBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.getScanModelBtn addTarget:self action:@selector(getScanModelBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.getScanModelBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            //make.left.equalTo(self.setScanModelBtn.mas_right).offset(15);
            make.top.equalTo(self.setScanModelBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
            make.height.mas_equalTo(AdaptH(40));
        }];
        
               
        UILabel *parameterLab=[UILabel new];
        [scrollView addSubview:parameterLab];
        parameterLab.text = @"Barcode Parameter:";
        parameterLab.font=[UIFont systemFontOfSize:15];
        parameterLab.textColor=RGB(150, 150, 150, 1);
        [parameterLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.getScanModelBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(150);
        }];
        
        self.parameterField=[UITextField new];
        [scrollView addSubview:self.parameterField];
        self.parameterField.text = @"";
        self.parameterField.font=[UIFont systemFontOfSize:16];
        [self.parameterField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(parameterLab.mas_right);
            make.centerY.equalTo(parameterLab);
            make.width.mas_equalTo(180);
            make.height.mas_equalTo(30); 
        }];
        UIView *parameterLinview=[UIView new];
        [scrollView addSubview:parameterLinview];
        parameterLinview.backgroundColor=RGB(150, 150, 150, 1);
        [parameterLinview mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.parameterField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.parameterField);
        }];
        
        self.setParameterBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.setParameterBtn];
        [self.setParameterBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.setParameterBtn setTitle:@"Set Parameter" forState:UIControlStateNormal];
        self.setParameterBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.setParameterBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        [self.setParameterBtn addTarget:self action:@selector(setParamerBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.setParameterBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.right.equalTo(self).offset(-15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(self.parameterField.mas_bottom).offset(5);
//            make.width.mas_equalTo(width);
        }];
        
        
        UILabel *timeoutLab=[UILabel new];
        [scrollView addSubview:timeoutLab];
        timeoutLab.text = @"Barcode Timeout:";
        timeoutLab.font=[UIFont systemFontOfSize:15];
        timeoutLab.textColor=RGB(150, 150, 150, 1);
        [timeoutLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.setParameterBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(130);
        }];
        
        self.timeoutField=[UITextField new];
        [scrollView addSubview:self.timeoutField];
        self.timeoutField.text = @"";
        self.timeoutField.font=[UIFont systemFontOfSize:16];
        [self.timeoutField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(timeoutLab.mas_right);
            make.centerY.equalTo(timeoutLab);
            make.width.mas_equalTo(120);
            make.height.mas_equalTo(30);
        }];
        UIView *linview=[UIView new];
        [scrollView addSubview:linview];
        linview.backgroundColor=RGB(150, 150, 150, 1);
        [linview mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.timeoutField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.timeoutField);
        }];
        UILabel *label=[UILabel new];
        [scrollView addSubview:label];
        label.text=@" (0.5~9.9s)";
        label.font=[UIFont systemFontOfSize:16];
        label.textColor=RGB(150, 150, 150, 1);
        [label mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.timeoutField);
            make.left.equalTo(self.timeoutField.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(100);
        }];
        
        _setTimeoutBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_setTimeoutBtn];
        [_setTimeoutBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_setTimeoutBtn setTitle:@"Set Timeout" forState:UIControlStateNormal];
        _setTimeoutBtn.backgroundColor=RGB(210, 210, 210, 1);
        _setTimeoutBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        [_setTimeoutBtn addTarget:self action:@selector(setTimeoutBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_setTimeoutBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.right.equalTo(self).offset(-15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(self.timeoutField.mas_bottom).offset(5);
//            make.width.mas_equalTo(width);
        }];
        
        UILabel *codeIdLab=[UILabel new];
        [scrollView addSubview:codeIdLab];
        codeIdLab.text = @"Barcode CodeID:";
        codeIdLab.font=[UIFont systemFontOfSize:15];
        codeIdLab.textColor=RGB(150, 150, 150, 1);
        [codeIdLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.setTimeoutBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(125);
        }];
        
        self.codeIdYesBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:self.codeIdYesBtn];
        [self.codeIdYesBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.codeIdYesBtn setTitle:@"CodeID Open" forState:UIControlStateNormal];
        self.codeIdYesBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.codeIdYesBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        [self.codeIdYesBtn addTarget:self action:@selector(codeIdYesBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.codeIdYesBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(codeIdLab.mas_bottom).offset(5);
            make.width.mas_equalTo(width);
        }];
        
        self.codeIdNoBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview: self.codeIdNoBtn];
        [self.codeIdNoBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.codeIdNoBtn setTitle:@"CodeID Close" forState:UIControlStateNormal];
        self.codeIdNoBtn.titleLabel.font =[UIFont systemFontOfSize:16];
        self.codeIdNoBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.codeIdNoBtn addTarget:self action:@selector(codeIdNoBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.codeIdNoBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.codeIdYesBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        
        UILabel *buzzerlabel=[UILabel new];
        [scrollView addSubview:buzzerlabel];
        buzzerlabel.text = @"Buzzer:";
        buzzerlabel.font=[UIFont systemFontOfSize:15];
        buzzerlabel.textColor=RGB(150, 150, 150, 1);
        [buzzerlabel mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.codeIdYesBtn.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(30));
            make.width.mas_equalTo(125);
        }];
        
        _buzzerOpen=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_buzzerOpen];
        [_buzzerOpen setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_buzzerOpen setTitle:@"Buzzer Open" forState:UIControlStateNormal];
        _buzzerOpen.backgroundColor=RGB(210, 210, 210, 1);
        _buzzerOpen.titleLabel.font =[UIFont systemFontOfSize:16];
        [_buzzerOpen addTarget:self action:@selector(buzzerOpenn) forControlEvents:UIControlEventTouchUpInside];
        [_buzzerOpen mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.height.mas_equalTo(AdaptH(40));
            make.top.equalTo(buzzerlabel.mas_bottom).offset(5);
            make.width.mas_equalTo(width);
        }];
        
        _buzzerClose=[UIButton buttonWithType:UIButtonTypeCustom];
        [scrollView addSubview:_buzzerClose];
        [_buzzerClose setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [_buzzerClose setTitle:@"Buzzer Close" forState:UIControlStateNormal];
        _buzzerClose.titleLabel.font =[UIFont systemFontOfSize:16];
        _buzzerClose.backgroundColor=RGB(210, 210, 210, 1);
        [_buzzerClose addTarget:self action:@selector(buzzerClosee) forControlEvents:UIControlEventTouchUpInside];
        [_buzzerClose mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.buzzerOpen);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        
        UILabel *rssiLab = [UILabel new];
        [scrollView addSubview:rssiLab];
        rssiLab.text = @"RSSI:";
        rssiLab.font=[UIFont systemFontOfSize:16];
        rssiLab.textColor=RGB(150, 150, 150, 1);
        [rssiLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(self.buzzerOpen.mas_bottom).offset(30);
            make.width.mas_equalTo(40);
            make.height.mas_equalTo(AdaptH(30));
        }];
        
        self.rssiYesBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.rssiYesBtn];
        [self.rssiYesBtn setTitle:@"YES" forState:UIControlStateNormal];
        [self.rssiYesBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.rssiYesBtn.layer.borderWidth=2;
        self.rssiYesBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.rssiYesBtn.layer.masksToBounds=YES;
        self.rssiYesBtn.layer.cornerRadius=10;
        [self.rssiYesBtn addTarget:self action:@selector(rssiYesBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.rssiYesBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(rssiLab.mas_right).offset(40);
            make.bottom.equalTo(rssiLab.mas_bottom);
            make.width.mas_equalTo(70);
            make.height.mas_equalTo(30);
        }];
        
        self.rssiNoBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.rssiNoBtn];
        [self.rssiNoBtn setTitle:@"NO" forState:UIControlStateNormal];
        [self.rssiNoBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.rssiNoBtn.layer.borderWidth=2;
        self.rssiNoBtn.layer.borderColor=[UIColor grayColor].CGColor;
        self.rssiNoBtn.layer.masksToBounds=YES;
        self.rssiNoBtn.layer.cornerRadius=10;
        [self.rssiNoBtn addTarget:self action:@selector(rssiNoBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.rssiNoBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.rssiYesBtn.mas_right).offset(20);
            make.bottom.equalTo(rssiLab.mas_bottom);
            make.width.mas_equalTo(70);
            make.height.mas_equalTo(30);
        }];
        
        
        
        
        [scrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, 800)];
        
    }
    return self;
}
-(void)buzzerOpenn
{
    if (self.buzzerOpenBlock) {
        self.buzzerOpenBlock();
    }
}

-(void)buzzerClosee
{
    if (self.buzzerCloseBlock) {
        self.buzzerCloseBlock();
    }
}

-(void)workBtnn
{
    if (self.workBlock) {
        self.workBlock();
    }
}
-(void)setpinBtnn
{
    if (self.setFrequencyBlock) {
        self.setFrequencyBlock();
    }
}
-(void)readpinBtnn
{
    if (self.getFrequencyBlock) {
        self.getFrequencyBlock();
    }
}
-(void)usaBtnn
{
    self.usaBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.brazilBtn.backgroundColor=[UIColor whiteColor];
    self.otherBtn.backgroundColor=[UIColor whiteColor];
    if (self.usaBlock) {
        self.usaBlock();
    }
    
}
-(void)brazilBtnn
{
    self.brazilBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.usaBtn.backgroundColor=[UIColor whiteColor];
    self.otherBtn.backgroundColor=[UIColor whiteColor];
    if (self.brazilBlock) {
        self.brazilBlock();
    }
}
-(void)otherBtnn
{
    self.otherBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.brazilBtn.backgroundColor=[UIColor whiteColor];
    self.usaBtn.backgroundColor=[UIColor whiteColor];
    if (self.otherBlock) {
        self.otherBlock();
    }
}
-(void)hopBtnn
{
    if (self.hopBlock) {
        self.hopBlock();
    }
}
-(void)setdianBtnn
{
    if (self.setHotBlock) {
        self.setHotBlock();
    }
}
-(void)putBtnn
{
    if (self.putBlock) {
        self.putBlock();
    }
}
-(void)setgongBtnn
{
    if (self.setPowerBlock) {
        self.setPowerBlock();
    }
}
-(void)readgongBtnn
{
    if (self.getPowerBlock) {
        self.getPowerBlock();
    }
}

-(void)epcBtnn
{
//    if (self.epcBlock) {
//        self.epcBlock();
//    }
//    if (self.cleanBlock) {
//        self.cleanBlock();
//    }
    _chooseStr = @"0";
    self.epcBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.epcTidBtn.backgroundColor=[UIColor whiteColor];
    self.epcTidUserBtn.backgroundColor=[UIColor whiteColor];
    self.containerView.hidden = true;
    self.containerTopConstraint.equalTo(@0);
    self.containerHeightConstraint.equalTo(@0);
    self.userPtrLabHeightConstraint.equalTo(@0);
}
-(void)epcTidBtnn
{
    _chooseStr = @"1";
    self.epcBtn.backgroundColor=[UIColor whiteColor];
    self.epcTidBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.epcTidUserBtn.backgroundColor=[UIColor whiteColor];
    self.containerView.hidden = true;
    self.containerTopConstraint.equalTo(@0);
    self.containerHeightConstraint.equalTo(@0);
    self.userPtrLabHeightConstraint.equalTo(@0);
}
-(void)epcTidUserBtnn
{
    _chooseStr = @"2";
    
    self.epcBtn.backgroundColor=[UIColor whiteColor];
    self.epcTidBtn.backgroundColor=[UIColor whiteColor];
    self.epcTidUserBtn.backgroundColor=RGB(230, 230, 230, 1);
    self.containerView.hidden = false;
    self.containerTopConstraint.equalTo(@20);
    self.containerHeightConstraint.equalTo(@20);
    self.userPtrLabHeightConstraint.equalTo(@20);
}
-(void)setScanModelBtnn
{
    if (self.setScanModelBtnBlock) {
        self.setScanModelBtnBlock();
    }
}
-(void)getScanModelBtnn
{
    if (self.getScanModelBtnBlock) {
        self.getScanModelBtnBlock();
    }
}
-(void)chooseSelectWith:(NSString *)chooseStr
{
    _chooseStr = chooseStr;
    if (chooseStr.integerValue == 0) {
        [self epcBtnn];
    } else if (chooseStr.integerValue == 1) {
        [self epcTidBtnn];
    } else if (chooseStr.integerValue == 2) {
        [self epcTidUserBtnn];
    }
}


-(void)rssiYesBtnn
{
    if (self.rssiYesBtnBlock) {
        self.rssiNoBtn.layer.borderColor=[UIColor grayColor].CGColor;
        self.rssiYesBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.rssiYesBtnBlock();
    }
}
-(void)rssiNoBtnn
{
    if (self.rssiNoBtnBlock) {
        self.rssiYesBtn.layer.borderColor=[UIColor grayColor].CGColor;
        self.rssiNoBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.rssiNoBtnBlock();
    }
}


-(void)setParamerBtnn
{
    if (self.setParamerBtnBlock) {
        self.setParamerBtnBlock();
    }
}
-(void)codeIdYesBtnn
{
    if (self.codeIdOpenBtnBlock) {
        self.codeIdNoBtn.layer.borderColor=[UIColor grayColor].CGColor;
        self.codeIdYesBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.codeIdOpenBtnBlock();
    }
}
-(void)codeIdNoBtnn
{
    if (self.codeIdCloseBtnBlock) {
        self.codeIdYesBtn.layer.borderColor=[UIColor grayColor].CGColor;
        self.codeIdNoBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.codeIdCloseBtnBlock();
    }
}
-(void)setTimeoutBtnn
{
    if (self.setTimeoutBtnBlock) {
        self.setTimeoutBtnBlock();
    }
}

@end
