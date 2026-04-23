//
//  USERView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "USERView.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation USERView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        _writeLab=[UILabel new];
        [self addSubview:_writeLab];
        _writeLab.text=@"写加密数据：";
        _writeLab.font=[UIFont systemFontOfSize:16];
        _writeLab.textColor=RGB(150, 150, 150, 1);
        [_writeLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.top.equalTo(self).offset(10);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(225);
        }];
        self.writeField=[UITextField new];
        [self addSubview:self.writeField];
        self.writeField.font=[UIFont systemFontOfSize:16];
        [self.writeField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
            make.top.equalTo(_writeLab.mas_bottom).offset(10);
            make.height.mas_equalTo(35);
        }];
        UIView *linView=[UIView new];
        [self addSubview:linView];
        linView.backgroundColor=RGB(150, 150, 150, 1);
        [linView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.writeField.mas_bottom).offset(5);
            make.left.right.equalTo(self.writeField);
            make.height.mas_equalTo(1);
        }];
        
        _readLab=[UILabel new];
        [self addSubview:_readLab];
        _readLab.text=@"读加密数据：";
        _readLab.font=[UIFont systemFontOfSize:16];
        _readLab.textColor=RGB(150, 150, 150, 1);
        [_readLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(linView.mas_bottom).offset(10);
            make.left.right.equalTo(_writeLab);
            make.height.mas_equalTo(AdaptH(25));
        }];
        self.readField=[UITextField new];
        [self addSubview:self.readField];
        self.readField.font=[UIFont systemFontOfSize:16];
        [self.readField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
            make.top.equalTo(_readLab.mas_bottom).offset(10);
            make.height.mas_equalTo(35);
        }];
        UIView *linView1=[UIView new];
        [self addSubview:linView1];
        linView1.backgroundColor=RGB(150, 150, 150, 1);
        [linView1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.readField.mas_bottom).offset(5);
            make.left.right.equalTo(self.readField);
            make.height.mas_equalTo(1);
        }];

        UILabel *addressLab=[UILabel new];
        [self addSubview:addressLab];
        addressLab.text=@"起始地址";
        addressLab.font=[UIFont systemFontOfSize:15];
        addressLab.textColor=RGB(150, 150, 150, 1);
        [addressLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(linView1.mas_bottom).offset(25);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(75);
        }];
        self.addressField=[UITextField new];
        [self addSubview:self.addressField];
        self.addressField.font=[UIFont systemFontOfSize:16];
        [self.addressField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(addressLab.mas_right);
            make.centerY.equalTo(addressLab);
            make.height.mas_equalTo(30);
            make.width.mas_equalTo(AdaptH(100));
        }];
        UIView *linView2=[UIView new];
        [self addSubview:linView2];
        linView2.backgroundColor=RGB(150, 150, 150, 1);
        [linView2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.addressField.mas_bottom);
            make.left.right.equalTo(self.addressField);
            make.height.mas_equalTo(1);
        }];

        UILabel *lengthLab=[UILabel new];
        [self addSubview:lengthLab];
        lengthLab.text=@"数据长度";
        lengthLab.font=[UIFont systemFontOfSize:15];
        lengthLab.textColor=RGB(150, 150, 150, 1);
        [lengthLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset([UIScreen mainScreen].bounds.size.width/2.0);
            make.top.bottom.equalTo(addressLab);
            make.width.mas_equalTo(75);
        }];
        self.lengthField=[UITextField new];
        [self addSubview:self.lengthField];
        self.lengthField.font=[UIFont systemFontOfSize:16];
        [self.lengthField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(lengthLab.mas_right);
            make.centerY.equalTo(lengthLab);
            make.height.mas_equalTo(30);
            make.width.mas_equalTo(AdaptH(100));
        }];
        UIView *linView3=[UIView new];
        [self addSubview:linView3];
        linView3.backgroundColor=RGB(150, 150, 150, 1);
        [linView3 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.lengthField.mas_bottom);
            make.left.right.equalTo(self.lengthField);
            make.height.mas_equalTo(1);
        }];


        UIButton *writeBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:writeBtn];
        [writeBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [writeBtn setTitle:@"写" forState:UIControlStateNormal];
        writeBtn.backgroundColor=RGB(210, 210, 210, 1);
        [writeBtn addTarget:self action:@selector(writeBtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat width=([UIScreen mainScreen].bounds.size.width-20-30)/2.0;
        [writeBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(linView3.mas_bottom).offset(15);
            make.height.mas_equalTo(AdaptH(40));
            make.width.mas_equalTo(width);
        }];

        UIButton *readBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:readBtn];
        [readBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [readBtn setTitle:@"读" forState:UIControlStateNormal];
        readBtn.backgroundColor=RGB(210, 210, 210, 1);
        [readBtn addTarget:self action:@selector(readBtnn) forControlEvents:UIControlEventTouchUpInside];
        [readBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(writeBtn);
            make.right.equalTo(self).offset(-10);
            make.width.mas_equalTo(width);
        }];
        
        
    }
    return self;
}
-(void)writeBtnn
{
    if (self.writeBlock) {
        self.writeBlock();
    }
}
-(void)readBtnn
{
    if (self.readBlock) {
        self.readBlock();
    }
}
@end
