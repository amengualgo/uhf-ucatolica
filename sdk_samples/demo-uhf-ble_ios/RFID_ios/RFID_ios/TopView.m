//
//  TopView.m
//  RFID_ios
//
//  Created by  on 2018/4/26.
//  Copyright © 2018年 . All rights reserved.
//

#import "TopView.h"
#import <Masonry.h>
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]

@implementation TopView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        
        UIView *topView=[UIView new];
        [self addSubview:topView];
        topView.backgroundColor=[UIColor blueColor];
        [topView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.top.equalTo(self);
            make.height.mas_equalTo(AdaptH(65));
        }];
        
        UILabel *topLab=[UILabel new];
        [topView addSubview:topLab];
        topLab.text=@"RFID_ios";
        topLab.textColor=[UIColor whiteColor];
        topLab.font=[UIFont boldSystemFontOfSize:18];
        [topLab mas_makeConstraints:^(MASConstraintMaker *make) {
//            make.centerY.equalTo(topView);
            make.left.equalTo(topView).offset(15);
            make.top.equalTo(topView.mas_top).offset(25);
            make.height.mas_equalTo(30);
            make.width.mas_equalTo(150);
        }];
        
        
        UIView *bottomView=[UIView new];
        [self addSubview:bottomView];
        bottomView.backgroundColor=RGB(200, 200, 200, 1);
        [bottomView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self);
            make.top.equalTo(topView.mas_bottom);
            make.height.mas_equalTo(AdaptH(52));
        }];
        
        _connectBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [bottomView addSubview:_connectBtn];
        [_connectBtn setTitle:@"Connect" forState:UIControlStateNormal];
        _connectBtn.layer.borderWidth=1;
        _connectBtn.backgroundColor=RGB(236, 236, 236, 1);
        [_connectBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        _connectBtn.titleLabel.font=[UIFont systemFontOfSize:17];
        _connectBtn.layer.borderColor=RGB(170, 170, 170, 1).CGColor;
        [_connectBtn addTarget:self action:@selector(connectBtnn) forControlEvents:UIControlEventTouchUpInside];
        [_connectBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(8);
            make.top.equalTo(topView.mas_bottom).offset(AdaptH(6));
            make.width.mas_equalTo(AdaptH(120));
            make.height.mas_equalTo(AdaptH(40));
        }];
        
        
        self.stateLab=[UILabel new];
        [bottomView addSubview:self.stateLab];
        self.stateLab.textColor=RGB(130, 130, 130, 1);
        self.stateLab.font=[UIFont systemFontOfSize:19];
        self.stateLab.text=@"Device: Not Connected";
        [self.stateLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.connectBtn.mas_right).offset(10);
            make.top.equalTo(self.connectBtn.mas_top);
            make.height.mas_equalTo(AdaptH(40));
//            make.top.equalTo(topView.mas_bottom).offset(AdaptH(6));
//            make.right.equalTo(self).offset(-15);
        }];
        
        
//        _searchBtn=[UIButton buttonWithType:UIButtonTypeCustom];
//        [bottomView addSubview:_searchBtn];
//        [_searchBtn setTitle:@"Search" forState:UIControlStateNormal];
//        _searchBtn.layer.borderWidth=1;
//        _searchBtn.backgroundColor=RGB(236, 236, 236, 1);
//        [_searchBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
//        _searchBtn.titleLabel.font=[UIFont systemFontOfSize:17];
//        _searchBtn.layer.borderColor=RGB(170, 170, 170, 1).CGColor;
//        [_searchBtn addTarget:self action:@selector(searchBtnn) forControlEvents:UIControlEventTouchUpInside];
//        [_searchBtn mas_makeConstraints:^(MASConstraintMaker *make) {
//            make.right.equalTo(self).offset(-15);
//            make.top.bottom.equalTo(_connectBtn);
//            make.width.mas_equalTo(AdaptH(160));
//            
//        }];
        
        

    }
    return self;
}
-(void)connectBtnn
{
    if (self.connectBlock) {
        self.connectBlock();
    }
}
//-(void)searchBtnn
//{
//    if (self.searchBlock) {
//        self.searchBlock();
//    }
//}
@end
