//
//  TopView.h
//  RFID_ios
//
//  Created by  on 2018/4/26.
//  Copyright © 2018年 . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface TopView : UIView
@property (nonatomic,strong)UILabel *stateLab;
@property (nonatomic,strong)UIButton *connectBtn;
//@property (nonatomic,strong)UIButton *searchBtn;

@property (nonatomic,copy)void (^connectBlock)(void);
//@property (nonatomic,copy)void (^searchBlock)(void);


@end
