//
//  UpgradeView.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "UpgradeView.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue

@implementation UpgradeView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
       // self.backgroundColor=[UIColor redColor];
        
        
        self.fileText=[UITextField new];
        [self addSubview:self.fileText];
        [self.fileText mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(self).offset(20);
            make.right.equalTo(self).offset(-115);
            make.height.mas_equalTo(28);
        }];
        
        UIView *linView=[UIView new];
        [self addSubview:linView];
        linView.backgroundColor=RGB(180, 180, 180, 1);
        [linView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.fileText);
            make.top.equalTo(self.fileText.mas_bottom);
            make.height.mas_equalTo(2);
        }];
        
        self.chooseBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.chooseBtn];
        self.chooseBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.chooseBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        [self.chooseBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.chooseBtn addTarget:self action:@selector(choosefile) forControlEvents:UIControlEventTouchUpInside];
        [self.chooseBtn setTitle:@"Choose file" forState:UIControlStateNormal];
        [self.chooseBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.right.equalTo(self).offset(-5);
            make.left.equalTo(linView.mas_right).offset(10);
            make.top.equalTo(self).offset(18);
            make.height.mas_equalTo(38);
        }];
        
        self.radioBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.radioBtn];
       // self.radioBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.radioBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        self.radioBtn.layer.masksToBounds=YES;
        self.radioBtn.layer.borderWidth=2;
        self.radioBtn.layer.cornerRadius=15;
        self.radioBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [self.radioBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.radioBtn addTarget:self action:@selector(radioBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.radioBtn setTitle:@"R2000" forState:UIControlStateNormal];
        [self.radioBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(20);
            make.top.equalTo(linView.mas_bottom).offset(25);
            make.width.mas_equalTo(90);
            make.height.mas_equalTo(35);
        }];
        
        
        self.boardBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.boardBtn];
        // self.radioBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.boardBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        self.boardBtn.layer.masksToBounds=YES;
        self.boardBtn.layer.borderWidth=2;
        self.boardBtn.layer.cornerRadius=15;
        self.boardBtn.layer.borderColor=RGB(220, 220, 220, 1).CGColor;
        [self.boardBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.boardBtn addTarget:self action:@selector(boardBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.boardBtn setTitle:@"STM32" forState:UIControlStateNormal];
        [self.boardBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.radioBtn.mas_right).offset(20);
            make.top.bottom.equalTo(self.radioBtn);
            make.width.mas_equalTo(90);
        }];
        
        
        
        
        
        
        self.upgradeBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.upgradeBtn];
        self.upgradeBtn.backgroundColor=RGB(210, 210, 210, 1);
         self.upgradeBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        [self.upgradeBtn addTarget:self action:@selector(upgradeBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.upgradeBtn setTitle:@"Update" forState:UIControlStateNormal];
        [self.upgradeBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.upgradeBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.radioBtn.mas_bottom).offset(40);
            make.height.mas_equalTo(42);
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
        }];
        
        self.readVersonBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.readVersonBtn];
        self.readVersonBtn.titleLabel.font=[UIFont systemFontOfSize:15];
        self.readVersonBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.readVersonBtn addTarget:self action:@selector(readVersonBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.readVersonBtn setTitle:@"stm32 version" forState:UIControlStateNormal];
        [self.readVersonBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.readVersonBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.upgradeBtn.mas_bottom).offset(20);
            make.height.mas_equalTo(42);
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
        }];
        
      
        
        
        
        
        
    }
    return self;
}
-(void)radioBtnn
{
    self.radioBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
    self.boardBtn.layer.borderColor=RGB(220, 220, 220, 1).CGColor;
}

-(void)boardBtnn
{
    self.boardBtn.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
    self.radioBtn.layer.borderColor=RGB(220, 220, 220, 1).CGColor;
}
-(void)choosefile
{
    
}
-(void)readVersonBtnn
{
    
}
-(void)upgradeBtnn
{
    if (self.upgradeBlock) {
        self.upgradeBlock();
    }
}
@end
