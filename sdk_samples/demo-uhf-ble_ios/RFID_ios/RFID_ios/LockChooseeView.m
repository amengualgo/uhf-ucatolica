//
//  LockChooseeView.m
//  RFID_ios
//
//  Created by hjl on 2018/11/7.
//  Copyright © 2018年  . All rights reserved.
//

#import "LockChooseeView.h"
#import <Masonry.h>
@implementation LockChooseeView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        self.backgroundColor=[[UIColor blackColor] colorWithAlphaComponent:0.5];
        UIView *bigView=[UIView new];
        [self addSubview:bigView];
        bigView.backgroundColor=[UIColor whiteColor];
        [bigView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self).offset(100);
            make.left.equalTo(self).offset(15);
            make.right.equalTo(self).offset(-15);
            make.height.mas_equalTo(400);
        }];
        
        UILabel *lockLab=[UILabel new];
        [bigView addSubview:lockLab];
        lockLab.text=@"Lock Code：";
        lockLab.font=[UIFont systemFontOfSize:18];
        [lockLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(bigView).offset(15);
            make.top.equalTo(bigView).offset(20);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(200);
        }];
        
        
        self.openBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.openBtn];
        [self.openBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.openBtn addTarget:self action:@selector(chooseopen) forControlEvents:UIControlEventTouchUpInside];
        [self.openBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(bigView).offset(15);
            make.top.equalTo(lockLab.mas_bottom).offset(15);
            make.width.mas_equalTo(20);
            make.height.mas_equalTo(20);
        }];
        UILabel *label1=[UILabel new];
        [bigView addSubview:label1];
        label1.text=@"Open";
        label1.font=[UIFont systemFontOfSize:16];
        [label1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.openBtn.mas_right).offset(9);
            make.centerY.equalTo(self.openBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(45);
        }];
        
        self.lockBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.lockBtn];
        [self.lockBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.lockBtn addTarget:self action:@selector(chooseLock) forControlEvents:UIControlEventTouchUpInside];
        [self.lockBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(label1.mas_right).offset(13);
            make.top.bottom.equalTo(self.openBtn);
            make.width.mas_equalTo(20);
            
        }];
        UILabel *label2=[UILabel new];
        [bigView addSubview:label2];
        label2.text=@"Lock";
        [label2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.lockBtn.mas_right).offset(10);
            make.centerY.equalTo(self.lockBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(40);
        }];
        
        
        self.longLockBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.longLockBtn];
        [self.longLockBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.longLockBtn addTarget:self action:@selector(chooselongLock) forControlEvents:UIControlEventTouchUpInside];
        [self.longLockBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(label2.mas_right).offset(13);
            make.top.bottom.equalTo(self.openBtn);
            make.width.mas_equalTo(20);
            
        }];
        UILabel *label3=[UILabel new];
        [bigView addSubview:label3];
        label3.text=@"Permanent mask";
        label3.font =[UIFont systemFontOfSize:16];
        [label3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.longLockBtn.mas_right).offset(10);
            make.centerY.equalTo(self.longLockBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(128);
        }];
        
        
        self.killBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.killBtn];
        [self.killBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.killBtn addTarget:self action:@selector(choosekill) forControlEvents:UIControlEventTouchUpInside];
        [self.killBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.openBtn);
            make.top.equalTo(self.openBtn.mas_bottom).offset(23);
            make.height.mas_equalTo(20);
            
        }];
        UILabel *label4=[UILabel new];
        [bigView addSubview:label4];
        label4.text=@"kill";
        [label4 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.killBtn.mas_right).offset(10);
            make.centerY.equalTo(self.killBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(80);
        }];
        
        self.accessBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.accessBtn];
        [self.accessBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.accessBtn addTarget:self action:@selector(chooseaccess) forControlEvents:UIControlEventTouchUpInside];
        [self.accessBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.openBtn);
            make.top.equalTo(self.killBtn.mas_bottom).offset(23);
            make.height.mas_equalTo(20);
            
        }];
        UILabel *label5=[UILabel new];
        [bigView addSubview:label5];
        label5.text=@"access";
        [label5 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.accessBtn.mas_right).offset(10);
            make.centerY.equalTo(self.accessBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(80);
        }];
        
        self.epcBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.epcBtn];
        [self.epcBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.epcBtn addTarget:self action:@selector(chooseepc) forControlEvents:UIControlEventTouchUpInside];
        [self.epcBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.openBtn);
            make.top.equalTo(self.accessBtn.mas_bottom).offset(23);
            make.height.mas_equalTo(20);
            
        }];
        UILabel *label6=[UILabel new];
        [bigView addSubview:label6];
        label6.text=@"epc";
        [label6 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.epcBtn.mas_right).offset(10);
            make.centerY.equalTo(self.epcBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(80);
        }];
        
        self.tidBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.tidBtn];
        [self.tidBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.tidBtn addTarget:self action:@selector(choosetid) forControlEvents:UIControlEventTouchUpInside];
        [self.tidBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.openBtn);
            make.top.equalTo(self.epcBtn.mas_bottom).offset(23);
            make.height.mas_equalTo(20);
            
        }];
        UILabel *label7=[UILabel new];
        [bigView addSubview:label7];
        label7.text=@"tid";
        [label7 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.tidBtn.mas_right).offset(10);
            make.centerY.equalTo(self.tidBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(80);
        }];
        
        self.userBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bigView addSubview:self.userBtn];
        [self.userBtn setBackgroundImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
        [self.userBtn addTarget:self action:@selector(chooseuser) forControlEvents:UIControlEventTouchUpInside];
        [self.userBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.openBtn);
            make.top.equalTo(self.tidBtn.mas_bottom).offset(23);
            make.height.mas_equalTo(20);
            
        }];
        UILabel *label8=[UILabel new];
        [bigView addSubview:label8];
        label8.text=@"user";
        [label8 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.userBtn.mas_right).offset(10);
            make.centerY.equalTo(self.userBtn);
            make.height.mas_equalTo(20);
            make.width.mas_equalTo(80);
        }];
        
        
        UIButton *sureBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:sureBtn];
        [sureBtn setTitle:@"OK" forState:UIControlStateNormal];
        [sureBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [sureBtn addTarget:self action:@selector(surebtn) forControlEvents:UIControlEventTouchUpInside];
        [sureBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.right.equalTo(bigView).offset(-20);
            make.bottom.equalTo(bigView).offset(-20);
            make.width.mas_equalTo(60);
            make.height.mas_equalTo(30);
        }];
        
        UIButton *cancelBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:cancelBtn];
        [cancelBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [cancelBtn setTitle:@"CANCEL" forState:UIControlStateNormal];
        [cancelBtn addTarget:self action:@selector(cancel) forControlEvents:UIControlEventTouchUpInside];
        [cancelBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.right.equalTo(bigView).offset(-90);
            make.top.bottom.equalTo(sureBtn);
            make.width.mas_equalTo(85);
        }];
        
        
        
        
    }
    return self;
}

-(void)chooseopen
{
    self.openBtn.tag = 1;
    [self.openBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    self.lockBtn.tag = 0;
    [self.lockBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
}
-(void)chooseLock
{
    self.lockBtn.tag = 1;
    
    [self.lockBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    self.openBtn.tag = 0;
    [self.openBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
}
-(void)chooselongLock
{
    if (self.longLockBtn.tag==0) {
        self.longLockBtn.tag = 1;
        [self.longLockBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.longLockBtn.tag = 0;
        [self.longLockBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}
-(void)choosekill
{
    if (self.killBtn.tag==0) {
        self.killBtn.tag = 1;
        [self.killBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.killBtn.tag = 0;
        [self.killBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}
-(void)chooseaccess
{
    if (self.accessBtn.tag==0) {
        self.accessBtn.tag = 1;
        [self.accessBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.accessBtn.tag = 0;
        [self.accessBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}
-(void)chooseepc
{
    if (self.epcBtn.tag==0) {
        self.epcBtn.tag = 1;
        [self.epcBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.epcBtn.tag = 0;
        [self.epcBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}
-(void)choosetid
{
    if (self.tidBtn.tag==0) {
        self.tidBtn.tag = 1;
        [self.tidBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.tidBtn.tag = 0;
        [self.tidBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}
-(void)chooseuser
{
    if (self.userBtn.tag==0) {
        self.userBtn.tag = 1;
        [self.userBtn setImage:[UIImage imageNamed:@"选项icon_选中的副本"] forState:UIControlStateNormal];
    }
    else
    {
        self.userBtn.tag = 0;
        [self.userBtn setImage:[UIImage imageNamed:@"选项icon的副本"] forState:UIControlStateNormal];
    }
}

-(void)cancel
{
     [self removeFromSuperview];
}
-(void)surebtn
{
     [self removeFromSuperview];
    
    NSString *aa=@"000000000000000000000000";
    if (self.userBtn.tag==1) {
        aa = [aa stringByReplacingCharactersInRange:NSMakeRange(12, 1) withString:@"1"];
        if (self.lockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(22, 1) withString:@"1"];
        }
        if (self.longLockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(23, 1) withString:@"1"];
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(13, 1) withString:@"1"];
        }
    }
    
    if (self.tidBtn.tag==1) {
        aa = [aa stringByReplacingCharactersInRange:NSMakeRange(10, 1) withString:@"1"];
        if (self.lockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(20, 1) withString:@"1"];
        }
        if (self.longLockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(21, 1) withString:@"1"];
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(11, 1) withString:@"1"];
        }
    }
    
    if (self.epcBtn.tag==1) {
        aa = [aa stringByReplacingCharactersInRange:NSMakeRange(8, 1) withString:@"1"];
        if (self.lockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(18, 1) withString:@"1"];
        }
        if (self.longLockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(19, 1) withString:@"1"];
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(9, 1) withString:@"1"];
        }
    }
    
    if (self.accessBtn.tag==1) {
        aa = [aa stringByReplacingCharactersInRange:NSMakeRange(6, 1) withString:@"1"];
        if (self.lockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(16, 1) withString:@"1"];
        }
        if (self.longLockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(17, 1) withString:@"1"];
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(7, 1) withString:@"1"];
        }
    }
    
    if (self.killBtn.tag==1) {
        aa = [aa stringByReplacingCharactersInRange:NSMakeRange(4, 1) withString:@"1"];
        if (self.lockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(14, 1) withString:@"1"];
        }
        if (self.longLockBtn.tag==1) {
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(15, 1) withString:@"1"];
            aa = [aa stringByReplacingCharactersInRange:NSMakeRange(5, 1) withString:@"1"];
        }
    }
    
    NSLog(@"aa====%@",aa);
    NSString *bb=[self getHexByBinary:aa];
    NSLog(@"bb===%@",bb);
    self.returnBlock(bb);

    
}
- (NSString *)getHexByBinary:(NSString *)binary {
    
    NSMutableDictionary *binaryDic = [[NSMutableDictionary alloc] initWithCapacity:16];
    [binaryDic setObject:@"0" forKey:@"0000"];
    [binaryDic setObject:@"1" forKey:@"0001"];
    [binaryDic setObject:@"2" forKey:@"0010"];
    [binaryDic setObject:@"3" forKey:@"0011"];
    [binaryDic setObject:@"4" forKey:@"0100"];
    [binaryDic setObject:@"5" forKey:@"0101"];
    [binaryDic setObject:@"6" forKey:@"0110"];
    [binaryDic setObject:@"7" forKey:@"0111"];
    [binaryDic setObject:@"8" forKey:@"1000"];
    [binaryDic setObject:@"9" forKey:@"1001"];
    [binaryDic setObject:@"A" forKey:@"1010"];
    [binaryDic setObject:@"B" forKey:@"1011"];
    [binaryDic setObject:@"C" forKey:@"1100"];
    [binaryDic setObject:@"D" forKey:@"1101"];
    [binaryDic setObject:@"E" forKey:@"1110"];
    [binaryDic setObject:@"F" forKey:@"1111"];
    
    if (binary.length % 4 != 0) {
        
        NSMutableString *mStr = [[NSMutableString alloc]init];;
        for (int i = 0; i < 4 - binary.length % 4; i++) {
            
            [mStr appendString:@"0"];
        }
        binary = [mStr stringByAppendingString:binary];
    }
    NSString *hex = @"";
    for (int i=0; i<binary.length; i+=4) {
        
        NSString *key = [binary substringWithRange:NSMakeRange(i, 4)];
        NSString *value = [binaryDic objectForKey:key];
        if (value) {
            
            hex = [hex stringByAppendingString:value];
        }
    }
    return hex;
}

@end
